using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBar : MonoBehaviour, GetQuset
{
    public Text QuestTxt;
    public Text GoalTxt;
    public Text CountTxt;

    private Color m_Color;

    [HideInInspector]
    public int m_nCount = 0;
    [HideInInspector]
    public int m_nGoal;

    private GameObject m_CancleQuestObj;

    private QuestMenu m_QuestMenu;

    //세이브한 퀘스트를 없애기위해 세이브한 키를 없앤다.
    private int m_nkey;

    Dictionary<int, GameObject> m_DicSaveQuest = new Dictionary<int, GameObject>();
    private Dictionary<int, GetQuset> m_DicQuestInterface = new Dictionary<int, GetQuset>();

    bool m_bTouch = false;

    private int m_nMoney;
    private GameObject m_RewardObj;

    private Inventory m_Inventory;
    private BuyMenu m_Buymenu;

    private bool m_bOneSound = false;

    public void Init(QuestMenu questMenu, int key, out GetQuset getQuset)
    {
        m_QuestMenu = questMenu;
        m_DicQuestInterface = questMenu.m_DicQuestInterface;
        getQuset = this;

        QuestTxt.text = questMenu.m_QuestTxt.text;
        GoalTxt.text = questMenu.m_nCount.ToString();
        CountTxt.text = m_nCount.ToString();
        m_nGoal = questMenu.m_nCount;


        m_nkey = key;

        m_DicSaveQuest = questMenu.m_DicSaveQuest;
        m_CancleQuestObj = questMenu.m_CancleQuestObj;

        m_Color = new Color(0.0f, 1.0f, 0.0f);

        m_nMoney = questMenu.m_nMoney;
        m_RewardObj = questMenu.m_RewardObj;

        m_Inventory = questMenu.m_Iventory;
        m_Buymenu = questMenu.m_BuyMenu;
    }

    public void CompleteBtn()
    {
        GameObject inven_accessory = Instantiate(m_RewardObj);
        inven_accessory.transform.SetParent(m_Inventory.m_AccessoryScreen.transform);

        GameObject buy_accessory = Instantiate(m_RewardObj);
        buy_accessory.transform.SetParent(m_Buymenu.m_InventoryShow.m_AccessoryScreenObj.transform);

        AccessoryEquipment accessoryEquipment = inven_accessory.GetComponent<AccessoryEquipment>();
        accessoryEquipment.Init(m_Inventory, buy_accessory);
        inven_accessory.GetComponent<Button>().onClick.AddListener(delegate { accessoryEquipment.AskInstall(); });

        m_Inventory.SetMoney(m_Inventory.m_Money + m_nMoney);
        m_Inventory.SetMoney();
        m_Buymenu.m_InventoryShow.m_MoneyTxt.text = m_Inventory.m_Money.ToString();

        //List는 0부터 키는 1부터
        m_QuestMenu.CompleteBtn(m_nkey - 1);

        inven_accessory.transform.localScale = new Vector3(0.14f, 0.15f, 0.14f);
        inven_accessory.GetComponent<RectTransform>().pivot = new Vector2(0, 0.79f);
        buy_accessory.transform.localScale = new Vector3(0.14f, 0.20f, 0.14f);
        buy_accessory.GetComponent<RectTransform>().pivot = new Vector2(0, 1.35f);

        m_bOneSound = false;
    }

    public void TouchQuest()
    {
        m_bTouch = true;
        if (m_nCount >= m_nGoal)
        {
            CompleteBtn();
            CancleQuestYes();
        }
        else
        {
            m_CancleQuestObj.SetActive(true);
        }
    }

    public void CancleQuestYes()
    {
        if (m_bTouch)
        {
            m_DicQuestInterface.Remove(m_nkey);
            m_DicSaveQuest.Remove(m_nkey);
            Destroy(gameObject);
            m_CancleQuestObj.SetActive(false);

            m_bTouch = false;

            AudioManager.Instance.PlayEffect(0);
        }
    }

    public void GoQuesting()
    {
        m_CancleQuestObj.SetActive(false);

        m_bTouch = false;
    }

    public void saveQuest(int key)
    {
        ++m_nCount;
        CountTxt.text = m_nCount.ToString();

        if (m_nCount >= m_nGoal)
        {
            if (!m_bOneSound)
            {
                AudioManager.Instance.PlayEffect(11);
                m_bOneSound = true;
            }
            Image image = GetComponent<Image>();
            image.color = m_Color;
        }
    }
}
