namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class LevelUpData0Sub
    {
        public LevelUpData0Sub(byte[] data) 
        {
            hp = data[0];
            tp = data[1];
            str = data[2];
            vit = data[3];
            agi = data[4];
            luc = data[5];
            tec = data[6];
        }

        public byte hp;
        public byte tp;
        public byte str;
        public byte vit;
        public byte agi;
        public byte luc;
        public byte tec;
    }

    public class LevelUpData0
    {
        public LevelUpData0(byte[] data)
        {
            stat_per_class = new LevelUpData0Sub[9];

            for (int i = 0; i < stat_per_class.Length; i++)
            {
                byte[] sub_data = data.Skip(i * 7).Take(7).ToArray();
                stat_per_class[i] = new LevelUpData0Sub(sub_data);
            }
        }

        public LevelUpData0Sub[] stat_per_class;
    }
}
