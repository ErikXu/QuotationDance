using System;
using System.IO;
using System.IO.Compression;

namespace QuotationDance.Collector.Decompress
{
    public class GZipDecompress : IDecompress
    {
        public string Decompress(byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            using var buffer = new MemoryStream();
            var block = new byte[1024];
            while (true)
            {
                var bytesRead = gzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                buffer.Write(block, 0, bytesRead);
            }
            gzipStream.Close();
            return System.Text.Encoding.UTF8.GetString(buffer.ToArray());
        }
    }
}