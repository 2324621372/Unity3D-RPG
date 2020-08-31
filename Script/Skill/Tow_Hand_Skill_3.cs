using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tow_Hand_Skill_3 : Skill
{
    public override void Init()
    {
        m_sName = "참격";
        m_nDamege = 10;
        m_fCoolTime = 8.0f;
        m_nLevel = 0;
        m_nMp = 50;
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
