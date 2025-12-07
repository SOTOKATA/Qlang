namespace Qlang.Core.Lang.Interpreter.Native;

public class NativeList
{
    public static void IsList(object obj)
    {
        
    }
    
    public static List<object> Add(List<object> list, object item)
    {
        list.Add(item);

        return list;
    }
    
}