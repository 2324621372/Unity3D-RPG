using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    [HideInInspector]
    public int m_nAttackCount;

    [HideInInspector]
    public bool m_bAniStat = true;
    private bool m_bMove = false;

    public string m_sAttackState;
    private float m_nAttackPos;

    private int m_nAttackSave;
    private bool m_bOneSkillShot = false;

    public void Attack_AI()
    {
       m_Ani.SetInteger("Action", 2);
    }

    public void Attack_Boss()
    {
        if (m_nAttackSave >= 4&& !m_bOneSkillShot)
        {
            m_Ani.SetTrigger("Skill");
            m_bOneSkillShot = true;
        }
        else
        {
            m_Ani.SetInteger("Action", 2);
        }
    } 

    public void Attack_Player(ref bool a_bAttackState, ref bool a_bSkillState, int a_fSkillIndex)
    {
        AnimatorStateInfo stateinfo = m_Ani.GetCurrentAnimatorStateInfo(0);

        if (stateinfo.IsTag("ATTACK"))
        {
            if (stateinfo.normalizedTime > (stateinfo.length) * 0.85f)
            {
                if (m_Ani.IsInTransition(0))
                    return;

                m_nAttackCount = 0;
                m_Ani.SetInteger("Attack", m_nAttackCount);
                m_sAttackState = "Idle";
                a_bAttackState = false;
            }
        }
        if (m_bMove)
        {
            Vector3 worldDir = new Vector3(0, 0, m_nAttackPos) * Time.deltaTime;
            transform.Translate(worldDir, Space.Self);
        }

        if (a_bAttackState == true)
        {
            m_sAttackState = "Attack";
            if (a_bSkillState)
            {
                m_Ani.SetInteger("Skill", a_fSkillIndex);
            }
            else
            {
                if (stateinfo.IsName("Idle"))
                {
                    m_nAttackCount = 1;
                    m_Ani.SetInteger("Attack", m_nAttackCount);
                }
                else
                {
                    if (m_Ani.IsInTransition(0))
                        return;

                    if (stateinfo.normalizedTime > (stateinfo.length * 0.6f))
                    {
                        ++m_nAttackCount;
                        m_Ani.SetInteger("Attack", m_nAttackCount);

                        if (m_nAttackCount == 4)
                        {
                            m_Ani.SetFloat("MoveX", 0.0f);
                            m_Ani.SetFloat("MoveZ", 0.0f);
                        }
                    }
                }
            }
            a_bAttackState = false;
            a_bSkillState = false;
        }
    }
    public void IdleChange()
    {
        m_sAttackState = "Idle";
        m_nAttackPos = 0;
        m_Ani.SetInteger("Skill", 0);
        m_bMove = false;
        m_nAttackCount = 0;
    }

    //스킬사용 시 직접 위치 값을 지정하였습니다.
    public void PosChange(float pos)
    {
        m_bMove = true;
        m_nAttackPos = pos;
    }

    //Idle로 가지 않고 애니메이션의 음직임을 멈추기위한 부분입니다.
    public void PosNotChange()
    {
        m_bMove = false;
        m_nAttackPos = 0;
    }

    public void AttackCount()
    {
        ++m_nAttackSave;
    }
    public void AttackCountZero()
    {
        m_nAttackSave = 0;
        m_bOneSkillShot = false;
    }

}
