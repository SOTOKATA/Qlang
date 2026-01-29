using System.IO.Compression;
using System.Text;

namespace Core;

public static class Brotli
{
    public static byte[] Compress(object data)
    {
        var bytes = data switch
        {
            byte[] b => b,
            string s => Encoding.UTF8.GetBytes(s),
            _        => Encoding.UTF8.GetBytes(data.ToString()!)
        };

        using var output = new MemoryStream();
        using (var brotli = new BrotliStream(output, CompressionLevel.Optimal, leaveOpen: true))
            brotli.Write(bytes, 0, bytes.Length);

        return output.ToArray();
    }

    public static byte[] Decompress(byte[] compressed)
    {
        using var input = new MemoryStream(compressed);
        using var brotli = new BrotliStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();

        brotli.CopyTo(output);
        return output.ToArray();
    }
}