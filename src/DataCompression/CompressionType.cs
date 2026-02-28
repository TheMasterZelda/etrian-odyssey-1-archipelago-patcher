namespace etrian_odyssey_ap_patcher.DataCompression
{
    public enum CompressionType : byte 
    { 
        None = 0,
        LZ77 = 0x10, 
        Huffman4Bit = 0x24, 
        Huffman8Bit = 0x28, 
        RLE = 0x30 
    };
}
