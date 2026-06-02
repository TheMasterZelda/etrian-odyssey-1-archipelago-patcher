using etrian_odyssey_ap_patcher.DataCompression;
using System.Text;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Files
{
    public abstract class BaseFile
    {
        public bool IsNestedFile { get; private set; }
        public MemoryStream DataStream { get; set; }
        public byte[] FileData { get; set; }
        public virtual string MagicNumber { get; }

        public CompressionType CompressionType { get; private set; }

        public BaseFile(byte[] fileData, CompressionType compressionType, bool isNestedFile)
        {
            IsNestedFile = isNestedFile;
            FileData = fileData;
            CompressionType = compressionType;
            DataStream = new MemoryStream(fileData);

            Parse();
        }

        public BaseFile(byte[] fileData) : this(CompressionHelper.DetectCompressionTypeAndDecompressIfRequired(fileData, out var compressionType), compressionType, false)
        {
        }

        protected void ValidateMagicNumber()
        {
            string magicNumber = Encoding.ASCII.GetString(FileData, 0, 4);

            if (magicNumber != MagicNumber)
                throw new Exception($"Invalid file signature. Expected {MagicNumber} but was {magicNumber}.");
        }

        public abstract void Parse();

        protected abstract byte[] Save();

        public byte[] GetUpdatedData()
        {
            byte[] newFileData = Save();

            if (CompressionType != CompressionType.None)
                newFileData = CompressionHelper.Compress(newFileData, CompressionType);

            return newFileData;
        }
    }
}
