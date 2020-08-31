using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician_Skill_3 : Skill
{
    public override void Init()
    {
        m_sName = "아이스에이지";
        m_nDamege = 10;
        m_fCoolTime = 15.0f;
        m_nLevel = 0;
        m_nMp = 90;
        m_fNextDamege = 10 + m_nDamege;
        m_nIndex = 2;
    }

    public override void setLevel()
    {
        if (m_nLevel != 0)
        {
            m_nDamege += 10;
            m_fNextDamege = 10 + m_nDamege;
        }
        ++m_nLevel;
    }
}
