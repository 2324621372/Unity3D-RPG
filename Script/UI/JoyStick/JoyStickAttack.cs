using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickAttack : MonoBehaviour
{
    [HideInInspector]
    public int m_nAttackCount;
    [HideInInspector]
    public bool m_bAttackStat;
    [HideInInspector]
    public int m_fSkillIndex;
    [HideInInspector]
    public bool m_fSKillStat;

    [SerializeField]
    private Image[] m_CoolTimeDownImg = new Image[6];
    private bool[] m_bCoolTimeDown = new bool[6];
    private float[] m_fCoolTimeDownSave=new float[6];
    private int m_nMp;

    [HideInInspector]
    public UIPlayerState m_UIPlayerState;
    private Dictionary<int, float> m_DicCoolTimeDown = new Dictionary<int, float>();
    private Dictionary<int, int> m_DicSlot = new Dictionary<int, int>();

    private void Start()
    {
        for (int i = 0; i < m_bCoolTimeDown.Length; ++i)
        {
            m_bCoolTimeDown[i] = true;
        }
        m_UIPlayerState = FindObjectOfType<UIPlayerState>();
    }

    public void OnAttackBtn()
    {
        m_bAttackStat = true;
        m_fSKillStat = false;
    }

    public void SkillIndex(int a_fSkill, float a_fSkillCoolTime, int slot, int Mp)
    {
        if (m_UIPlayerState.m_PlayerAbility.m_Mp > Mp)
        {
            if (m_bCoolTimeDown[slot])
            {
                if (m_DicCoolTimeDown.ContainsKey(a_fSkill))
                {
                    m_DicCoolTimeDown.Remove(a_fSkill);
                    m_DicSlot.Remove(a_fSkill);
                }

                m_DicCoolTimeDown.Add(a_fSkill, a_fSkillCoolTime);
                m_DicSlot.Add(a_fSkill, slot);

                m_fSkillIndex = a_fSkill;
                m_nMp = Mp;
                m_bAttackStat = true;
                m_fSKillStat = true;
            }
        }
    }

    public IEnumerator Cooltime(int index)
    {
        m_UIPlayerState.m_PlayerAbility.SetMp(m_UIPlayerState.m_PlayerAbility.m_Mp - m_nMp);
        m_UIPlayerState.SetMp();

        m_bCoolTimeDown[m_DicSlot[index]] = false;

        m_CoolTimeDownImg[m_DicSlot[index]].fillAmount = 1;

        while (true)
        {
            m_fCoolTimeDownSave[m_DicSlot[index]] += Time.deltaTime;
            yield return null;
            m_CoolTimeDownImg[m_DicSlot[index]].fillAmount = 1 - (m_fCoolTimeDownSave[m_DicSlot[index]] / m_DicCoolTimeDown[index]);

            if (m_fCoolTimeDownSave[m_DicSlot[index]] > m_DicCoolTimeDown[index])
            {
                m_bCoolTimeDown[m_DicSlot[index]] = true;
                m_fCoolTimeDownSave[m_DicSlot[index]] = 0;
                break;
            }
        }
    }
}
