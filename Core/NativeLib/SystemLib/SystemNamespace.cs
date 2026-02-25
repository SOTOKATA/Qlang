using Core.NativeLib.SystemLib.Classes;

namespace Core.NativeLib.SystemLib;

public class SystemNamespace  : IQlangNamespace
{
    public string Name { get; set; } = "std";
    public List<IQlangClass> Classes { get; set; } = [ 
        new ArrayClass(), new ConsoleClass(), new EnvironmentClass(),
        new DateTimeClass(), new ExceptionClass(), new FileSystemClass(),
        new NumberClass(), new ObjectClass(), new ParserClass(),
        new RegexClass(), new StringClass(), new MathClass(), new MetaClass(), 
        new MetaClassClass()
    ];
}