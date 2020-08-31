using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TowHandController : PlayerController
{
    [SerializeField]
    private GameObject m_Bullet;
    private Bullet m_BulletCS;

    public Material m_Reinforcement;
    public SkinnedMeshRenderer m_Skin;
    private Material m_Origin;
    private ParticleSystem m_ParticleSystem;

    protected override void JobInit()
    {
        m_BulletCS = m_Bullet.GetComponent<Bullet>();
        m_BulletCS.ReceiveSet(m_CollisionHit, this);

        Skill[] skill = new Skill[4];

        skill[0] = new Tow_Hand_Skill_1();
        skill[1] = new Tow_Hand_Skill_2();
        skill[2] = new Tow_Hand_Skill_3();
        skill[3] = new Tow_Hand_Skill_4();

        for (int i = 0; i < skill.Length; ++i)
        {
            skill[i].Init();
            m_LisSkill.Add(skill[i]);
        }

        m_Origin = m_Skin.material;
    }

    public override void MoveAction()
    {
        AnimatorStateInfo stateinfo = m_Ani.GetCurrentAnimatorStateInfo(0);

        if (!stateinfo.IsTag("Idle"))
        {
            m_Ani.SetTrigger("Idle");
            m_AttackAction.m_nAttackCount = 0;
        }

        m_WeaponTrailObj.SetActive(false);

        m_MoveAction.Move_Player(m_joyStickMove.Dir);

        dir_normaliezd = m_joyStickMove.Dir.normalized;

        m_Bodybounds.center = m_Pivot.position;
        m_fPosZ = 0;
    }

    public override void AttackAction()
    {
        m_WeaponTrailObj.SetActive(true);

        m_AttackAction.Attack_Player(ref m_joyStickAttack.m_bAttackStat, ref m_joyStickAttack.m_fSKillStat, m_joyStickAttack.m_fSkillIndex);

        if (m_AttackAction.m_sAttackState == "Idle")
        {
            actionType = ActionType.Idle;
            m_AttackAction.m_sAttackState = string.Empty;
        }
    }

    public override void DieAction()
    {
        if (!m_bRevival)
        {
            StartCoroutine("PlayerMoveRevival");
        }
    }

    private void Update()
    {
        m_Pivot.localRotation = transform.localRotation;

        switch (actionType)
        {
            case ActionType.Idle:
                MoveAction();
                break;
            case ActionType.Attack:
                AttackAction();
                break;
            case ActionType.Die:
                DieAction();
                break;
        }

        if (m_joyStickAttack.m_bAttackStat)
        {
            actionType = ActionType.Attack;
        }

        if (m_joyStickRotate.m_vecDirection.x != 0)
        {
            transform.rotation = Quaternion.AngleAxis(m_joyStickRotate.Rotate, Vector3.up);
        }

        m_Bodybounds.center = m_Pivot.transform.position;

        if (m_PlayerAbility.m_Hp < 1)
        {
            actionType = ActionType.Die;
        }
    }

    public override void Target(int angle)
    {
        bool _nCritical = Critical();
        m_Attackbounds.extents = new Vector3(0.8f, 0.4f, 0.8f);

        m_Attackbounds.center = new Vector3(m_AttackPos.transform.position.x, m_AttackPos.transform.position.y, m_AttackPos.transform.position.z);
        List<Vector3> _vecPos = m_CollisionHit.ListCollisions(m_Attackbounds);

        if (_vecPos.Count > 0)
        {
            for (int i = 0; i < _vecPos.Count; ++i)
            {
                if (_nCritical)
                {
                    m_CollisionHit.PhysicsEffs((_vecPos[i] - transform.position).normalized * 0.5f, i, angle, m_PlayerAbility.m_Attack * 2, 0, 0, true);
                }
                else
                {
                    m_CollisionHit.PhysicsEffs((_vecPos[i] - transform.position).normalized * 0.1f, i, angle, m_PlayerAbility.m_Attack, 0, 0, false);
                }
            }
            if (_nCritical)
            {
                m_CollisionHit.HitShakeCamera(0.1f, 0.13f);
            }
            else
            {
                m_CollisionHit.HitShakeCamera(0.1f, 0.04f);
            }
        }
    }

    public override void SkillTarget(int index)
    {
        bool _nCritical = Critical();
        int _nHit = 1;
        switch (index)
        {
            case 2:
                m_Attackbounds.center = new Vector3(m_AttackPos.transform.position.x, m_AttackPos.transform.position.y, m_AttackPos.transform.position.z);
                break;
            case 4:
                _nHit = 0;
                m_Attackbounds.center = new Vector3(m_Pivot.transform.position.x, m_Pivot.transform.position.y, m_Pivot.transform.position.z);
                break;
        }
        m_Attackbounds.extents = new Vector3(1.5f, 1.0f, 1.5f);

        List<Vector3> _vecPos = m_CollisionHit.ListCollisions(m_Attackbounds);

        if (_vecPos.Count > 0)
        {
            for (int i = 0; i < _vecPos.Count; ++i)
            {
                if (_nCritical)
                {
                    m_CollisionHit.PhysicsEffs((_vecPos[i] - transform.position).normalized * 0.5f, i, 0, (m_PlayerAbility.m_Attack + (int)(m_PlayerAbility.m_Attack * (m_LisSkill[index - 1].m_nDamege *0.01f)) * 2), _nHit, 0, true);
                }
                else
                {
                    m_CollisionHit.PhysicsEffs((_vecPos[i] - transform.position).normalized * 0.1f, i, 0, m_PlayerAbility.m_Attack + (int)(m_PlayerAbility.m_Attack * (m_LisSkill[index - 1].m_nDamege * 0.01f)), _nHit, 0, false);
                }
            }
            if (_nCritical)
            {
                m_CollisionHit.HitShakeCamera(0.1f, 0.13f);
            }
            else
            {
                m_CollisionHit.HitShakeCamera(0.1f, 0.01f);
            }
        }
    }

    enum SkillType
    {
        Skill_3 = 1,
        Skill_2 = 0,
        Reinforcement = -1,
    }

    public override void StartSkill(int index)
    {
        if (index != -1)
        {
            ParticleSystem particleSystem = m_ParticleManager.MakeParticle(index);
            switch (index)
            {
                case (int)SkillType.Skill_3:
                    particleSystem.transform.position = m_Pivot.transform.position;
                    particleSystem.transform.localRotation = transform.localRotation;
                    m_ParticleSystem = particleSystem;
                    break;
                case (int)SkillType.Skill_2:
                    particleSystem.transform.position = new Vector3( m_AttackPos.transform.position.x, m_AttackPos.transform.position.y-0.7f, m_AttackPos.transform.position.z);
                    break;
            }
            particleSystem.Play();
        }
        else
        {
            StartCoroutine(Reinforcement(m_LisSkill[0].m_fTime));
        }
    }

    public IEnumerator Reinforcement(float time)
    {
        int _fAttack = m_PlayerAbility.m_Attack;

        m_PlayerAbility.SetAttack(m_PlayerAbility.m_Attack + (int)(m_PlayerAbility.m_Attack * (m_LisSkill[0].m_fAttack / 100)));
        m_Skin.material = m_Reinforcement;
        yield return new WaitForSeconds(time);
        m_PlayerAbility.SetAttack(_fAttack);
        m_Skin.material = m_Origin;
    }

    public override void MakeBullet(int index)
    {
        m_Bullet.transform.position = m_AttackPos.transform.position;
        m_Bullet.SetActive(true);

        m_BulletCS.MakeDamege(m_PlayerAbility.m_Attack + (int)(m_PlayerAbility.m_Attack * (m_LisSkill[3].m_nDamege / 100.0f)), m_ParticleSystem, index, m_PlayerAbility.m_Critical);
    }
}
