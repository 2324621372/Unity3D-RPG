using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOutput : MonoBehaviour
{

    [SerializeField]
    private Transform DamageBox;
    [SerializeField]
    private Transform NameBox;
    [SerializeField]
    private Transform HpBox;
    [SerializeField]
    private Transform NPCBox;
    [SerializeField]
    private Transform QuestBox;

    private int m_nDamgeCount;
    private int m_nAddCount;

    private GameObject DamageObj;
    private GameObject NameObj;
    private GameObject HpObj;

    //On Off부분
    private List<GameObject> m_LisDamageTxt = new List<GameObject>();
    private List<GameObject> m_LisHpImg = new List<GameObject>();
    private List<GameObject> m_LisNameTxt = new List<GameObject>();

    //스크립트에서 실직적으로 사용되는 부분
    private Dictionary<GameObject, DamageTxt> m_DicDamageTxt = new Dictionary<GameObject, DamageTxt>();
    private Dictionary<int, Text> m_DicNameTxt = new Dictionary<int, Text>();
    private Dictionary<int, Image> m_DicHpImg = new Dictionary<int, Image>();

    //Hp와 name부분을 id로 간단히 호출하기 위한 부분
    public Dictionary<int, Image> m_DicHpId = new Dictionary<int, Image>();
    public Dictionary<int, Text> m_DicNameId = new Dictionary<int, Text>();

    //Update에서 사용하기 위한 부분들
    public bool m_fShow = false;
    private List<Text> m_LisShowName = new List<Text>();
    private List<Image> m_LisShowHp = new List<Image>();
    private List<AIController> m_LisAi = new List<AIController>();

    //끌때 사용하기 위한 리스트들의 값을 만들어 놓기 위한 부분이다. Id등록
    public Dictionary<int, int> m_DicInt = new Dictionary<int, int>();

    //NPC클릭하기 위한 부분
    private List<GameObject> m_LisLoadNPC = new List<GameObject>();
    public Dictionary<GameObject, GameObject> m_DicNPCObj = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, Vector3> m_DicNPCpos = new Dictionary<GameObject, Vector3>();

    private Dictionary<GameObject, int> m_DicIndexObj = new Dictionary<GameObject, int>();
    private Dictionary<int, GameObject> m_DicIndex = new Dictionary<int, GameObject>();

    private Dictionary<GameObject, NPC_Btn> m_DicRemoveNPC = new Dictionary<GameObject, NPC_Btn>();

    //자식컴포너트를 On Off 부분
    private Dictionary<int, GameObject> m_BtnObj = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> m_TxtObj = new Dictionary<int, GameObject>();

    public BuyMenu m_BuyMenu;
    public PortionMenu m_PortionMenu;
    public QuestMenu m_QuestMenu;
    public GateAsk m_GateAsk;
    public BossClearAsk m_BossClearAsk;

    private List<GameObject> m_LisWeaponBtnObj = new List<GameObject>();
    private List<GameObject> m_LisDefenseBtnObj = new List<GameObject>();
    private List<GameObject> m_LisPortionBtnObj = new List<GameObject>();

    private TextMesh m_TextMesh;
    public void SendInven(Inventory inven, int index)
    {
        inven.Init(index);
        m_BuyMenu.GetInven(inven);
        m_QuestMenu.GetInven(inven, m_BuyMenu);
        m_PortionMenu.GetInven(inven);

        m_BuyMenu.gameObject.SetActive(false);
        m_PortionMenu.gameObject.SetActive(false);
        m_QuestMenu.gameObject.SetActive(false);
        m_GateAsk.gameObject.SetActive(false);
        m_BossClearAsk.gameObject.SetActive(false);
    }

    public void Init(out GetQuset getQuset, MakeBoss makeBoss, PlayerAbility playerAbility)
    {
        DamageObj = Resources.Load("UI/Box/DamegeTxt") as GameObject;
        NameObj = Resources.Load("UI/Box/NameTxt") as GameObject;
        HpObj = Resources.Load("UI/Box/HpImg") as GameObject;

        m_LisLoadNPC.Add(Resources.Load("UI/NPC/Buy_NPC") as GameObject);
        m_LisLoadNPC.Add(Resources.Load("UI/NPC/Quest_NPC") as GameObject);
        m_LisLoadNPC.Add(Resources.Load("UI/NPC/Portion_NPC") as GameObject);
        m_LisLoadNPC.Add(Resources.Load("UI/NPC/Boss_NPC") as GameObject);
        m_LisLoadNPC.Add(Resources.Load("UI/NPC/Clear_NPC") as GameObject);

        //데미지 부분 미리 10개 생성
        for (int i = 0; i < 10; ++i)
        {
            GameObject damageObj = Instantiate(DamageObj);

            DamageTxt damageTxt = damageObj.GetComponent<DamageTxt>();
            damageObj.transform.SetParent(DamageBox);

            m_LisDamageTxt.Add(damageObj);
            m_DicDamageTxt.Add(m_LisDamageTxt[i], damageTxt);

            damageTxt.Init();
            damageObj.SetActive(false);
        }

        for (int i = 0; i < 5; ++i)
        {
            GameObject hpObj = Instantiate(HpObj, HpBox.transform);
            Image hpimg = hpObj.GetComponent<Image>();
            m_LisHpImg.Add(hpObj);
            m_DicHpImg.Add(i, hpimg);
            hpObj.SetActive(false);

            GameObject nameObj = Instantiate(NameObj, NameBox.transform);
            Text nameTxt = nameObj.GetComponent<Text>();
            m_LisNameTxt.Add(nameObj);
            m_DicNameTxt.Add(i, nameTxt);
            nameObj.SetActive(false);

            ++m_nAddCount;
        }

        m_DicIndexObj.Add(m_BuyMenu.gameObject, 0);
        m_DicIndexObj.Add(m_QuestMenu.gameObject, 1);
        m_DicIndexObj.Add(m_PortionMenu.gameObject, 2);
        m_DicIndexObj.Add(m_BossClearAsk.gameObject, 3);

        m_DicIndex.Add(0, m_BuyMenu.gameObject);
        m_DicIndex.Add(1, m_QuestMenu.gameObject);
        m_DicIndex.Add(2, m_PortionMenu.gameObject);
        m_DicIndex.Add(3, m_GateAsk.gameObject);
        m_DicIndex.Add(4, m_BossClearAsk.gameObject);

        Object[] weaponbtnObj = null;
        Object[] defenseObj = Resources.LoadAll("UI/EquipmentBtn/Armor");
        Object[] portionObj = Resources.LoadAll("UI/EquipmentBtn/Portion");

        switch (playerAbility.m_nIndex)
        {
            case 1:
                weaponbtnObj = Resources.LoadAll("UI/EquipmentBtn/Weapon/TowHandSword");
                break;
            case 2:
                weaponbtnObj = Resources.LoadAll("UI/EquipmentBtn/Weapon/Magician");
                break;
        }

        for (int i = 0; i < weaponbtnObj.Length; ++i)
        {
            m_LisWeaponBtnObj.Add(weaponbtnObj[i] as GameObject);
        }
        for (int i = 0; i < defenseObj.Length; ++i)
        {
            m_LisDefenseBtnObj.Add(defenseObj[i] as GameObject);
        }
        for (int i = 0; i < portionObj.Length; ++i)
        {
            m_LisPortionBtnObj.Add(portionObj[i] as GameObject);
        }
        m_BuyMenu.Init(m_LisWeaponBtnObj, m_LisDefenseBtnObj, playerAbility);
        m_QuestMenu.Init(out getQuset);
        m_PortionMenu.Init(m_LisPortionBtnObj);

        PlayerPos _playerPos = m_BossClearAsk.Init();
        m_GateAsk.Init(makeBoss, _playerPos, QuestBox.gameObject);
    }

    public void SetDamageOutput(int damege, Vector3 Pos, bool critical)
    {
        bool _make = false;
        Pos = new Vector3(Pos.x, Pos.y + 220, Pos.z);

        for (int i = 0; i < m_LisDamageTxt.Count; ++i)
        {
            if (!m_LisDamageTxt[i].activeSelf)
            {
                m_LisDamageTxt[i].SetActive(true);
                m_LisDamageTxt[i].transform.position = Pos;

                if (critical)
                {
                    m_DicDamageTxt[m_LisDamageTxt[i]].SetTxt("CRITICAL");
                }
                else
                {
                    m_DicDamageTxt[m_LisDamageTxt[i]].SetTxt(damege.ToString());
                }
                _make = true;

                StartCoroutine(TimeActiveFalse(m_LisDamageTxt[i], 0.7f, null));
                break;
            }
        }

        if (!_make)
        {
            GameObject damageObj = Instantiate(DamageObj, Pos, Quaternion.identity);
            DamageTxt damageTxt = damageObj.GetComponent<DamageTxt>();

            damageObj.transform.SetParent(DamageBox);
            m_LisDamageTxt.Add(damageObj);

            m_DicDamageTxt.Add(m_LisDamageTxt[m_LisDamageTxt.Count - 1], damageTxt);

            damageTxt.Init();
            damageTxt.SetTxt(damege.ToString());

            StartCoroutine(TimeActiveFalse(damageObj, 0.7f, null));
        }
    }

    public void SetShow(AIController aIController)
    {
        bool _make = false;

        if (!m_DicInt.ContainsKey(aIController.m_nMonsterId))
        {
            for (int i = 0; i < m_LisNameTxt.Count; ++i)
            {
                if (!m_LisNameTxt[i].activeSelf)
                {
                    m_LisNameTxt[i].SetActive(true);
                    m_DicNameTxt[i].text = aIController.m_MonsterAbility.m_Name;

                    m_LisHpImg[i].SetActive(true);
                    m_DicHpImg[i].fillAmount = (float)aIController.m_MonsterAbility.m_Hp / aIController.m_MonsterAbility.m_AllHp;

                    m_LisShowName.Add(m_DicNameTxt[i]);
                    m_LisShowHp.Add(m_DicHpImg[i]);

                    //있을경우 계산하기 위한 부분
                    m_DicNameId.Add(aIController.m_nMonsterId, m_DicNameTxt[i]);
                    m_DicHpId.Add(aIController.m_nMonsterId, m_DicHpImg[i]);

                    _make = true;

                    m_DicInt.Add(aIController.m_nMonsterId, i);
                    break;
                }
            }

            if (!_make)
            {
                GameObject nameObj = Instantiate(NameObj, NameBox.transform);
                Text nameTxt = nameObj.GetComponent<Text>();
                m_LisNameTxt.Add(nameObj);
                m_DicNameTxt.Add(m_nAddCount, nameTxt);

                m_DicNameTxt[m_nAddCount].text = aIController.m_MonsterAbility.m_Name;

                GameObject hpObj = Instantiate(HpObj, HpBox.transform);
                Image hpimg = hpObj.GetComponent<Image>();
                m_LisHpImg.Add(hpObj);
                m_DicHpImg.Add(m_nAddCount, hpimg);

                m_DicNameId.Add(aIController.m_nMonsterId, nameTxt);
                m_DicHpId.Add(aIController.m_nMonsterId, hpimg);

                m_LisShowName.Add(nameTxt);
                m_LisShowHp.Add(hpimg);

                m_DicHpImg[m_nAddCount].fillAmount = (float)aIController.m_MonsterAbility.m_Hp / aIController.m_MonsterAbility.m_AllHp;

                m_DicInt.Add(aIController.m_nMonsterId, m_nAddCount);

                ++m_nAddCount;
            }

            m_LisAi.Add(aIController);
        }
        else
        {
            m_DicHpId[aIController.m_nMonsterId].fillAmount = (float)aIController.m_MonsterAbility.m_Hp / aIController.m_MonsterAbility.m_AllHp;
        }

        m_fShow = true;
    }

    public IEnumerator TimeActiveFalse(GameObject obj, float time, AIController aIController)
    {
        yield return new WaitForSeconds(time);
        if (obj == null)
        {
            if (m_DicInt.ContainsKey(aIController.m_nMonsterId))
            {
                m_LisNameTxt[m_DicInt[aIController.m_nMonsterId]].SetActive(false);
                m_LisHpImg[m_DicInt[aIController.m_nMonsterId]].SetActive(false);

                //순서를 지켜야한다 추가한 순서를 반대로 했기 떄문이다.
                m_LisShowName.Remove(m_DicNameTxt[m_DicInt[aIController.m_nMonsterId]]);
                m_LisShowHp.Remove(m_DicHpImg[m_DicInt[aIController.m_nMonsterId]]);

                if (m_LisShowName.Count <= 0)
                {
                    m_fShow = false;
                }

                m_LisAi.Remove(aIController);
                m_DicNameId.Remove(aIController.m_nMonsterId);
                m_DicHpId.Remove(aIController.m_nMonsterId);

                m_DicInt.Remove(aIController.m_nMonsterId);
            }
        }
        else
        {
            obj.SetActive(false);
        }
    }

    private void Update()
    {
        if (m_fShow)
        {
            float _fUpPos;
            for (int i = 0; i < m_LisShowName.Count; ++i)
            {
                if (m_LisAi[i].tag.Equals("Boss"))
                {
                    _fUpPos = 4.0f;
                }
                else
                {
                    _fUpPos = 2.0f;
                }
                m_LisShowName[i].transform.position =
                  Camera.main.WorldToScreenPoint(new Vector3(m_LisAi[i].transform.position.x, m_LisAi[i].transform.position.y + _fUpPos + 0.2f, m_LisAi[i].transform.position.z));
                m_LisShowHp[i].transform.position =
                    Camera.main.WorldToScreenPoint(new Vector3(m_LisAi[i].transform.position.x, m_LisAi[i].transform.position.y + _fUpPos, m_LisAi[i].transform.position.z));
            }
            
        }
    }

    public void setNPC(GameObject obj, int index, Vector3 pos)
    {
        int _Index;
        if (!m_DicNPCObj.ContainsKey(obj))
        {
            m_DicNPCObj.Add(obj, Instantiate(m_LisLoadNPC[index]));
            NPC_Btn npc_Btn = m_DicNPCObj[obj].GetComponent<NPC_Btn>();
            npc_Btn.Init(this, m_DicIndex[npc_Btn.m_nIndex].transform);
            _Index = npc_Btn.m_nIndex;

            m_DicRemoveNPC.Add(obj, npc_Btn);

            m_DicNPCObj[obj].transform.position = Camera.main.WorldToScreenPoint(pos);
            m_DicNPCObj[obj].transform.SetParent(NPCBox);

            m_BtnObj.Add(_Index, npc_Btn.m_BtnObj);
            m_TxtObj.Add(_Index, npc_Btn.m_TextObj);
        }
    }
    public void SetNPCRemove(GameObject obj)
    {
        m_DicNPCObj.Remove(obj);

        m_BtnObj.Remove(m_DicRemoveNPC[obj].m_nIndex);
        m_TxtObj.Remove(m_DicRemoveNPC[obj].m_nIndex);

        Destroy(m_DicRemoveNPC[obj].gameObject);
    }
    public void FixPos(GameObject obj, Vector3 pos)
    {
        m_DicNPCObj[obj].transform.position = Camera.main.WorldToScreenPoint(pos);
    }

    public void removeNPC(GameObject obj)
    {
        if (m_DicNPCObj.ContainsKey(obj))
        {
            m_DicNPCObj[obj].SetActive(false);
        }
    }

    public void SetCancle(GameObject Obj)
    {
        Obj.SetActive(false);
        m_BtnObj[m_DicIndexObj[Obj]].SetActive(true);
        m_TxtObj[m_DicIndexObj[Obj]].SetActive(true);
       
    }

}
