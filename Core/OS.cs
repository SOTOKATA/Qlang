namespace Core;

public class OS
{
    public static string GetExecutableExtension()
    {
        if (OperatingSystem.IsWindows())
            return ".exe";
        if (OperatingSystem.IsMacOS())
            return ".app";
        if (OperatingSystem.IsLinux())
            return ".so";
        
        throw new PlatformNotSupportedException("Current OS is not supported.");
    }
}