using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public enum ActionType
    {
        Idle=0,
        Move = 2,
        Attack = 3,
        
        Die=4,
    }
    public Bounds m_Bodybounds;
    public Bounds m_Attackbounds;

    protected MoveAction m_MoveAction;
    protected AttackAction m_AttackAction;
    public Animator m_Ani;
    protected CollisionHit m_CollisionHit;
    protected ParticleManager m_ParticleManager;

    public void Init()
    {
        m_ParticleManager = FindObjectOfType<ParticleManager>();
        m_MoveAction = GetComponent<MoveAction>();
        m_AttackAction = GetComponent<AttackAction>();
        m_CollisionHit = FindObjectOfType<CollisionHit>();
        ChildInit();
    }

    public virtual void MoveAction() { }
    public virtual void AttackAction() { }
    public virtual void DieAction() { }
    public virtual void ChildInit() { }

}