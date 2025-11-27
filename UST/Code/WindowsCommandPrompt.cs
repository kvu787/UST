using System.Runtime.InteropServices;

namespace UST;

public static partial class WindowsCommandPrompt {
    [LibraryImport("kernel32.dll")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool AllocConsole();

}
