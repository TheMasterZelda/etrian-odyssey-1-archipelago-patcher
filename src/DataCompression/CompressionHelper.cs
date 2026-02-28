namespace etrian_odyssey_ap_patcher.DataCompression
{
    public static class CompressionHelper
    {
        public static byte[] Compress(byte[] fileData, CompressionType compressionType)
        {
            MemoryStream outputStream;

            switch (compressionType)
            {
                case CompressionType.LZ77:
                    outputStream = new LZ77Stream(CompressionMode.Compress);
                    outputStream.Write(fileData, 0, fileData.Length);
                    break;

                case CompressionType.Huffman4Bit:
                case CompressionType.Huffman8Bit:
                    throw new NotImplementedException();
                //outputStream = new HuffmanStream(CompressionMode.Decompress);
                //outputStream.Write(inputStream.ToArray(), offset, count);
                //break;

                case CompressionType.RLE:
                    throw new NotImplementedException();
                //outputStream = new RLEStream(CompressionMode.Decompress);
                //outputStream.Write(inputStream.ToArray(), offset, count);
                //break;

                default:
                    return fileData;
            }

            return outputStream.ToArray();
        }

        public static byte[] Decompress(byte[] fileData, CompressionType compressionType)
        {
            MemoryStream outputStream;

            switch (compressionType)
            {
                case CompressionType.LZ77:
                    outputStream = new LZ77Stream(CompressionMode.Decompress);
                    outputStream.Write(fileData, 0, fileData.Length);
                    break;

                case CompressionType.Huffman4Bit:
                case CompressionType.Huffman8Bit:
                    throw new NotImplementedException();
                //outputStream = new HuffmanStream(CompressionMode.Decompress);
                //outputStream.Write(inputStream.ToArray(), offset, count);
                //break;

                case CompressionType.RLE:
                    throw new NotImplementedException();
                //outputStream = new RLEStream(CompressionMode.Decompress);
                //outputStream.Write(inputStream.ToArray(), offset, count);
                //break;

                default:
                    return fileData;
            }

            return outputStream.ToArray();
        }

        public static byte[] DetectCompressionTypeAndDecompressIfRequired(byte[] fileData, out CompressionType compressionType)
        {
            compressionType = GetCompressionType(fileData);
            if (compressionType != CompressionType.None)
                fileData = Decompress(fileData, compressionType);

            return fileData;
        }

        public static CompressionType GetCompressionType(byte[] fileData)
        {
            return (CompressionType)fileData[0];
        }
    }
}
