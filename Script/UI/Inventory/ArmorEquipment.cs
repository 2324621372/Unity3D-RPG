using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorEquipment : Equipment
{
    [SerializeField]
    protected int m_nInstall;
    private string m_sDefense;

    protected override void InitChild()
    {
        m_sName = DataMng.Get(TableType.ArmorTable).ToS(m_nIndex, "NAME");
        m_sDefense = DataMng.Get(TableType.ArmorTable).ToS(m_nIndex, "DEFENSE");
        m_nMoney = DataMng.Get(TableType.ArmorTable).ToI(m_nIndex, "Money");
    }

    public override void AskInstall()
    {
        if (transform.parent.name.Equals("DefenseScreen"))
        {
            m_AskScreen.gameObject.SetActive(true);
            m_AskScreen.SetEquipment(m_Image, m_nIndex, gameObject, m_nInstall,m_BuyObj);
        }
        else
        {
            m_AskClear.gameObject.SetActive(true);
            m_AskClear.setIndex(m_nInstall,1, m_BuyObj);
        }
    }
}
