using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//장비를 만들 생각이면 HP나 MP까지 만들어서 장비의 능력의 폭을 올리는건 어떨지 의견
public class Inventory : MonoBehaviour
{
    [HideInInspector]
    public PlayerAbility m_Ability;

    [SerializeField]
    private Text m_txtATC;
    [SerializeField]
    private Text m_txtnDEF;
    [SerializeField]
    private Text m_txtnCRI;

    [SerializeField]
    private Image m_AttackBagImg;
    [SerializeField]
    private Image m_DefenseBagImg;
    [SerializeField]
    private Image m_AccessoryBagImg;
    [SerializeField]
    private Image m_PortionBagImg;

    public GameObject m_AttackScreen;
    public GameObject m_DefenseScreen;
    public GameObject m_AccessoryScreen;
    public GameObject m_PortionScreen;

    public AskScreen m_AskScreen;
    public AskClear m_AskClear;

    public Text m_MoneyTxt;

    public GameObject[] m_PlayerJob;

    private int m_nMoney;
    public int m_Money { get { return m_nMoney; } }
    public void SetMoney(int money) { m_nMoney = money; }

    public void Init(int index)
    {
        m_Ability = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbility>();

        m_MoneyTxt.text = "100000";
        SetMoney(int.Parse(m_MoneyTxt.text));

        m_PlayerJob[index-1].SetActive(true);
    }

    public void SetMoney()
    {
        m_MoneyTxt.text = m_Money.ToString();
    }

    void Update()
    {
        m_txtATC.text = m_Ability.m_Attack.ToString();
        m_txtnDEF.text = m_Ability.m_Defense.ToString();
        m_txtnCRI.text = m_Ability.m_Critical.ToString()+" %";
    }


    public void CloseInventory()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.PlayEffect(0);
    }

    public void AttackBag()
    {
        m_AttackScreen.SetActive(true);
        m_DefenseScreen.SetActive(false);
        m_AccessoryScreen.SetActive(false);
        m_PortionScreen.SetActive(false);

        m_AttackBagImg.color = new Color(m_AttackBagImg.color.r, m_AttackBagImg.color.g, m_AttackBagImg.color.b, 1.0f);
        m_DefenseBagImg.color = new Color(m_DefenseBagImg.color.r, m_DefenseBagImg.color.g, m_DefenseBagImg.color.b, 0.5f);
        m_AccessoryBagImg.color = new Color(m_AccessoryBagImg.color.r, m_AccessoryBagImg.color.g, m_AccessoryBagImg.color.b, 0.5f);
        m_PortionBagImg.color = new Color(m_AccessoryBagImg.color.r, m_AccessoryBagImg.color.g, m_AccessoryBagImg.color.b, 0.5f);

    }
    public void DefenseBag()
    {
        m_AttackScreen.SetActive(false);
        m_DefenseScreen.SetActive(true);
        m_AccessoryScreen.SetActive(false);
        m_PortionScreen.SetActive(false);

        m_AttackBagImg.color = new Color(m_AttackBagImg.color.r, m_AttackBagImg.color.g, m_AttackBagImg.color.b, 0.5f);
        m_DefenseBagImg.color = new Color(m_DefenseBagImg.color.r, m_DefenseBagImg.color.g, m_DefenseBagImg.color.b, 1.0f);
        m_AccessoryBagImg.color = new Color(m_AccessoryBagImg.color.r, m_AccessoryBagImg.color.g, m_AccessoryBagImg.color.b, 0.5f);
        m_PortionBagImg.color = new Color(m_AccessoryBagImg.color.r, m_AccessoryBagImg.color.g, m_AccessoryBagImg.color.b, 0.5f);
    }
    public void AccessoryBag()
    {
        m_AttackScreen.SetActive(false);
        m_DefenseScreen.SetActive(false);
        m_AccessoryScreen.SetActive(true);
        m_PortionScreen.SetActive(false);

        m_AttackBagImg.color = new Color(m_AttackBagImg.color.r, m_AttackBagImg.color.g, m_AttackBagImg.color.b, 0.5f);
        m_DefenseBagImg.color = new Color(m_DefenseBagImg.color.r, m_DefenseBagImg.color.g, m_DefenseBagImg.color.b, 0.5f);
        m_AccessoryBagImg.color = new Color(m_AccessoryBagImg.color.r, m_AccessoryBagImg.color.g, m_AccessoryBagImg.color.b, 1.0f);
        m_PortionBagImg.color = new Color(m_AccessoryBagImg.color.r, m_AccessoryBagImg.color.g, m_AccessoryBagImg.color.b, 0.5f);
    }
    public void PortionBag()
    {
        m_AttackScreen.SetActive(false);
        m_DefenseScreen.SetActive(false);
        m_AccessoryScreen.SetActive(false);
        m_PortionScreen.SetActive(true);

        m_AttackBagImg.color = new Color(m_AttackBagImg.color.r, m_AttackBagImg.color.g, m_AttackBagImg.color.b, 0.5f);
        m_DefenseBagImg.color = new Color(m_DefenseBagImg.color.r, m_DefenseBagImg.color.g, m_DefenseBagImg.color.b, 0.5f);
        m_AccessoryBagImg.color = new Color(m_AccessoryBagImg.color.r, m_AccessoryBagImg.color.g, m_AccessoryBagImg.color.b, 0.5f);
        m_PortionBagImg.color = new Color(m_AccessoryBagImg.color.r, m_AccessoryBagImg.color.g, m_AccessoryBagImg.color.b, 1.0f);
    }
}
