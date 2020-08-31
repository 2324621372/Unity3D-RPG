using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryEquipment : Equipment
{
    private string m_sAccessory;
    private int m_nInstall=4;

    protected override void InitChild()
    {
        m_sAccessory = DataMng.Get(TableType.AccessoryTable).ToS(m_nIndex, "NAME");
    }

    public override void AskInstall()
    {

        if (transform.parent.name.Equals("AccessoryScreen"))
        {
            m_AskScreen.gameObject.SetActive(true);
            m_AskScreen.SetEquipment(m_Image, m_nIndex, gameObject, m_nInstall, m_BuyObj);
        }
        else
        {
            m_AskClear.gameObject.SetActive(true);
            m_AskClear.setIndex(m_nInstall, 2, m_BuyObj);
        }
    }
}
