using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : ControllerManager
{
    [HideInInspector]
    public ActionType actionType = ActionType.Idle;
    [HideInInspector]
    public Material m_OriginMaterial;
    [HideInInspector]
    public MonsterAbility m_MonsterAbility;
    [HideInInspector]
    public int m_nMonsterId;
    [SerializeField]
    protected Transform m_AttackPivot;

    [HideInInspector]
    public Bounds m_Seebounds;
    protected Bounds m_PlayerBody;

    public SkinnedMeshRenderer m_Skin;
    public MeshRenderer m_Mesh;

    protected MonsterManager m_MonsterManager;

    protected Vector3 m_vecDir;
    protected Vector3 m_vecPos;
    public Transform m_Pivot;

    protected int m_nPlayerHp;
    protected bool m_bDie = true;
    public float m_fSpeed;
    protected float m_fIdleTime = 0;

    public void setMob(MonsterManager manager, int id)
    {
        m_MonsterManager = manager;
        m_nMonsterId = id;
    }

    public override void ChildInit()
    {
        m_Bodybounds.center = m_Pivot.position;

        m_OriginMaterial = m_Skin.material;
        m_Seebounds.center = m_Pivot.position;
        m_Seebounds.extents = new Vector3(2.5f, 2.5f, 2.5f);
        m_MonsterAbility = GetComponent<MonsterAbility>();
        m_MonsterAbility.Init();

        m_MonsterAbility.SetHp(m_MonsterAbility.m_AllHp);
    }

    protected void Update()
    {
        m_Seebounds.center = m_Pivot.position;
        m_Bodybounds.center = m_Pivot.position;

        switch (actionType)
        {
            case ActionType.Idle:
                IdleAction();
                break;
            case ActionType.Move:
                MoveAction();
                break;
            case ActionType.Attack:
                AttackAction();
                break;
            case ActionType.Die:
                DieAction();
                break;
        }
        m_vecPos = m_CollisionHit.seeRange(out m_PlayerBody, this, out m_nPlayerHp);

        if (m_MonsterAbility.m_Hp <= 0)
        {
            actionType = ActionType.Die;
            m_Bodybounds.center = new Vector3(2000, 2000, 2000);
        }
    }

    public override void DieAction()
    {
        if (m_bDie)
        {
            m_Ani.SetTrigger("Die");
            switch (m_MonsterAbility.m_nGate)
            {
                case 0:
                    --m_MonsterManager.m_nMonsterCount_1;
                    break;
                case 1:
                    --m_MonsterManager.m_nMonsterCount_2;
                    break;
            }
            m_MonsterManager.setDie(gameObject, m_nMonsterId);
            m_bDie = false;
        }
    }

    private void DieAni()
    {
        m_Seebounds.extents = new Vector3(2.5f, 2.5f, 2.5f);
        transform.position = m_Bodybounds.center;
        actionType = ActionType.Idle;
        m_MonsterAbility.SetHp(m_MonsterAbility.m_AllHp);
        m_bDie = true;
        gameObject.SetActive(false);
    }

    protected void IdleAction()
    {
        AnimatorStateInfo stateinfo = m_Ani.GetCurrentAnimatorStateInfo(0);

        if (m_Ani.IsInTransition(0))
        {
            return;
        }

        m_Ani.SetInteger("Action", 0);

        m_fIdleTime += Time.deltaTime;

        if (m_fIdleTime >= 1.0f || m_Seebounds.Intersects(m_PlayerBody))
        {
            if (!stateinfo.IsTag("ATTACK"))
            {
                m_fIdleTime = 0;
                actionType = ActionType.Move;

                m_vecDir.x = transform.position.x + Random.Range(-3, 3);
                m_vecDir.y = transform.position.y;
                m_vecDir.z = transform.position.z + Random.Range(-3, 3);
            }
        }
    }

    public override void MoveAction()
    {
        AnimatorStateInfo stateinfo = m_Ani.GetCurrentAnimatorStateInfo(0);

        if (m_Seebounds.Intersects(m_PlayerBody) && m_nPlayerHp > 0)
        {
            m_MoveAction.Move_AI(m_vecPos, m_fSpeed * 2, 1);

            m_Attackbounds.center = m_AttackPivot.position;

            m_CollisionHit.SetShow(this);

            if (m_Attackbounds.Intersects(m_PlayerBody))
            {
                actionType = ActionType.Attack;
                transform.LookAt(m_vecPos);
            }
        }
        else
        {
            if (!stateinfo.IsTag("ATTACK"))
            {
                m_MoveAction.Move_AI(m_vecDir, m_fSpeed, 0);
                m_CollisionHit.ReSetShow(this);
            }
        }


        if (m_MoveAction.m_sMoveState == "Idle")
        {
            actionType = ActionType.Idle;
        }
    }

    public override void AttackAction()
    {
        m_CollisionHit.SetShow(this);
        m_AttackAction.Attack_AI();

        if (!m_Attackbounds.Intersects(m_PlayerBody) || m_nPlayerHp <= 0)
        {
            actionType = ActionType.Idle;
        }
    }

    public void Vibration()
    {
        if (m_Attackbounds.Intersects(m_PlayerBody))
        {
            m_CollisionHit.plaseHit(m_MonsterAbility.m_Attack, this);
        }
    }


}
