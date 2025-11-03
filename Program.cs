using Qlang;
using Qlang.AST;
using Qlang.Compiler;

string functionScript = """
                        function main():
                            // Comment
                            $var = "Hello World!"
                            printLine($var)
                            
                            if $var == "Hello World!":
                                Term.print("Hello!")
                            else if $var == "":
                                Term.print("You type nothing...")
                            else:
                                Term.print("You typed: ", $var)
                        function printLine(line):
                            Term.print(line)
                        """;


var lang = new QLang();
lang.Compile(functionScript);
lang.Run();