using System.IO.Compression;
using System.Text;

namespace Core;

public static class GZip
{
    public static byte[] Compress(string jsonSerialized)
    {
        var bytes = Encoding.UTF8.GetBytes(jsonSerialized);
    
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.Optimal))
            gzip.Write(bytes, 0, bytes.Length);
        
        return output.ToArray();
    }
    
    public static string Decompress(byte[] compressed)
    {
        using var input = new MemoryStream(compressed);
        using var gzip = new GZipStream(input, CompressionMode.Decompress);
        using var reader = new StreamReader(gzip);
    
        return reader.ReadToEnd();
    }
}