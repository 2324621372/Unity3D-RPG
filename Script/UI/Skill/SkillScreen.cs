using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface SkillCount
{
    void setSkillPoiont(int count);
}
public class SkillScreen : MonoBehaviour, SkillCount
{
    [SerializeField]
    private Text m_SkillCount;
    [SerializeField]
    private GameObject m_SkillList;

    private Dictionary<int, GameObject> m_LisSkillSlot = new Dictionary<int, GameObject>();
    private Dictionary<GameObject, GameObject> m_LisAddBtn = new Dictionary<GameObject, GameObject>();

    private RectTransform m_RectTransform;

    private int m_BtnCount = 0;

    public GameObject[] m_Character = new GameObject[2];
    public GameObject[] m_Partical = new GameObject[2];

    private PlayerAbility m_PlayerAbility;

    public void Init(PlayerController playerController)
    {
        m_RectTransform = m_SkillList.GetComponent<RectTransform>();
        m_PlayerAbility = playerController.m_PlayerAbility;
        Animator _Ani = null;
        switch (playerController.m_PlayerAbility.m_nIndex)
        {
            case 1:
                m_Character[0].SetActive(true);
                m_Character[1].SetActive(false);

                m_Partical[0].SetActive(true);
                m_Partical[1].SetActive(false);
                _Ani = m_Character[0].GetComponent<Animator>();
                break;
            case 2:
                m_Character[0].SetActive(false);
                m_Character[1].SetActive(true);

                m_Partical[0].SetActive(false);
                m_Partical[1].SetActive(true);
                _Ani = m_Character[1].GetComponent<Animator>();
                break;
        }

        for (int i = 0; i < m_SkillList.transform.childCount; ++i)
        {
            m_LisSkillSlot.Add(i, m_SkillList.transform.GetChild(i).gameObject);
            m_LisAddBtn.Add(m_LisSkillSlot[i], m_LisSkillSlot[i].transform.Find("SkillAddBtn").gameObject);
            m_LisSkillSlot[i].GetComponent<SkillAddBtn>().Init(playerController.m_LisSkill[i], playerController.m_PlayerAbility.m_nIndex - 1, _Ani, this);
        }
    }
    private void Update()
    {
        if (int.Parse(m_SkillCount.text) > 0)
        {
            for (int i = 0; i < m_LisAddBtn.Count; ++i)
            {
                m_LisAddBtn[m_LisSkillSlot[i]].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < m_LisAddBtn.Count; ++i)
            {
                m_LisAddBtn[m_LisSkillSlot[i]].SetActive(false);
            }
        }
    }

    public void ScreenClose()
    {
        AudioManager.Instance.PlayEffect(0);
        gameObject.SetActive(false);
    }

    public void ListUp()
    {
        if (m_BtnCount > 0)
        {
            AudioManager.Instance.PlayEffect(0);
            m_RectTransform.position = new Vector3(m_RectTransform.position.x, m_RectTransform.position.y - 278, m_RectTransform.position.z);
            --m_BtnCount;
        }
    }

    public void ListDown()
    {
        if (m_BtnCount < 3)
        {
            AudioManager.Instance.PlayEffect(0);
            m_RectTransform.position = new Vector3(m_RectTransform.position.x, m_RectTransform.position.y + 278, m_RectTransform.position.z);
            ++m_BtnCount;
        }
    }

    public void setSkillPoiont(int count)
    {
        m_PlayerAbility.SetSkillPoint(count);
    }

    public void InputSkill()
    {
        m_SkillCount.text = m_PlayerAbility.m_SkillPoint.ToString();
    }

}
