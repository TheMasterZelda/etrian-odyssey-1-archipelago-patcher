using etrian_odyssey_ap_patcher.DataCompression;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Files
{
    public class EventFile
    {

        // 00-03 = 06 00 00 00
        // 04-07 = number of entries?
        // 08-0B = Unknown, ID? counter?
        // 0C-0F = EE EE EE EE
        // 10-> start of events block
        // each event is 0x18

        public bool IsNestedFile { get; private set; }
        public MemoryStream DataStream { get; set; }
        public byte[] FileData { get; set; }
        public CompressionType CompressionType { get; private set; }

        public EventFile(byte[] fileData, CompressionType compressionType, bool isNestedFile)
        {
            IsNestedFile = isNestedFile;
            FileData = fileData;
            CompressionType = compressionType;
            DataStream = new MemoryStream(fileData);

            //string magicNumber = Encoding.ASCII.GetString(fileData, 0, 4);

            //if (magicNumber != MagicNumber)
            //    throw new Exception($"Invalid file signature. Expected {MagicNumber} but was {magicNumber}.");

            Parse();
        }

        public EventFile(byte[] fileData) : this(CompressionHelper.DetectCompressionTypeAndDecompressIfRequired(fileData, out var compressionType), compressionType, false)
        {
        }

        public void Parse()
        {

        }
    }
}
