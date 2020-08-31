using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryShow : MonoBehaviour
{
    public Image m_AttackBtnImg;
    public Image m_DefenseBtnImg;
    public Image m_AccessoryBtnImg;

    public GameObject m_AttackScreenObj;
    public GameObject m_DefenseScreenObj;
    public GameObject m_AccessoryScreenObj;

    private Color m_ColorAlpa = new Color(1, 1, 1, 0.5f);
    private Color m_OriginColor = new Color(1, 1, 1, 1.0f);

    private Inventory m_Inventory;

    private List<GameObject> m_LisWeaponObj = new List<GameObject>();
    private List<GameObject> m_LisDefenseObj = new List<GameObject>();

    public Text m_MoneyTxt;

    public void Init(Inventory inven)
    {
        m_AttackBtnImg.color = m_ColorAlpa;
        m_DefenseBtnImg.color = m_ColorAlpa;
        m_AccessoryBtnImg.color = m_ColorAlpa;

        m_Inventory = inven;
        m_MoneyTxt.text = m_Inventory.m_MoneyTxt.text;
        AttackBtn();
    }

    public void AttackBtn()
    {    
        if (m_AttackBtnImg.color.a != 1.0f)
        {
            m_AttackBtnImg.color = m_OriginColor;
            m_DefenseBtnImg.color = m_ColorAlpa;
            m_AccessoryBtnImg.color = m_ColorAlpa;

            m_AttackScreenObj.SetActive(true);
            m_DefenseScreenObj.SetActive(false);
            m_AccessoryScreenObj.SetActive(false);
        }
    }

    public void DefenseBtn()
    {
        if (m_DefenseBtnImg.color.a != 1.0f)
        {
            m_AttackBtnImg.color = m_ColorAlpa;
            m_DefenseBtnImg.color = m_OriginColor;
            m_AccessoryBtnImg.color = m_ColorAlpa;


            m_AttackScreenObj.SetActive(false);
            m_DefenseScreenObj.SetActive(true);
            m_AccessoryScreenObj.SetActive(false);          
        }
    }

    public void AccessoryBtn()
    {
        if (m_AccessoryBtnImg.color.a != 1.0f)
        {
            m_AttackBtnImg.color = m_ColorAlpa;
            m_DefenseBtnImg.color = m_ColorAlpa;
            m_AccessoryBtnImg.color = m_OriginColor;

            m_AttackScreenObj.SetActive(false);
            m_DefenseScreenObj.SetActive(false);
            m_AccessoryScreenObj.SetActive(true);
        }
    }
}
