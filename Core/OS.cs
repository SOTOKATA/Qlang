namespace Core;

public static class OS
{
    public static string GetExecutableExtension()
    {
        if (OperatingSystem.IsWindows())
            return ".exe";
        if (OperatingSystem.IsMacOS())
            return "";
        if (OperatingSystem.IsLinux())
            return ".so";
        
        throw new PlatformNotSupportedException("Current OS is not supported.");
    }
}