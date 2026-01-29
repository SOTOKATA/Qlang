using MessagePack;

namespace Core;

public class MessagePack
{
    public static byte[] Serialize(object obj)
    {
        return MessagePackSerializer.Serialize(obj);
    }
    
    public static T Deserialize<T>(byte[] bytes)
    {
        return MessagePackSerializer.Deserialize<T>(bytes);
    }
}