namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class EncounterData
    {
        public EncounterData(byte[] data, ushort encounder_id, Dictionary<ushort, EnemyData> enemies = null)
        {
            EncounterID = encounder_id;
            Unknown_00 = BitConverter.ToUInt32(data, 0);
            Unknown_04 = BitConverter.ToUInt32(data, 4);

            Enemies = new EnemyData[8];
            EnemiesID = new ushort[8];
            for (int i = 0; i < 8; i++)
            {
                ushort enemy_id = BitConverter.ToUInt16(data, 08 + (i * 2));
                EnemiesID[i] = enemy_id;

                if (enemy_id == 0)
                    continue;

                if (enemies == null)
                    continue;

                Enemies[i] = enemies[enemy_id];
            }
        }

        public override string ToString()
        {
            return string.Join(',', Enemies.Where(w => w != null).Select(s=> s?.ToString()));
        }

        public ushort EncounterID;
        public uint Unknown_00;
        public uint Unknown_04;
        public EnemyData[] Enemies;
        public ushort[] EnemiesID;
    }
}
