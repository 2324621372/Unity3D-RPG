using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AniWindow : MonoBehaviour
{
    [SerializeField]
    private Text m_NameTxt;
    [SerializeField]
    private Text m_MPTxt;
    [SerializeField]
    private Text m_CoolDownTimeTxt;
    [SerializeField]
    private GameObject m_State;

    private Text m_DamageTxt;
    private Text m_NextDamageTxt;

    private Text[] m_ATKDEFTxt =new Text[2];
    private Text[] m_NexDefenseTxt=new Text[2];

    private Text m_Timetxt;
    private Text m_NextTimetxt;

    public GameObject[] m_TimeOBj = new GameObject[4];
    public GameObject[] m_DamageOBj = new GameObject[4];
    public GameObject[] m_ATKDEFOBj = new GameObject[4];

    public void Init()
    {
        m_DamageTxt = m_DamageOBj[2].GetComponent<Text>();
        m_NextDamageTxt = m_DamageOBj[3].GetComponent<Text>();

        m_ATKDEFTxt [0] = m_ATKDEFOBj[0].GetComponent<Text>();
        m_NexDefenseTxt[0] = m_ATKDEFOBj[1].GetComponent<Text>();

        m_ATKDEFTxt [1] = m_ATKDEFOBj[2].GetComponent<Text>();
        m_NexDefenseTxt[1] = m_ATKDEFOBj[3].GetComponent<Text>();

        m_Timetxt = m_TimeOBj[2].GetComponent<Text>();
        m_NextTimetxt = m_TimeOBj[3].GetComponent<Text>();
    }

    public void setInput(Skill skill)
    {
        m_NameTxt.text = skill.m_sName;
        m_CoolDownTimeTxt.text = skill.m_fCoolTime.ToString() + " 초";
        m_MPTxt.text = skill.m_nMp.ToString();

        switch (skill.m_nIndex)
        {
            case 1:
                m_State.SetActive(false);

                for (int i = 0; i < m_TimeOBj.Length; ++i)
                {
                    m_TimeOBj[i].SetActive(true);
                }

                for (int i = 0; i < m_ATKDEFOBj.Length; ++i)
                {
                    m_DamageOBj[i].SetActive(false);
                    m_ATKDEFOBj[i].SetActive(true);
                }
                m_Timetxt.text = " " + skill.m_fTime.ToString() + " 초";
                m_NextTimetxt.text = " " + skill.m_fNextTime.ToString() + " 초";

                m_ATKDEFTxt[0].text = "공격력 증가 ";
                m_NexDefenseTxt[0].text = "다음 레벨 공격력 증가";
                m_ATKDEFTxt[1] .text = " + " + skill.m_fAttack.ToString() + " %";
                m_NexDefenseTxt[1].text = " + " + skill.m_fNextAttack.ToString() + " %";
                break;
            case 2:
                m_State.SetActive(true);

                for (int i = 0; i < m_TimeOBj.Length; ++i)
                {
                    m_TimeOBj[i].SetActive(false);
                }

                for (int i = 0; i < m_ATKDEFOBj.Length; ++i)
                {
                    m_DamageOBj[i].SetActive(true);
                    m_ATKDEFOBj[i].SetActive(false);
                }
                m_DamageTxt.text = " + " + skill.m_nDamege.ToString() + " %";
                m_NextDamageTxt.text = " + " + skill.m_fNextDamege.ToString() + " %";
                break;
            case 3:
                m_State.SetActive(false);

                for (int i = 0; i < m_TimeOBj.Length; ++i)
                {
                    m_TimeOBj[i].SetActive(true);
                }

                for (int i = 0; i < m_ATKDEFOBj.Length; ++i)
                {
                    m_DamageOBj[i].SetActive(false);
                    m_ATKDEFOBj[i].SetActive(true);
                }
                m_Timetxt.text = " "+ skill.m_fTime.ToString() + " 초";
                m_NextTimetxt.text = " " + skill.m_fNextTime.ToString() + " 초";

                m_ATKDEFTxt[0].text = "방어력 증가";
                m_NexDefenseTxt[0].text = "다음 레벨 방어력 증가";
                m_ATKDEFTxt[1].text = " + " + skill.m_fDefense.ToString() + " %";
                m_NexDefenseTxt[1].text = " + " + skill.m_fNextDefense.ToString() + " %";
                break;
            default:
                m_State.SetActive(false);

                for (int i = 0; i < m_TimeOBj.Length; ++i)
                {
                    m_TimeOBj[i].SetActive(false);
                }

                for (int i = 0; i < m_ATKDEFOBj.Length; ++i)
                {
                    m_DamageOBj[i].SetActive(true);
                    m_ATKDEFOBj[i].SetActive(false);
                }
                m_DamageTxt.text = " + " + skill.m_nDamege.ToString() + " %";
                m_NextDamageTxt.text = " + " + skill.m_fNextDamege.ToString() + " %";
                break;
        }
    }
}
