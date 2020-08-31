using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerState : MonoBehaviour
{
    [HideInInspector]
    public PlayerAbility m_PlayerAbility;
    private ParticleManager m_ParticleManager;

    [SerializeField]
    private Text m_AllHpTxt;
    [SerializeField]
    private Text m_AllMpTxt;

    [SerializeField]
    private Text m_LevelTxt;
    [SerializeField]
    private Text m_EXPTxt;
    [SerializeField]
    private Text m_HpTxt;
    [SerializeField]
    private Text m_MpTxt;

    [SerializeField]
    private Image m_HpImg;
    [SerializeField]
    private Image m_MpImg;
    [SerializeField]
    private Image m_EXPImg;


    void Start()
    {
        m_PlayerAbility = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbility>();
        m_ParticleManager = FindObjectOfType<ParticleManager>();

        m_AllHpTxt.text = m_PlayerAbility.m_Hp.ToString();
        m_AllMpTxt.text = m_PlayerAbility.m_Mp.ToString();

        m_HpTxt.text = m_PlayerAbility.m_Hp.ToString();
        m_MpTxt.text = m_PlayerAbility.m_Mp.ToString();
    }

    public void SetHp()
    {
        m_HpTxt.text = m_PlayerAbility.m_Hp.ToString();
        m_HpImg.fillAmount = m_PlayerAbility.m_Hp / (float)m_PlayerAbility.m_AllHp;
    }

    public void SetMp()
    {
        m_MpTxt.text = m_PlayerAbility.m_Mp.ToString();
        m_MpImg.fillAmount =m_PlayerAbility.m_Mp / (float)m_PlayerAbility.m_AllMp;    
    }

    public void SetAllHp()
    {
        m_AllHpTxt.text = m_PlayerAbility.m_AllHp.ToString();
    }

    public void SetAllMp()
    {
        m_AllMpTxt.text = m_PlayerAbility.m_AllMp.ToString();
    }

    int m_nLevel=1;

    public void SetEXPUP(int level)
    {
        if(m_nLevel< level)
        {
            m_nLevel = level;

            SetHp();
            SetMp();

            SetAllHp();
            SetAllMp();
            m_ParticleManager.SetEXP(m_PlayerAbility.transform.position);
        }
        m_LevelTxt.text = m_PlayerAbility.m_Level.ToString();
        m_EXPTxt.text = m_PlayerAbility.m_Exp.ToString();

        m_EXPImg.fillAmount = m_PlayerAbility.m_Exp / m_PlayerAbility.m_AllExp;
    }
}
