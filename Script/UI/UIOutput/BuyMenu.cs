using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ReturnMoney
{
    void InputMoney(int money);
    void SetTrueAskSell(ReturnItem returnItem);
}
public class BuyMenu : MonoBehaviour, ReturnMoney
{
    [SerializeField]
    private PortionMenu m_PortionMenu;
    public List<Sprite> m_LisTowHand = new List<Sprite>();
    public List<Sprite> m_LisMagician = new List<Sprite>();
    public List<Sprite> m_LisArmor = new List<Sprite>();

    private Dictionary<Sprite, string> m_DicName = new Dictionary<Sprite, string>();
    private Dictionary<Sprite, string> m_DicAttack = new Dictionary<Sprite, string>();
    private Dictionary<Sprite, string> m_DicDefense = new Dictionary<Sprite, string>();
    private Dictionary<Sprite, string> m_DicMoney = new Dictionary<Sprite, string>();

    private Dictionary<Sprite, GameObject> m_DicBtn = new Dictionary<Sprite, GameObject>();
    private Dictionary<Sprite, int> m_DicBtnMoney = new Dictionary<Sprite, int>();
    private Dictionary<GameObject, int> m_DicBtnType = new Dictionary<GameObject, int>();

    public List<Image> m_LisBuybarImg = new List<Image>();

    public List<Text> m_LisBuyAttackTxt = new List<Text>();
    public List<Text> m_LisBuyDefenseTxt = new List<Text>();

    public List<Text> m_LisBuyNameTxt = new List<Text>();
    public List<Text> m_LisBuyMoneyTxt = new List<Text>();

    private List<Sprite> m_LisWeapon = new List<Sprite>();

    private PlayerAbility m_PlayerAbility;
    private Inventory m_Inventory;
    public InventoryShow m_InventoryShow;

    public GameObject m_AskBuy;
    private GameObject m_BtnObj;

    private int m_nIndex;

    public List<int> m_LisBuyMoney = new List<int>();

    private List<GameObject> m_LisBtnObj = new List<GameObject>();

    public Image m_WeapenImg;
    public Image m_DefenseImg;
    public GameObject m_WeaponScreenObj;
    public GameObject m_DefenseScreenObj;

    private Color m_ColorAlpa = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color m_OriginColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    public BuyMenu_DefenseScreen m_BuyMenu_DefenseScreen;
    public GameObject m_AskSell;
    public Button m_AskSellYes;

    private int m_nPrice;

    public void Init(List<GameObject> Lisweapon, List<GameObject> Lisdefense, PlayerAbility playerAbility)
    {
        int _nNumber = 2;
        m_PlayerAbility = playerAbility;

        switch (m_PlayerAbility.m_nIndex)
        {
            case 1:
                m_LisWeapon = m_LisTowHand;
                _nNumber = 2;
                break;
            case 2:
                _nNumber = 6;
                m_LisWeapon = m_LisMagician;
                break;
        }

        for (int i = 0; i < m_LisWeapon.Count; ++i)
        {
            m_DicName.Add(m_LisWeapon[i], DataMng.Get(TableType.WeaponTable).ToS(i + _nNumber, "K_NAME"));

            m_DicAttack.Add(m_LisWeapon[i], DataMng.Get(TableType.WeaponTable).ToS(i + _nNumber, "ATTACK"));

            m_DicMoney.Add(m_LisWeapon[i], DataMng.Get(TableType.WeaponTable).ToS(i + _nNumber, "Money"));
            m_LisBuyMoney.Add(DataMng.Get(TableType.WeaponTable).ToI(i + _nNumber, "Money"));

            m_DicBtn.Add(m_LisWeapon[i], Lisweapon[i]);
            m_DicBtnMoney.Add(m_LisWeapon[i], DataMng.Get(TableType.WeaponTable).ToI(i + _nNumber, "Money"));
            m_DicBtnType.Add(Lisweapon[i], 1);
        }

        for (int i = 0; i < m_LisArmor.Count; ++i)
        {
            m_DicName.Add(m_LisArmor[i], DataMng.Get(TableType.ArmorTable).ToS(i + 2, "K_NAME"));

            m_DicDefense.Add(m_LisArmor[i], DataMng.Get(TableType.ArmorTable).ToS(i + 2, "DEFENSE"));

            m_DicMoney.Add(m_LisArmor[i], DataMng.Get(TableType.ArmorTable).ToS(i + 2, "Money"));
            m_LisBuyMoney.Add(DataMng.Get(TableType.ArmorTable).ToI(i + 2, "Money"));

            m_DicBtn.Add(m_LisArmor[i], Lisdefense[i]);
            m_DicBtnMoney.Add(m_LisArmor[i], DataMng.Get(TableType.ArmorTable).ToI(i + 2, "Money"));
            m_DicBtnType.Add(Lisdefense[i], 2);
        }

        for (int i = 0; i < m_LisBuyAttackTxt.Count; ++i)
        {
            m_LisBuybarImg[i].sprite = m_LisWeapon[i];
            m_LisBuyNameTxt[i].text = m_DicName[m_LisWeapon[i]];
            m_LisBuyAttackTxt[i].text = m_DicAttack[m_LisWeapon[i]];
            m_LisBuyMoneyTxt[i].text = m_DicMoney[m_LisWeapon[i]];
        }

        //i+ m_LisBuyAttackTxt.Count한 이유는 Attackscreen부분에서 먼저 출력하고 다음에 방어를 했기때문에 i+ m_LisBuyAttackTxt.Count를 함
        for (int i = m_LisBuyAttackTxt.Count; i < m_LisBuybarImg.Count; ++i)
        {
            m_LisBuybarImg[i].sprite = m_LisArmor[i - m_LisBuyAttackTxt.Count];
            m_LisBuyNameTxt[i].text = m_DicName[m_LisArmor[i - m_LisBuyAttackTxt.Count]];
            //m_LisBuyDefenseTxt은 0부터 시작함
            m_LisBuyDefenseTxt[i - m_LisBuyAttackTxt.Count].text = m_DicDefense[m_LisArmor[i - m_LisBuyAttackTxt.Count]];
            m_LisBuyMoneyTxt[i].text = m_DicMoney[m_LisArmor[i - m_LisBuyAttackTxt.Count]];
        }
        m_WeapenImg.color = m_OriginColor;
        m_DefenseImg.color = m_ColorAlpa;

        m_BuyMenu_DefenseScreen.Init(Lisdefense, this);
    }

    public void OtherMoney()
    {
        m_InventoryShow.m_MoneyTxt.text = m_Inventory.m_Money.ToString();
    }

    public void NextDefenseItem(int nextItem)
    {
        for (int i = m_LisBuyAttackTxt.Count; i < m_LisBuybarImg.Count; ++i)
        {
            if (m_LisArmor.Count <= i - m_LisBuyAttackTxt.Count + nextItem)
            {
                m_LisBuybarImg[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                m_LisBuybarImg[i].transform.parent.gameObject.SetActive(true);
                m_LisBuybarImg[i].sprite = m_LisArmor[i - m_LisBuyAttackTxt.Count + nextItem];
                m_LisBuyNameTxt[i].text = m_DicName[m_LisArmor[i - m_LisBuyAttackTxt.Count + nextItem]];
                //m_LisBuyDefenseTxt은 0부터 시작함
                m_LisBuyDefenseTxt[i - m_LisBuyAttackTxt.Count].text = m_DicDefense[m_LisArmor[i - m_LisBuyAttackTxt.Count + nextItem]];
                m_LisBuyMoneyTxt[i].text = m_DicMoney[m_LisArmor[i - m_LisBuyAttackTxt.Count + nextItem]];
            }
        }
    }

    public void SetAttackBtn()
    {
        m_WeapenImg.color = m_OriginColor;
        m_DefenseImg.color = m_ColorAlpa;

        m_WeaponScreenObj.SetActive(true);
        m_DefenseScreenObj.SetActive(false);
    }

    public void SetDefenseBtn()
    {
        m_WeapenImg.color = m_ColorAlpa;
        m_DefenseImg.color = m_OriginColor;

        m_WeaponScreenObj.SetActive(false);
        m_DefenseScreenObj.SetActive(true);
    }

    public void GetInven(Inventory inven)
    {
        m_Inventory = inven;
        m_InventoryShow.Init(inven);
    }

    public void AskBuy(Image img)
    {
        if (m_DicBtnMoney[img.sprite] <= m_Inventory.m_Money)
        {
            m_AskBuy.SetActive(true);
            m_nPrice = m_DicBtnMoney[img.sprite];
            m_BtnObj = m_DicBtn[img.sprite];
        }
    }

    public void SandMake()
    {
        GameObject btn = Instantiate(m_BtnObj);
        GameObject btn_2 = Instantiate(m_BtnObj);

        if (m_DicBtnType[m_BtnObj] == 1)
        {
            WeaponEquipment weanponEquipment = btn.GetComponent<WeaponEquipment>();
            //btn_2를 주는 이유는 장착시 스크린에서도 없애기 위한 부분
            weanponEquipment.Init(m_Inventory, btn_2);

            btn_2.AddComponent<ReturnItem>().Init(this, weanponEquipment.m_nMoney, btn);

            btn.transform.SetParent(m_Inventory.m_AttackScreen.transform);
            btn_2.transform.SetParent(m_InventoryShow.m_AttackScreenObj.transform);

            m_InventoryShow.AttackBtn();
        }
        else if (m_DicBtnType[m_BtnObj] == 2)
        {
            ArmorEquipment armorEquipment = btn.GetComponent<ArmorEquipment>();
            armorEquipment.Init(m_Inventory, btn_2);

            btn_2.AddComponent<ReturnItem>().Init(this, armorEquipment.m_nMoney, btn);

            btn.transform.SetParent(m_Inventory.m_DefenseScreen.transform);
            btn_2.transform.SetParent(m_InventoryShow.m_DefenseScreenObj.transform);

            m_InventoryShow.DefenseBtn();
        }

        btn.transform.localScale = new Vector3(0.14f, 0.15f, 0.14f);
        btn.GetComponent<RectTransform>().pivot = new Vector2(0, 0.79f);

        btn_2.transform.localScale = new Vector3(0.14f, 0.20f, 0.14f);
        btn_2.transform.GetChild(0).localScale = new Vector3(1.1f, 0.8f, 0.14f);
        btn_2.GetComponent<RectTransform>().pivot = new Vector2(0, 1.35f);

        m_Inventory.SetMoney(m_Inventory.m_Money - m_nPrice);
        m_InventoryShow.m_MoneyTxt.text = m_Inventory.m_Money.ToString();
        m_Inventory.SetMoney();

        m_PortionMenu.OtherMoney();
        m_AskBuy.SetActive(false);

        AudioManager.Instance.PlayEffect(0);
    }

    public void Cancle(GameObject obj)
    {
        obj.SetActive(false);
        AudioManager.Instance.PlayEffect(0);
    }

    public void SetTrueAskSell(ReturnItem returnItem)
    {
        m_AskSell.SetActive(true);
        returnItem.m_AskSell = m_AskSell;

        m_AskSellYes.onClick.AddListener(delegate { returnItem.RemoveSell(m_AskSellYes); });
    }

    public void InputMoney(int money)
    {
        m_Inventory.SetMoney(m_Inventory.m_Money+money);
        m_InventoryShow.m_MoneyTxt.text = m_Inventory.m_Money.ToString();
        m_Inventory.SetMoney();
    }
}

