using System.IO;
using System.IO.Compression;

namespace QuotationDance.Collector.Decompress
{
    public class DeflateDecompress : IDecompress
    {
        public string Decompress(byte[] bytes)
        {
            using var decompressedStream = new MemoryStream();
            using var compressedStream = new MemoryStream(bytes);
            using var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress);

            deflateStream.CopyTo(decompressedStream);
            decompressedStream.Position = 0;
            using var streamReader = new StreamReader(decompressedStream);
            return streamReader.ReadToEnd();
        }
    }
}