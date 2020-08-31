using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAbility : Ability
{
    private string m_sName;
    private int m_nAllHp;
    private int m_nEXP;

    public int m_EXP { get { return m_nEXP; } }
    public int m_AllHp { get { return m_nAllHp; } }
    public string m_Name { get { return m_sName; } }

    //몬스터의 영역을 나타내기 위한 변수
    public int m_nGate;

    public override void Init()
    {
        m_nAttack = DataMng.Get(TableType.MonsterTable).ToI(m_nIndex, "ATTACK");
        m_nDefense = DataMng.Get(TableType.MonsterTable).ToI(m_nIndex, "DEFENCE");
        m_nHp = DataMng.Get(TableType.MonsterTable).ToI(m_nIndex, "HP");
        m_nEXP = DataMng.Get(TableType.MonsterTable).ToI(m_nIndex, "EXP");
        m_nAllHp = DataMng.Get(TableType.MonsterTable).ToI(m_nIndex, "HP");
        m_sName = DataMng.Get(TableType.MonsterTable).ToS(m_nIndex, "NAME");
    }

    public override void DecreaseHp(int attack,out int sum)
    {
        int _attack;
        _attack = attack - m_Defense;

        if (_attack < 0)
        {
            _attack = 0;
        }
        sum = _attack;
        SetHp(m_Hp - _attack);
    }
}
