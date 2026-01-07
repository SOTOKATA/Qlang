using Core.NativeLib.SystemLib.Classes;

namespace Core.NativeLib.SystemLib;

public class SystemNamespace  : IQlangNamespace
{
    public string Name { get; set; } = "lib";
    public List<IQlangClass> Classes { get; set; } = [ 
    new ArrayClass(), new ConsoleClass(), new ConsoleCommandClass(),
    new DateTimeClass(), new ExceptionClass(), new FileSystemClass(),
    new MetaClass(), new NumberClass(), new ObjectClass(), new ParserClass(),
    new RegexClass(), new StringClass()
    ];
}