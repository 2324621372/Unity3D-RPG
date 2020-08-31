using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskClear : ASK
{
    public AskScreen m_AskScreenObj;
    private GameObject m_Portion;
    private int m_nTable;
    private int m_nIndex;

    public override void YesBtn()
    {
        m_InstallScreen.m_LisEquipmentObj[m_nIndex].transform.GetChild(1).position = m_LisScreenObj[m_nTable].transform.position;
        m_InstallScreen.m_LisEquipmentObj[m_nIndex].transform.GetChild(1).SetParent(m_LisScreenObj[m_nTable].transform);

        m_InstallScreen.m_LisEquipmentObj[m_nIndex].transform.GetChild(0).gameObject.SetActive(true);

        m_AskScreenObj.m_DicBeforeIndex.Remove(m_nIndex);

        //포션
        if (m_nTable == 3)
        {
            Destroy(m_InstallScreen.m_InstallEquipment.m_DicPortion[m_nIndex]);
            m_InstallScreen.m_InstallEquipment.m_DicPortion.Remove(m_nIndex);    
        }
        //악세서리
        else if(m_nTable==2)
        {
            m_InstallScreen.SetInstall(0, m_nIndex);
        }
        //무기 및 방어구
        else
        {
            m_InstallScreen.SetInstall(1, m_nIndex);
        }

        gameObject.SetActive(false);
        m_OtherObj.SetActive(true);
        m_AskScreenObj.m_BeforeOtherObj[m_nTable] = null;

        AudioManager.Instance.PlayEffect(0);
    }

    public override void NoBtn()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayEffect(0);
    }

    public void setIndex(int index, int table,GameObject other)
    {
        m_nIndex = index;
        m_nTable = table;
        m_OtherObj = other;
    }

    public void setPortion(GameObject obj)
    {
        m_Portion = obj;
    }
}
