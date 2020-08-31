using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tow_Hand_Skill_2 : Skill
{
    public override void Init()
    {
        m_sName = "폭발";
        m_nDamege = 20;
        m_fCoolTime = 15.0f;
        m_nLevel = 0;
        m_nMp = 70;
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
