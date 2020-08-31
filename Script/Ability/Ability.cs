using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public int m_nIndex;

    protected int m_nAttack;
    protected int m_nDefense;
    protected int m_nHp;

    public int m_Attack { get { return m_nAttack; } }
    public int m_Defense { get { return m_nDefense; } }
    public int m_Hp { get { return m_nHp; } }

    public void SetAttack(int attack){  m_nAttack = attack;}
    public void SetDefense(int defense) { m_nDefense = defense; }
    public void SetHp(int hp) { m_nHp = hp; }

    public virtual void Init() { }
    public virtual void DecreaseHp(int attack, out int sum){sum = attack;}
}
