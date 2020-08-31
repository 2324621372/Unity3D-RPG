using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortionEquipment : Equipment
{
    private string m_sContent;
    private static int m_nInstall = 5;

    private int m_nFixInstall;

    private GameObject m_SubPortion;
    private GameObject m_ReallyPortion;

    private SandportionMenu m_PortionMenu;

    protected override void InitChild()
    {
        m_sName = DataMng.Get(TableType.PortionTable).ToS(m_nIndex, "NAME");
        m_sContent = DataMng.Get(TableType.PortionTable).ToS(m_nIndex, "Content");
        m_nMoney = DataMng.Get(TableType.PortionTable).ToI(m_nIndex, "Money");
    }

    public void CopyObj(SandportionMenu portionMenu, GameObject reallyObj, GameObject subObj)
    {
        m_PortionMenu = portionMenu;
        m_ReallyPortion = reallyObj;
        m_SubPortion = subObj;
    }

    public override void AskInstall()
    {
        if (transform.parent.name.Equals("PortionScreen"))
        {
            m_nFixInstall = m_nInstall;
            m_AskScreen.gameObject.SetActive(true);
            m_AskScreen.SetEquipment(m_Image, m_nIndex, gameObject, m_nInstall, m_BuyObj);

            ++m_nInstall;

            if (m_nInstall > 6)
            {
                m_nInstall = 5;
            }
        }
        else
        {
            m_AskClear.gameObject.SetActive(true);
            m_AskClear.setIndex(m_nFixInstall, 3, m_BuyObj);
            m_AskClear.setPortion(gameObject);
            m_nInstall = m_nFixInstall;
        }
    }

    public void ZeroCount()
    {
        m_AskScreen.m_InstallScreen.m_LisEquipmentObj[m_nFixInstall].transform.GetChild(0).gameObject.SetActive(true);
        m_AskScreen.m_DicBeforeIndex.Remove(m_nFixInstall);
        Destroy(gameObject);
        Destroy(m_SubPortion);
        m_PortionMenu.Remove(m_ReallyPortion);
    }
}
