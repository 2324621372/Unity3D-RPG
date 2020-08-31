using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tow_Hand_Skill_1 : Skill
{
    public override void Init()
    {
        m_sName = "강화";
        m_fAttack = 10;
        m_fCoolTime = 30.0f;
        m_nLevel= 0;
        m_nMp = 100;
        m_fNextAttack = 10 + m_fAttack;

        m_fTime = 5.0f;
        m_fNextTime = 1 + m_fTime;

        m_nIndex = 1;
    }

    public override void setLevel()
    {
        if (m_nLevel!=0)
        {
            m_fAttack = m_fNextAttack;
            m_fNextAttack = 10 + m_fAttack;
            ++m_fTime;
            m_fNextTime = 1 + m_fTime;
        }
        ++m_nLevel;
    }
}
