using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    [HideInInspector]
    public UIOutput m_UIOutput;
    public GetQuset m_GetQuset;
    private MakeBoss m_MakeBoss;

    public void Init()
    {
        DataMng.AddTable(TableType.WeaponTable);
        DataMng.AddTable(TableType.ArmorTable);
        DataMng.AddTable(TableType.AccessoryTable);
        DataMng.AddTable(TableType.PortionTable);

        foreach (UIType t in Enum.GetValues(typeof(UIType)))
        {
            UIAdd.Load<Component>(t);
        }
        m_MakeBoss = FindObjectOfType<MonsterManager>();
    }

    public void Save(Inventory inven,  PlayerAbility playerAbility)
    {
        UIAdd.Get<UIOutput>(UIType.UIOutput).SendInven(inven, playerAbility.m_nIndex);

        m_UIOutput = UIAdd.Get<UIOutput>(UIType.UIOutput);
        m_UIOutput.Init(out m_GetQuset, m_MakeBoss, playerAbility);
    }
}
