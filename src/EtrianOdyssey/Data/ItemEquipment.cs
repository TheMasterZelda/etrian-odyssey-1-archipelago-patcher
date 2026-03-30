namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public enum EquipmentType : byte
    {
        Unknown = 0x00,
        Sword = 0x01,
        Staff = 0x02,
        Axe = 0x03,
        Katana = 0x04,
        Bow = 0x05,
        Whip = 0x06,
        Armor = 0x08,
        Shield = 0x09,
        Headgear = 0x0A,
        Gloves = 0x0B,
        Boots = 0x0C,
        Accessory = 0x0D
    }

    public enum DamageType : byte
    {
        Slash = 00,
        Bash = 01,
        Pierce = 02,
        Fire = 03,
        Ice = 04,
        Volt = 05,
        None = 06
    }

    public class ItemEquipment
    {
        public ItemEquipment(byte[] data, EtrianString[] nameTable)
        {
            item_id = BitConverter.ToUInt16(data, 0);
            name = nameTable[item_id - 1];

            equipment_type = data[0x0A];

            damage_type = data[02];
            secondary_damage_type = data[03];
            attack_1 = BitConverter.ToUInt16(data, 04);
            defense = BitConverter.ToUInt16(data, 08);

            weapon_speed_modifier = data[0x0B]; // Unsure. 0B
            slash_reduction = data[0x0C];
            blunt_reduction = data[0x0D];
            pierce_reduction = data[0x0E];
            fire_reduction = data[0x0F];
            ice_reduction = data[0x10];
            volt_reduction = data[0x11];
            death_reduction = data[0x12];
            ailment_reduction = data[0x13];
            head_bind_reduction = data[0x14];
            arm_bind_reduction = data[0x15];
            leg_bind_reduction = data[0x16];
            bonus_str = data[0x17];
            bonus_vit = data[0x18];
            bonus_agi = data[0x19];
            bonus_luc = data[0x1A];
            bonus_tec = data[0x1B];
            bonus_hp = data[0x1C];
            bonus_tp = data[0x1D];
            bonus_bp = data[0x1E];
            unknown_1f = data[0x1F]; // always 0x00.
            buy_price = BitConverter.ToUInt32(data, 0x20); // 0x20-0x23
            sell_price = BitConverter.ToUInt32(data, 0x24); ; // 0x24-0x27
            usable_by = data[0x28]; ; // 0x28
            usable_by_2_and_unknown = data[0x29]; ; // 0x29
            unknown_2A = data[0x2A]; ; // 0x2A
            unknown_2B = data[0x2B]; ; // 0x2B
        }

        public override string ToString()
        {
            return name.StringValue;
        }

        public EquipmentType EquipmentType => (EquipmentType)equipment_type;
        public DamageType DamageType => (DamageType)damage_type;
        public DamageType SecondaryDamageType => (DamageType)secondary_damage_type;

        public ushort item_id; // 00-01
        public EtrianString name;
        public byte damage_type; // 02
        public byte secondary_damage_type; // 03
        public ushort attack_1; // 04-05
        public ushort attack_2; // 06-07
        public ushort defense; // 08-09
        public byte equipment_type; // 0A
        public byte weapon_speed_modifier; // Unsure. 0B
        public byte slash_reduction;
        public byte blunt_reduction;
        public byte pierce_reduction;
        public byte fire_reduction;
        public byte ice_reduction;
        public byte volt_reduction;
        public byte death_reduction;
        public byte ailment_reduction;
        public byte head_bind_reduction;
        public byte arm_bind_reduction;
        public byte leg_bind_reduction;
        public byte bonus_str;
        public byte bonus_vit;
        public byte bonus_agi;
        public byte bonus_luc;
        public byte bonus_tec;
        public byte bonus_hp;
        public byte bonus_tp;
        public byte bonus_bp;
        public byte unknown_1f; // always 0x00.
        public uint buy_price; // 0x20-0x23
        public uint sell_price; // 0x24-0x27
        public byte usable_by; // 0x28
        public byte usable_by_2_and_unknown; // 0x29
        public byte unknown_2A; // 0x2A
        public byte unknown_2B; // 0x2B
    }
}
