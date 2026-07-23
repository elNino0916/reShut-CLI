using System.ComponentModel;
using System.Runtime.InteropServices;
using reShutCLI.Helpers;

namespace reShutCLI.Services;

/// <summary>
/// Performs shutdown, reboot and logoff via the Win32 API.
///
/// None of these require an elevated process. SeShutdownPrivilege is granted to the
/// Users group by default on Windows client SKUs - it is simply disabled in the token
/// until explicitly enabled, which is what <see cref="EnableShutdownPrivilege"/> does.
/// (Windows Server does not grant it to Users, so the call can still legitimately fail
/// with ERROR_NOT_ALL_ASSIGNED there.)
/// </summary>
internal static partial class PowerManager
{
    /// <summary>
    /// The shutdown/restart entry point, and the same one shutdown.exe drives.
    ///
    /// Deliberately not ExitWindowsEx: that returns TRUE the moment a shutdown is
    /// *initiated* and then proceeds asynchronously, so a veto afterwards leaves the
    /// caller believing it succeeded while nothing happens. EWX_FORCE only suppresses
    /// the veto for processes the caller is able to terminate, which a non-elevated
    /// process cannot do to anything running at higher integrity or as another user -
    /// hence "works sometimes without admin, always with admin". InitiateShutdown
    /// reports a real Win32 error code instead of failing silently.
    /// </summary>
    [LibraryImport("advapi32.dll", StringMarshalling = StringMarshalling.Utf16)]
    private static partial uint InitiateShutdownW(string? lpMachineName, string? lpMessage,
        uint dwGracePeriod, uint dwShutdownFlags, uint dwReason);

    /// <summary>Cancels a shutdown that is still inside its grace period.</summary>
    [LibraryImport("advapi32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AbortSystemShutdownW(string? lpMachineName);

    /// <summary>Still the correct call for logoff, which InitiateShutdown does not cover.</summary>
    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ExitWindowsEx(uint uFlags, uint dwReason);

    [LibraryImport("kernel32.dll")]
    private static partial IntPtr GetCurrentProcess();

    [LibraryImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

    [LibraryImport("advapi32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool LookupPrivilegeValueW(string? lpSystemName, string lpName, out LUID luid);

    [LibraryImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AdjustTokenPrivileges(IntPtr tokenHandle, [MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges,
        ref TOKEN_PRIVILEGES newState, uint bufferLength, IntPtr previousState, IntPtr returnLength);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CloseHandle(IntPtr hObject);

    private const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
    private const uint TOKEN_QUERY = 0x0008;
    private const uint SE_PRIVILEGE_ENABLED = 0x00000002;
    private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

    private const uint EWX_LOGOFF = 0x00000000;

    private const uint SHUTDOWN_FORCE_OTHERS = 0x00000001;
    private const uint SHUTDOWN_FORCE_SELF = 0x00000002;
    private const uint SHUTDOWN_RESTART = 0x00000004;
    private const uint SHUTDOWN_POWEROFF = 0x00000008;

    /// <summary>Planned + "other", so this does not land in the event log as an unexpected shutdown.</summary>
    private const uint SHTDN_REASON_PLANNED_OTHER = 0x80000000;

    private const uint ERROR_SUCCESS = 0;
    private const uint ERROR_ACCESS_DENIED = 5;
    private const uint ERROR_SHUTDOWN_IN_PROGRESS = 1115;
    private const uint ERROR_NO_SHUTDOWN_IN_PROGRESS = 1116;
    private const uint ERROR_NOT_ALL_ASSIGNED = 1300;

    [StructLayout(LayoutKind.Sequential)]
    private struct LUID
    {
        public uint LowPart;
        public int HighPart;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct TOKEN_PRIVILEGES
    {
        public uint PrivilegeCount;
        public LUID Luid;
        public uint Attributes;
    }

    /// <summary>
    /// Enables SeShutdownPrivilege in this process's token, returning the Win32 error
    /// code (ERROR_SUCCESS when the privilege is now enabled).
    /// </summary>
    private static uint EnableShutdownPrivilege()
    {
        // The pseudo-handle from GetCurrentProcess() needs no cleanup, unlike
        // Process.GetCurrentProcess(), which allocates a real handle that was leaked here.
        if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out var tokenHandle))
            return (uint)Marshal.GetLastWin32Error();

        try
        {
            if (!LookupPrivilegeValueW(null, SE_SHUTDOWN_NAME, out var luid))
                return (uint)Marshal.GetLastWin32Error();

            var tp = new TOKEN_PRIVILEGES
            {
                PrivilegeCount = 1,
                Luid = luid,
                Attributes = SE_PRIVILEGE_ENABLED,
            };

            if (!AdjustTokenPrivileges(tokenHandle, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero))
                return (uint)Marshal.GetLastWin32Error();

            // AdjustTokenPrivileges reports success as long as the *call* was well formed,
            // even when it enabled nothing at all - it cannot add a privilege the token
            // does not already hold. Only GetLastError distinguishes the two, and without
            // this check a missing privilege is indistinguishable from a granted one.
            return (uint)Marshal.GetLastWin32Error();
        }
        finally
        {
            CloseHandle(tokenHandle);
        }
    }

    private static void InitiateShutdown(uint shutdownFlags, uint graceSeconds)
    {
        var privilegeResult = EnableShutdownPrivilege();
        if (privilegeResult != ERROR_SUCCESS)
        {
            ErrorHandler.ShowError(privilegeResult == ERROR_NOT_ALL_ASSIGNED
                ? "This account does not hold the \"Shut down the system\" right (SeShutdownPrivilege). " +
                  "It may have been removed by policy, or this is a Windows Server install, where it is " +
                  "not granted to standard users by default."
                : $"Failed to enable shutdown privilege: {Describe(privilegeResult)}", true);
            return;
        }

        // FORCE_OTHERS/FORCE_SELF preserve the old EWX_FORCE behaviour: applications do
        // not get to veto the shutdown. A non-zero grace period is what "shutdown /t"
        // does - Windows shows its own countdown and the shutdown stays abortable
        // through AbortPendingShutdown until it elapses.
        var result = InitiateShutdownW(null, null, graceSeconds,
            shutdownFlags | SHUTDOWN_FORCE_OTHERS | SHUTDOWN_FORCE_SELF, SHTDN_REASON_PLANNED_OTHER);

        if (result == ERROR_SUCCESS) return;

        ErrorHandler.ShowError(result switch
        {
            ERROR_ACCESS_DENIED =>
                "Access denied. Another process is blocking shutdown at a higher privilege level, " +
                "or policy prevents this account from shutting the system down.",
            ERROR_SHUTDOWN_IN_PROGRESS =>
                "A shutdown is already in progress.",
            _ => $"Shutdown failed: {Describe(result)}",
        }, true);
    }

    private static string Describe(uint error) => $"{new Win32Exception((int)error).Message} (error {error})";

    // Separate overloads rather than one optional parameter: these are passed around as
    // Action method groups (see Program.ConfirmAndRun), and a method group with an
    // optional parameter no longer converts to Action.
    public static void Reboot() => InitiateShutdown(SHUTDOWN_RESTART, 0);

    public static void Shutdown() => InitiateShutdown(SHUTDOWN_POWEROFF, 0);

    /// <param name="delaySeconds">Grace period before the machine goes down.</param>
    public static void Reboot(uint delaySeconds) => InitiateShutdown(SHUTDOWN_RESTART, delaySeconds);

    /// <param name="delaySeconds">Grace period before the machine goes down.</param>
    public static void Shutdown(uint delaySeconds) => InitiateShutdown(SHUTDOWN_POWEROFF, delaySeconds);

    /// <summary>
    /// Cancels a shutdown still within its grace period. Returns true when one was
    /// actually cancelled; "nothing was pending" is reported as false rather than as
    /// an error, since asking to cancel when nothing is scheduled is not a failure.
    /// </summary>
    public static bool AbortPendingShutdown()
    {
        // Cancelling is gated on SeShutdownPrivilege exactly like initiating is; without
        // enabling it first this returns ERROR_ACCESS_DENIED.
        var privilegeResult = EnableShutdownPrivilege();
        if (privilegeResult != ERROR_SUCCESS)
        {
            ErrorHandler.ShowError($"Failed to enable shutdown privilege: {Describe(privilegeResult)}", true);
            return false;
        }

        if (AbortSystemShutdownW(null)) return true;

        var error = (uint)Marshal.GetLastWin32Error();
        if (error is ERROR_NO_SHUTDOWN_IN_PROGRESS) return false;

        ErrorHandler.ShowError(error == ERROR_ACCESS_DENIED
            ? "Access denied while cancelling the pending shutdown."
            : $"Could not cancel the pending shutdown: {Describe(error)}", true);
        return false;
    }

    public static void Logoff()
    {
        // Logoff does NOT require admin
        if (!ExitWindowsEx(EWX_LOGOFF, 0))
        {
            var err = Marshal.GetLastWin32Error();
            UIDraw.DrawLine($"Logoff failed with error {err}: {new Win32Exception(err).Message}");
        }
    }
}
