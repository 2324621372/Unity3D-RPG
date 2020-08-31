using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician_Skill_4 : Skill
{
    public override void Init()
    {
        m_sName = "실드";
        m_fDefense = 10;
        m_fCoolTime = 15.0f;
        m_nLevel = 0;
        m_nMp = 90;
        m_fNextDefense = 10 + m_fDefense;
        m_fTime = 5.0f;
        m_fNextTime = 1+m_fTime;
        m_nIndex = 3;
    }

    public override void setLevel()
    {
        if (m_nLevel != 0)
        {
            m_fDefense += 10;
            m_fNextDefense = 10 + m_fDefense;
            ++m_fTime;
            m_fNextTime = 1 + m_fTime;
        }
        ++m_nLevel;
    }
}
