using System.Diagnostics;
using Core.Exceptions;

namespace Core.NativeLib.SystemLib;

public class ConsoleCommandLib : IQlangLib
{
    public string Name { get; } = "ConsoleCommandLib";
    public string Version { get; } = "0.0.0";
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "console_command";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
        ("run", (string command) =>
        {
            ProcessStartInfo psi = new()
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process? process = Process.Start(psi);
            
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            Console.WriteLine(output);

            if (!string.IsNullOrEmpty(error))
                throw new QlangRuntimeException(error, null);
        })
        ];
    }
}