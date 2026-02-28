namespace etrian_odyssey_ap_patcher.DataCompression
{
    public enum CompressionMode { Decompress, Compress };

    public abstract class CompressedStream : MemoryStream
    {
        public CompressionMode CompressionMode { get; private set; }
        public MemoryStream OriginalStream { get; private set; }

        public CompressedStream(CompressionMode compressionMode)
        {
            CompressionMode = compressionMode;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (CompressionMode == CompressionMode.Decompress)
            {
                try
                {
                    OriginalStream = new MemoryStream(buffer, offset, count);
                    byte[] decompressed = Decompress(buffer, offset);
                    base.Write(decompressed, 0, decompressed.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} decompression failed.", GetType().Name), ex);
                }
            }
            else if (CompressionMode == CompressionMode.Compress)
            {
                try
                {
                    OriginalStream = new MemoryStream(buffer, offset, count);
                    byte[] compressed = Compress(buffer, offset, count);
                    base.Write(compressed, 0, compressed.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("{0} compression failed.", GetType().Name), ex);
                }
            }
        }

        public virtual byte[] Decompress(byte[] buffer, int sOffset)
        {
            throw new NotImplementedException(string.Format("Decompression not implemented for compression type {0}", GetType().FullName));
        }

        public virtual byte[] Compress(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException(string.Format("Compression not implemented for compression type {0}", GetType().FullName));
        }
    }
}
