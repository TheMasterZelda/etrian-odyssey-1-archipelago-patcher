using System.Text;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Table
{
    public abstract class BaseTable
    {
        public abstract string MagicNumber { get; }


        public BaseTable(MemoryStream tableFileData, int offset)
        {
            tableFileData.Seek(offset, SeekOrigin.Begin);

            string table_signature = Encoding.ASCII.GetString(tableFileData.ToArray(), offset, 4);
            if (table_signature != MagicNumber)
                throw new Exception($"Invalid table signature. Expected {MagicNumber} but was {table_signature}.");

            Parse(tableFileData, offset);
        }

        protected abstract void Parse(MemoryStream tableFileData, int offset);

        public abstract byte[] Rebuild();

    }
}
