using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tow_Hand_Skill_4 : Skill
{
    public override void Init()
    {
        m_sName = "회오리 베기";
        m_nDamege = 10;
        m_fCoolTime = 15.0f;
        m_nLevel = 0;
        m_nMp = 90;
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
