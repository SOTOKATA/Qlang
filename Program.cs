using System.Globalization;

namespace Qlang;

public class Program
{
    public static void Main(string[] args)
    {
        // to double write with dot (not comma)
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        if (args.Length < 1)
        {
             Console.WriteLine($"""
                                Help (command structure):
                                Qlang.exe [file-path] --[arg]
                                
                                To run code example type "Qlang.exe --test"
                                To change settings use "--s" and write settings name and new value
                                To get current settings value write "--s" and name of setting
                                """);
             return;
        }

        var filePath = args[0];
        
        string? code = null;

        if (filePath == "--test")
        {
            Console.WriteLine("Test command detected. Running demonstration...");
            code = """
                   function main():
                       Term.print("Type your name: ")
                       $name = Term.read()
                       
                       Term.println("Hello, ", $name, "!")
                   """;
        }
        
        QLang lang = new();

        switch (args)
        {
            case ["--s", _, _, ..]:
                lang.SetSettings(args[1].Trim(), args[2].Trim());
                return;
            case ["--s", _, ..]:
                lang.SetSettings(args[1].Trim(), null);   
                return;
        }

        if (File.Exists(filePath))
            code = File.ReadAllText(filePath);
        else if (code == null)
        {
            Console.WriteLine("File not found: " + filePath);
            return;
        }
        
        lang.Compile(code);
        lang.Run();
    }
}


