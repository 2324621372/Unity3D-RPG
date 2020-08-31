using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface GetQuset
{
    void saveQuest(int key);
}

public class QuestMenu : MonoBehaviour, GetQuset
{
    private int m_nQuestIndex;

    public GameObject m_ContentScreen;

    public Text m_ContentTitleTxt;
    public Text m_ContentTxt;
    public Text m_QuestTxt;

    public Button CompleteOkBtn;

    public Image m_Reward_1Img;
    public Image m_Reward_2Img;

    public Transform m_QuestBoxObj;

    private GameObject m_QuestBar;

    //이미 퀘스트르 받았을 수가 있기 떄문에 중복 퀘스트를 방지하기 위한 부분.
    [HideInInspector]
    public Dictionary<int, GameObject> m_DicSaveQuest = new Dictionary<int, GameObject>();
    [HideInInspector]
    public Dictionary<int, GetQuset> m_DicQuestInterface = new Dictionary<int, GetQuset>();

    public GameObject m_YesBtnObj;
    public GameObject m_NoBtnObj;

    public GameObject m_CancleQuestObj;

    public Button m_CancleQuestYes;
    public Button m_CancleQuestNo;

    [HideInInspector]
    public int m_nCount;

    private GetQuset m_GetQuest;

    [HideInInspector]
    public int m_nMoney;

    private string m_sReward;

    [HideInInspector]
    public GameObject m_RewardObj;
    [HideInInspector]
    public Inventory m_Iventory;
    [HideInInspector]
    public BuyMenu m_BuyMenu;

    [SerializeField]
    private List<Image> m_LisItemImg = new List<Image>();
    [SerializeField]
    private Image m_MoneyImg;

    public void Init(out GetQuset m_GetQuest)
    {
        DataMng.AddTable(TableType.QuestTable);
        m_QuestBar = Resources.Load("UI/Box/QuestBar") as GameObject;
        m_GetQuest = this;
}

    public void GetInven(Inventory inventory, BuyMenu buyMenu)
    {
        m_Iventory = inventory;
        m_BuyMenu = buyMenu;
    }

    public void SetShowQuest(int index)
    {
        string text= DataMng.Get(TableType.QuestTable).ToS(index, "Content");
        m_ContentScreen.SetActive(true);

        m_ContentTitleTxt.text = DataMng.Get(TableType.QuestTable).ToS(index, "Title");
        m_ContentTxt.text = text.Replace("\\n", "\n");
        m_QuestTxt.text = DataMng.Get(TableType.QuestTable).ToS(index, "Quest");
        m_nCount = DataMng.Get(TableType.QuestTable).ToI(index, "Count");
        m_nMoney = DataMng.Get(TableType.QuestTable).ToI(index, "Money");
        m_sReward = DataMng.Get(TableType.QuestTable).ToS(index, "Reward");

        m_RewardObj = Resources.Load("UI/EquipmentBtn/Accessory/" + m_sReward) as GameObject;
        m_nQuestIndex = index;

        if (m_DicSaveQuest.ContainsKey(index))
        {
            m_YesBtnObj.SetActive(false);
            m_NoBtnObj.SetActive(false);
        }
        else
        {
            m_YesBtnObj.SetActive(true);
            m_NoBtnObj.SetActive(true);
        }
    }

    public void GetQuestYes()
    {
        GameObject obj = Instantiate(m_QuestBar);
        obj.transform.SetParent(m_QuestBoxObj);
        
        QuestBar questBar = obj.GetComponent<QuestBar>();
        questBar.Init(this, m_nQuestIndex, out m_GetQuest);
        m_DicSaveQuest.Add(m_nQuestIndex, obj);

        m_CancleQuestYes.onClick.AddListener(delegate { questBar.CancleQuestYes(); });
        m_CancleQuestNo.onClick.AddListener(delegate { questBar.GoQuesting(); });

        m_ContentScreen.SetActive(false);
        m_DicQuestInterface.Add(m_nQuestIndex, questBar);
    }

    public void CancleQuestNo()
    {
        m_ContentScreen.SetActive(false);
    }

    public void saveQuest(int key)
    {
        if (m_DicQuestInterface.ContainsKey(key))
        {
            m_DicQuestInterface[key].saveQuest(key);
        }
    }

    public void CompleteBtn(int index)
    {
        m_Reward_1Img.sprite = m_LisItemImg[index].sprite;
        m_Reward_2Img.sprite = m_MoneyImg.sprite;

        CompleteOkBtn.transform.parent.gameObject.SetActive(true);
    }

    public void CompleteOKBtn()
    {
        CompleteOkBtn.transform.parent.gameObject.SetActive(false);
    }
}
