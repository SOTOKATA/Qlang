using Qlang;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
//             Console.WriteLine($"""
//                                Help (command structure):
//                                Qlang.exe [file-path] --[arg]
//                                
//                                To run code example type "Qlang.exe --test"
//                                """);
//             return;
            string dcode = """
                          function main():
                              Term.print("Type your name: ")
                              $name = Term.read()
                              
                              Term.println("Hello " + $name + "!")
                          """;
            
            QLang dlang = new();
            dlang.Compile(dcode);
            dlang.Run();

            return;
        }

        string filePath = args[0];
        
        string code;

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
        else 
            code = File.ReadAllText(filePath);
        
        QLang lang = new();
        lang.Compile(code);
        lang.Run();
    }
}

// string functionScript = """
//                         function main():
//                             // Comment
//                             Term.print("Type anything: ")
//                             $var = Term.read()
//                             
//                             this.printer($var)
//                             
//                             if $var == "Hello World!":
//                                 Term.println("Hello!")
//                             else if $var == "":
//                                 Term.println("You type nothing...")
//                             else:
//                                 Term.println("You typed: ", $var)
//                                 
//                         function printer($toprint):
//                             Term.println($toprint)
//                         """;


