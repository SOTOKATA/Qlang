using System.Diagnostics;
using Core.Exceptions;

namespace Core.NativeLib.SystemLib.Classes;

public class ConsoleCommandClass : IQlangClass
{
    public string Name { get; init; } = "console_command";
    
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

            using var process = Process.Start(psi);
            
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            Console.WriteLine(output);

            if (!string.IsNullOrEmpty(error))
                throw new QlangRuntimeException(error, null);
        })
        ];
    }
}