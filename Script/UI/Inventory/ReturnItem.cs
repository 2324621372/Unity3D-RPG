using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnItem : MonoBehaviour
{
    private int m_nMoney;

    private ReturnMoney m_ReturnMoney;

    private GameObject m_OtherObj;
    public GameObject m_AskSell;

    public void Init(ReturnMoney returnMoney, int money, GameObject otherObj)
    {
        m_ReturnMoney = returnMoney;
        m_nMoney = money;

        Button _button = GetComponent<Button>();
        _button.onClick.AddListener(delegate { Addbtn(); });

        m_OtherObj = otherObj;
    }

    public void Addbtn()
    {
        m_ReturnMoney.SetTrueAskSell(this);
    }

    public void RemoveSell(Button button)
    {
        button.onClick.RemoveAllListeners();
        m_AskSell.SetActive(false);
        m_ReturnMoney.InputMoney(m_nMoney);    
        Destroy(m_OtherObj);
        Destroy(gameObject);
    }
}
