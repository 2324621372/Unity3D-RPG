using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public string m_sName;
    public float m_nDamege;
    public float m_fCoolTime;
    public int m_nMp;
    public int m_nLevel;
    public float m_fNextDamege;

    public float m_fAttack;
    public float m_fNextAttack;
    public float m_fDefense;
    public float m_fNextDefense;
    public float m_fTime;
    public float m_fNextTime;

    //이걸로 aniWindow의 내용을 바꿀 예정이다.
    public int m_nIndex;

    public abstract void Init();
    public abstract void setLevel();

}
