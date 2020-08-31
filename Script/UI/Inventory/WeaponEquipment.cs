using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquipment : Equipment
{
    private string m_sAttack;
    private const int m_nInstall=3;

    protected override void InitChild()
    {
        m_sName = DataMng.Get(TableType.WeaponTable).ToS(m_nIndex, "NAME");
        m_sAttack = DataMng.Get(TableType.WeaponTable).ToS(m_nIndex, "ATTACK");
        m_nMoney = DataMng.Get(TableType.WeaponTable).ToI(m_nIndex, "Money");
    }

    public override void AskInstall()
    {
        if (transform.parent.name.Equals("AttackScreen"))
        {
            m_AskScreen.gameObject.SetActive(true);
            m_AskScreen.SetEquipment(m_Image, m_nIndex, gameObject, m_nInstall, m_BuyObj);
        }
        else
        {        
            m_AskClear.gameObject.SetActive(true);
            m_AskClear.setIndex(m_nInstall,0, m_BuyObj);
        }
    }
}
