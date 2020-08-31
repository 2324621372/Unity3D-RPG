using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TableType
{
    Nono,
    PlayerTable,
    WeaponTable,
    MonsterTable,
    ArmorTable,
    EXPTable,
    QuestTable,
    AccessoryTable,
    PortionTable,
}

public static class DataMng
{
    private static Dictionary<TableType, LowBase> m_table
        = new Dictionary<TableType, LowBase>();

    public static void AddTable(TableType table)
    {
        if (m_table.ContainsKey(table) == false)
        {
            TextAsset textAsset =
                Resources.Load<TextAsset>("Table/" + table.ToString());

            LowBase lowBase = new LowBase();
            lowBase.Load(textAsset.text);

            m_table.Add(table, lowBase);
        }
    }

    public static LowBase Get(TableType table)
    {
        if (m_table.ContainsKey(table))
            return m_table[table];
        return null;
    }
}
