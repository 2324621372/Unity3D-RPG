using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician_Skill_1 : Skill
{
    public override void Init()
    {
        m_sName = "파이어볼";
        m_nDamege = 50;
        m_fCoolTime = 5.0f;
        m_nLevel = 0;
        m_nMp = 100;
        m_fNextDamege = 10 + m_nDamege;
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
