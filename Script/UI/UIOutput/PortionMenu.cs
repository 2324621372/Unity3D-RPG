using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface SandportionMenu
{
    void Remove(GameObject obj);
}

public class PortionMenu : MonoBehaviour, SandportionMenu
{
    [SerializeField]
    private BuyMenu m_BuyMenu;
    public Sprite[] m_PortionSprite;

    public Image[] m_PortionImg;

    public Text[] m_NameTxt;
    public Text[] m_ContentTxt;
    public Text[] m_MoneyTxt;

    private Dictionary<Sprite, string> m_DicName = new Dictionary<Sprite, string>();
    private Dictionary<Sprite, string> m_DicContent = new Dictionary<Sprite, string>();
    private Dictionary<Sprite, int> m_DicMoney = new Dictionary<Sprite, int>();
    private Dictionary<Sprite, GameObject> m_DicObj = new Dictionary<Sprite, GameObject>();

    private int m_nAllSolt;
    private int m_nSoltCount = 1;
    private int m_nCount;
    private int m_nPortionCount;
    private int m_nPrice;

    public GameObject m_AskBuy;
    public GameObject m_InventoryShowScreen;
    public GameObject m_AskSell;
    private GameObject m_BtnObj;

    private Inventory m_Inventory;

    private List<GameObject> m_LisBtnObj = new List<GameObject>();

    public Text m_InventoryShowMoneyTxt;
    public Text m_AskCountTxt;
    public Text m_AskPriceTxt;
    public Text m_AskSellCountTxt;
    public Text m_AskSellPriceTxt;

    private Dictionary<GameObject, int> m_DicPortionCount = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, int> m_DicPortion = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, Text> m_DicPortionCountText = new Dictionary<GameObject, Text>();
    private Dictionary<GameObject, Text> m_DicPortionCountText_2 = new Dictionary<GameObject, Text>();

    private PortionEquipment m_PortionEquiment;
    private InstallEquipment m_InstallEquipment;

    public void GetInven(Inventory inven)
    {
        m_Inventory = inven;
    }


    public void Init(List<GameObject> LisPortion)
    {
        for (int i = 0; i < m_PortionSprite.Length; ++i)
        {
            m_DicName.Add(m_PortionSprite[i], DataMng.Get(TableType.PortionTable).ToS(i + 1, "NAME"));
            m_DicContent.Add(m_PortionSprite[i], DataMng.Get(TableType.PortionTable).ToS(i + 1, "Content"));
            m_DicMoney.Add(m_PortionSprite[i], DataMng.Get(TableType.PortionTable).ToI(i + 1, "Money"));

            m_DicObj.Add(m_PortionSprite[i], LisPortion[i]);

        }
        for (int i = 0; i < m_PortionImg.Length; ++i)
        {
            m_PortionImg[i].sprite = m_PortionSprite[i];
            m_NameTxt[i].text = m_DicName[m_PortionSprite[i]];
            m_ContentTxt[i].text = m_DicContent[m_PortionSprite[i]];
            m_MoneyTxt[i].text = m_DicMoney[m_PortionSprite[i]].ToString();
        }
        m_nAllSolt = (int)(m_PortionSprite.Length / 4.0f);

        m_InstallEquipment = FindObjectOfType<InstallEquipment>();
        gameObject.SetActive(false);
    }

    public void OtherMoney()
    {
        m_InventoryShowMoneyTxt.text = m_Inventory.m_Money.ToString();
    }

    public void NextPage()
    {
        if (m_nAllSolt > m_nSoltCount)
        {
            ++m_nSoltCount;
            m_nCount += 4;
            for (int i = m_nCount; i < m_PortionImg.Length + m_nCount; ++i)
            {
                m_PortionImg[i - m_nCount].sprite = m_PortionSprite[i];
                m_NameTxt[i - m_nCount].text = m_DicName[m_PortionSprite[i]];
                m_ContentTxt[i - m_nCount].text = m_DicContent[m_PortionSprite[i]];
                m_MoneyTxt[i - m_nCount].text = m_DicMoney[m_PortionSprite[i]].ToString();
            }
            AudioManager.Instance.PlayEffect(0);
        }
    }

    public void BackPage()
    {
        if (1 < m_nSoltCount)
        {
            --m_nSoltCount;
            m_nCount -= 4;
            for (int i = m_nCount; i < m_PortionImg.Length - m_nCount; ++i)
            {
                m_PortionImg[i].sprite = m_PortionSprite[i];
                m_NameTxt[i].text = m_DicName[m_PortionSprite[i]];
                m_ContentTxt[i].text = m_DicContent[m_PortionSprite[i]];
                m_MoneyTxt[i].text = m_DicMoney[m_PortionSprite[i]].ToString();
            }
            AudioManager.Instance.PlayEffect(0);
        }
    }

    public void AskBuy(Image img)
    {
        if (m_DicMoney[img.sprite] <= m_Inventory.m_Money)
        {
            m_AskCountTxt.text = "1";
            m_AskBuy.SetActive(true);
            m_BtnObj = m_DicObj[img.sprite];
            m_nPrice = m_DicMoney[img.sprite];

            if (!m_DicPortionCount.ContainsKey(m_BtnObj))
            {
                m_DicPortionCount.Add(m_BtnObj, 1);
            }
            m_AskPriceTxt.text = m_nPrice.ToString();
            AudioManager.Instance.PlayEffect(0);
        }
    }

    public void AskSell(Image img, PortionEquipment portionEquipment)
    {
        m_AskSellCountTxt.text = "1";
        m_AskSell.SetActive(true);
        m_BtnObj = m_DicObj[img.sprite];

        m_nPrice = m_DicMoney[img.sprite];
        m_AskSellPriceTxt.text = m_nPrice.ToString();
        m_PortionEquiment = portionEquipment;
        AudioManager.Instance.PlayEffect(0);
    }

    public void SandMake()
    {
        if (m_Inventory.m_Money >= m_DicPortionCount[m_BtnObj] * m_nPrice)
        {
            if (!m_DicPortion.ContainsKey(m_BtnObj))
            {
                GameObject btn = Instantiate(m_BtnObj);
                GameObject btn_2 = Instantiate(m_BtnObj);

                Text txt = btn.transform.GetChild(2).gameObject.GetComponent<Text>();
                Text txt_2 = btn_2.transform.GetChild(2).gameObject.GetComponent<Text>();

                PortionEquipment portionEquipment = btn.GetComponent<PortionEquipment>();
                portionEquipment.Init(m_Inventory, btn_2);
                portionEquipment.CopyObj(this, m_BtnObj, btn_2);

                //처음것을 넣어야 밑제거에서 같이 없앤다.
                btn_2.GetComponent<Button>().onClick.AddListener(delegate { AskSell(btn_2.GetComponent<Image>(), portionEquipment); });

                btn.transform.SetParent(m_Inventory.m_PortionScreen.transform);
                btn_2.transform.SetParent(m_InventoryShowScreen.transform);

                btn.transform.localScale = new Vector3(0.14f, 0.15f, 0.14f);
                btn.GetComponent<RectTransform>().pivot = new Vector2(0, 0.79f);

                btn_2.transform.localScale = new Vector3(0.14f, 0.20f, 0.14f);
                btn_2.transform.GetChild(1).localScale = new Vector3(1.1f, 0.8f, 0.14f);
                btn_2.transform.GetChild(0).localScale = new Vector3(1.1f, 0.8f, 0.14f);
                btn_2.GetComponent<RectTransform>().pivot = new Vector2(0, 1.33f);

                txt.text = m_DicPortionCount[m_BtnObj].ToString();
                txt_2.text = m_DicPortionCount[m_BtnObj].ToString();

                m_DicPortion.Add(m_BtnObj, m_DicPortionCount[m_BtnObj]);

                m_DicPortionCountText.Add(m_BtnObj, txt);
                m_DicPortionCountText_2.Add(m_BtnObj, txt_2);

            }
            else
            {
                m_DicPortion[m_BtnObj] += m_DicPortionCount[m_BtnObj];
                m_DicPortionCountText[m_BtnObj].text = m_DicPortion[m_BtnObj].ToString();
                m_DicPortionCountText_2[m_BtnObj].text = m_DicPortion[m_BtnObj].ToString();
            }

            m_Inventory.SetMoney(m_Inventory.m_Money - (m_DicPortionCount[m_BtnObj] * m_nPrice));
            m_InventoryShowMoneyTxt.text = m_Inventory.m_Money.ToString();
            m_Inventory.SetMoney();
            m_BuyMenu.OtherMoney();
            m_AskBuy.SetActive(false);
        }
        AudioManager.Instance.PlayEffect(0);
    }

    public void AskBuyCancle()
    {
        m_DicPortionCount[m_BtnObj] = 1;

        m_AskCountTxt.text = m_DicPortionCount[m_BtnObj].ToString();

        m_AskBuy.SetActive(false);
        AudioManager.Instance.PlayEffect(0);
    }

    public void AskSellCancle()
    {
        m_nSellCount = 1;

        m_AskSellCountTxt.text = m_nSellCount.ToString();

        m_AskSell.SetActive(false);
        AudioManager.Instance.PlayEffect(0);
    }

    public void BuyChangeCount(int count)
    {
        m_DicPortionCount[m_BtnObj] += count;

        if (m_DicPortionCount[m_BtnObj] < 1)
        {
            m_DicPortionCount[m_BtnObj] = 1;
        }
        else if (m_DicPortionCount[m_BtnObj] > 99)
        {
            m_DicPortionCount[m_BtnObj] = 99;
        }

        m_AskCountTxt.text = m_DicPortionCount[m_BtnObj].ToString();
        m_AskPriceTxt.text = (m_DicPortionCount[m_BtnObj] * m_nPrice).ToString();
    }

    private int m_nSellCount = 1;

    public void SellChangeCount(int count)
    {
        m_nSellCount += count;

        if (m_nSellCount <= 1)
        {
            m_nSellCount = 1;
        }
        else if (m_nSellCount >= m_DicPortionCount[m_BtnObj])
        {
            m_nSellCount = m_DicPortionCount[m_BtnObj];
        }

        m_AskSellCountTxt.text = m_nSellCount.ToString();
        m_AskSellPriceTxt.text = (m_nSellCount * m_nPrice).ToString();
    }

    public void SellYes()
    {
        m_DicPortionCount[m_BtnObj] -= m_nSellCount;

        m_Inventory.SetMoney(m_Inventory.m_Money + (m_nSellCount * m_nPrice));
        m_InventoryShowMoneyTxt.text = m_Inventory.m_Money.ToString();

        m_DicPortionCountText[m_BtnObj].text = m_DicPortionCount[m_BtnObj].ToString();
        m_DicPortionCountText_2[m_BtnObj].text = m_DicPortionCount[m_BtnObj].ToString();

        int _nIndex = m_BtnObj.GetComponent<PortionEquipment>().m_nIndex;
        if (m_InstallEquipment.m_DicPortionObj.ContainsKey(_nIndex))
        {
            Text txt = m_InstallEquipment.m_DicPortionObj[_nIndex].transform.GetChild(2).gameObject.GetComponent<Text>();
            txt.text = m_DicPortionCount[m_BtnObj].ToString();
        }

        if (m_DicPortionCount[m_BtnObj] < 1)
        {
            m_PortionEquiment.ZeroCount();
            if (m_InstallEquipment.m_DicPortionObj.ContainsKey(_nIndex))
            {
                Destroy(m_InstallEquipment.m_DicPortionObj[_nIndex]);
                m_InstallEquipment.m_DicPortionObj.Remove(_nIndex);

            }
            Remove(m_BtnObj);
        }

        m_Inventory.SetMoney();
        m_AskSell.SetActive(false);
        m_nSellCount = 1;
        AudioManager.Instance.PlayEffect(0);
    }

    public void Remove(GameObject obj)
    {
        m_DicPortionCount.Remove(obj);
        m_DicPortion.Remove(obj);

        m_DicPortionCountText.Remove(obj);
        m_DicPortionCountText_2.Remove(obj);
    }
}
