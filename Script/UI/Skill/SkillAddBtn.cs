using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillAddBtn : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Animator m_Ani;

    [SerializeField]
    private int m_fSkillIndex;

    [SerializeField]
    private Text m_NameTxt;
    [SerializeField]
    private Text m_AddCountTxt;

    [SerializeField]
    private Text skillCount;

    [SerializeField]
    private GameObject NotSkill;
    [SerializeField]
    private GameObject MoveSkillObj;

    private GameObject moveSkill;

    private Bounds CilckBounds;
    private Bounds bounds;

    private List<Bounds> SlotBounds = new List<Bounds>();

    [SerializeField]
    private RectTransform Install;
    private Dictionary<int, RectTransform> InstallSlot = new Dictionary<int, RectTransform>();

    private SkillSlot skillSlot;
    private JoyStickAttack joyStickAttack;
    private Skill m_Skill;

    private AniWindow m_AniWindow;

    private Image m_SkillImg;

    public Sprite[] m_SkillSprite;
    private SkillCount m_SkillCount;

    public void Init(Skill skill, int skillIndex, Animator ani, SkillCount skillCount)
    {
        skillSlot = FindObjectOfType<SkillSlot>();
        joyStickAttack = FindObjectOfType<JoyStickAttack>();
        m_AniWindow = FindObjectOfType<AniWindow>();
        m_AniWindow.Init();

        for (int i = 0; i < Install.transform.childCount; ++i)
        {
            InstallSlot.Add(i, Install.transform.GetChild(i) as RectTransform);
            bounds.center = InstallSlot[i].position;
            bounds.extents = new Vector3(130f, 130f, 0);
            SlotBounds.Add(bounds);
        }
        CilckBounds.extents = new Vector3(0.2f, 0.2f, 0);
        m_Skill = skill;
        m_NameTxt.text = m_Skill.m_sName;
        m_AniWindow.setInput(m_Skill);

        m_SkillImg = MoveSkillObj.GetComponent<Image>();

        m_SkillImg.sprite = m_SkillSprite[skillIndex];

        m_Ani = ani;
        m_SkillCount = skillCount;
    }

    public void AddSkill(int index)
    {
        m_Skill.setLevel();
        //총 리스트
        int _nAddCount = int.Parse(m_AddCountTxt.text);

        --_nAddCount;
        m_SkillCount.setSkillPoiont(_nAddCount);
        skillCount.text = m_Skill.m_nLevel.ToString();
        m_AddCountTxt.text = _nAddCount.ToString();

        if (m_Skill.m_nLevel > 0)
        {
            NotSkill.SetActive(false);
        }
        m_AniWindow.setInput(m_Skill);
        m_Ani.SetInteger("Skill", index);
        AudioManager.Instance.PlayEffect(0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!NotSkill.activeSelf)
        {
            moveSkill = Instantiate(MoveSkillObj, transform);
            moveSkill.name = MoveSkillObj.name;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (moveSkill != null)
        {
            moveSkill.transform.position = Input.mousePosition;
            CilckBounds.center = moveSkill.transform.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool _NotDrag = false;

        if (moveSkill != null)
        {
            for (int i = 0; i < InstallSlot.Count; ++i)
            {
                if (CilckBounds.Intersects(SlotBounds[i]))
                {
                    _NotDrag = true;

                    //똑같은 부분 없애기 위한 부분
                    for (int j = 0; j < InstallSlot.Count; ++j)
                    {
                        if (InstallSlot[j].transform.Find(moveSkill.name))
                        {
                            skillSlot.m_LisSlot[i].SetActive(false);
                            Destroy(InstallSlot[j].transform.Find(moveSkill.name).gameObject);
                            Destroy(skillSlot.m_LisSlot[j].transform.GetChild(0).gameObject);
                        }
                    }
                    //같은 부분에 있는 스킬을 없애는 부분 그래야 밑에서 새로운 스킬을 입력함
                    if (InstallSlot[i].transform.childCount > 0)
                    {
                        skillSlot.m_LisSlot[i].SetActive(false);
                        Destroy(InstallSlot[i].transform.GetChild(0).gameObject);
                        Destroy(skillSlot.m_LisSlot[i].transform.GetChild(0).gameObject);
                    }
                    //스킬창과 조이스틱 스킬 슬롯에 저장하고 사용하기 위한 부분 윗에는 똑같은 부분을 삭제하는 부분
                    moveSkill.transform.position = InstallSlot[i].position;
                    moveSkill.transform.SetParent(InstallSlot[i]);
                    moveSkill.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

                    skillSlot.m_LisSlot[i].SetActive(true);
                    GameObject skillslot = Instantiate(moveSkill, skillSlot.m_LisSlot[i].transform);
                    skillslot.transform.SetAsFirstSibling();
                    skillslot.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    Button button = skillslot.AddComponent<Button>();
                    button.onClick.AddListener(delegate { joyStickAttack.SkillIndex(m_fSkillIndex, m_Skill.m_fCoolTime, i, m_Skill.m_nMp); });
                    break;
                }
            }
            if (!_NotDrag)
            {
                Destroy(moveSkill);
            }
        }
    }

    public void Ani()
    {
        m_AniWindow.setInput(m_Skill);
        m_Ani.SetTrigger("Idle");
        m_Ani.SetInteger("Skill", m_fSkillIndex);
    }
}
