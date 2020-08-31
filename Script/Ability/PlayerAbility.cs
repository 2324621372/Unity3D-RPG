using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : Ability
{
    private int m_nMp;
    private float m_fExp;
    private int m_nAllHp;
    private int m_nAllMp;
    private int m_nCritical;
    private float m_fAllExp;
    private int m_nLevel = 1;
    private int m_nSkillPoint = 6;

    public int m_nNotBuffAttack;
    public int m_nNotBuffDefense;

    public float m_AllExp { get { return m_fAllExp; } }
    public int m_Level { get { return m_nLevel; } }
    public int m_Mp { get { return m_nMp; } }
    public int m_Critical { get { return m_nCritical; } }
    public float m_Exp { get { return m_fExp; } }
    public int m_AllHp { get { return m_nAllHp; } }
    public int m_AllMp { get { return m_nAllMp; } }
    public int m_SkillPoint { get { return m_nSkillPoint; } }

    public void SetMp(int mp) { m_nMp = mp; }
    public void SetCritical(int critical) { m_nCritical = critical; }
    public void SetExp(float exp) { m_fExp = exp; }
    public void SetAllHp(int hp) { m_nAllHp = hp; }
    public void SetAllMp(int mp) { m_nAllMp = mp; }
    public void SetAllExp(int exp) { m_fAllExp = exp; }
    public void SetLevel(int level) { m_nLevel = level; }

    public void SetSkillPoint(int point) { m_nSkillPoint = point; }

    private Coroutine m_AttackBuff = null;
    private Coroutine m_DefenseBuff = null;

    public override void Init()
    {
        m_nAttack = DataMng.Get(TableType.PlayerTable).ToI(m_nIndex, "ATTACK");
        m_nDefense = DataMng.Get(TableType.PlayerTable).ToI(m_nIndex, "DEFENCE");
        m_nHp = DataMng.Get(TableType.PlayerTable).ToI(m_nIndex, "HP");
        m_nMp = DataMng.Get(TableType.PlayerTable).ToI(m_nIndex, "MP");
        m_nCritical = DataMng.Get(TableType.PlayerTable).ToI(m_nIndex, "Critical");
        m_fAllExp = DataMng.Get(TableType.EXPTable).ToI(m_Level, "EXP");

        m_nAllHp = m_Hp;
        m_nAllMp = m_Mp;
    }

    public override void DecreaseHp(int attack, out int sum)
    {
        int _attack;
        _attack = attack - m_Defense;

        if (_attack < 0)
        {
            _attack = 0;
        }
        sum = _attack;
        SetHp(m_Hp - _attack);
        if (m_Hp < 0)
        {
            SetHp(0);
        }
    }

    public void SetLevelUp(int exp)
    {
        SetExp(m_Exp + exp);
        if (m_AllExp <= m_Exp)
        {
            SetLevel(m_Level + 1);
            SetExp(m_Exp - m_AllExp);
            SetSkillPoint(m_SkillPoint + 2);

            SetAllExp(DataMng.Get(TableType.EXPTable).ToI(m_Level, "EXP"));

            SetHp(m_AllHp + DataMng.Get(TableType.EXPTable).ToI(m_Level, "HP"));

            SetMp(m_AllMp + DataMng.Get(TableType.EXPTable).ToI(m_Level, "MP"));

            SetAllHp(m_Hp);
            SetAllMp(m_Mp);

            SetAttack(m_Attack + DataMng.Get(TableType.EXPTable).ToI(m_Level, "ATTACK"));
            SetDefense(m_Defense + DataMng.Get(TableType.EXPTable).ToI(m_Level, "DEFENSE"));
        }
    }

    public void buff(int type, int effect)
    {
        switch (type)
        {
            case 3:
                if (m_AttackBuff != null)
                {
                    StopCoroutine(m_AttackBuff);
                    m_AttackBuff = null;
                }
                m_AttackBuff = StartCoroutine(UpBuff(type, effect));
                break;
            case 4:
                if (m_DefenseBuff != null)
                {
                    StopCoroutine(m_DefenseBuff);
                    m_DefenseBuff = null;
                }
                m_DefenseBuff = StartCoroutine(UpBuff(type, effect));
                break;
        }
    }

    public IEnumerator UpBuff(int type, int effect)
    {
        switch (type)
        {
            case 3:
                m_nNotBuffAttack = m_Attack;
                SetAttack(m_Attack + (int)(m_Attack * ((float)effect / 100)));
                break;
            case 4:
                m_nNotBuffDefense = m_Defense;
                SetDefense(m_Defense + (int)(m_Defense * ((float)effect / 100)));
                break;
        }
        yield return new WaitForSeconds(60.0f);
        switch (type)
        {
            case 3:
                SetAttack(m_Attack - (int)(m_nNotBuffAttack * ((float)effect / 100)));
                m_AttackBuff = null;
                break;
            case 4:
                SetDefense(m_Defense - (int)(m_nNotBuffDefense * ((float)effect / 100)));
                m_DefenseBuff = null;
                break;
        }
    }
}
