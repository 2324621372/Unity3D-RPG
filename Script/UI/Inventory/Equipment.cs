using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [HideInInspector]
    public AskScreen m_AskScreen;
    [HideInInspector]
    public int m_nMoney;

    public int m_nIndex;

    protected AskClear m_AskClear;
    protected Inventory m_Inventory;
    protected Image m_Image;
    protected Button m_button;
    protected string m_sName;
    protected GameObject m_BuyObj;

    public void Init(Inventory inven,GameObject btn)
    {
        m_Inventory = inven;

        m_AskScreen = m_Inventory.m_AskScreen;
        m_AskClear = m_Inventory.m_AskClear;

        m_Image = GetComponent<Image>();
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(delegate { AskInstall(); });

        m_BuyObj = btn;
        InitChild();
    }

    protected virtual void InitChild() { }
    //장착과 해제를 하는 함수
    public virtual void AskInstall() { }
}
