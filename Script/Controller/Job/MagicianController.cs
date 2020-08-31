using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianController : PlayerController
{
    [SerializeField]
    private GameObject[] m_Bullet;
    private Bullet[] m_BulletCS;

    private int m_nSkillIndex;

    private ParticleSystem m_ParticleSystem;

    private ParticleSystem m_ParticleShield;

    protected override void JobInit()
    {
        m_BulletCS = new Bullet[3];
        for (int i = 0; i < 3; ++i)
        {
            m_BulletCS[i] = m_Bullet[i].GetComponent<Bullet>();
            m_BulletCS[i].ReceiveSet(m_CollisionHit, this);
        }
        Skill[] skill = new Skill[4];

        skill[0] = new Magician_Skill_1();
        skill[1] = new Magician_Skill_2();
        skill[2] = new Magician_Skill_3();
        skill[3] = new Magician_Skill_4();

        for (int i = 0; i < skill.Length; ++i)
        {
            skill[i].Init();
            m_LisSkill.Add(skill[i]);
        }
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

    //기본 공격
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
                    m_CollisionHit.PhysicsEffs((_vecPos[i] - transform.position).normalized * 0.5f, i, angle, m_PlayerAbility.m_Attack * 2, 1, 0, true);
                }
                else
                {
                    m_CollisionHit.PhysicsEffs((_vecPos[i] - transform.position).normalized * 0.1f, i, angle, m_PlayerAbility.m_Attack, 1, 0, false);
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

    enum SkillType
    {
        FireBall = 2,
        RainStorm = 3,
        IceAge = 4,
        Shield = 5,
    }

    public override void StartSkill(int index)
    {
        ParticleSystem _ParticleSystem = m_ParticleManager.MakeParticle(index);

        switch (index)
        {
            case (int)SkillType.FireBall:
                _ParticleSystem.transform.position = m_Pivot.transform.position;
                _ParticleSystem.transform.localRotation = transform.localRotation;
                m_ParticleSystem = _ParticleSystem;
                break;
            case (int)SkillType.RainStorm:
                m_ParticleSystem = _ParticleSystem;
                break;
            case (int)SkillType.IceAge:
                m_ParticleSystem = _ParticleSystem;
                break;
            case (int)SkillType.Shield:
                _ParticleSystem.transform.position = m_Pivot.transform.position;
                _ParticleSystem.transform.SetParent(transform);
                m_ParticleShield = _ParticleSystem;
                StartCoroutine(ShieldContinue(m_LisSkill[3]));
                break;
        }
        _ParticleSystem.Play();
    }

    public override void MakeBullet(int index)
    {
        //파티클의 번호와 스킬의 번호가 달라 다시 입력해야함
        switch (index)
        {
            case 2:
                index = 0;
                break;
            case 3:
                index = 1;
                break;
            case 4:
                index = 2;
                break;
        }
        m_Bullet[index].transform.position = m_AttackPos.transform.position;
        m_Bullet[index].SetActive(true);
        m_BulletCS[index].MakeDamege(m_PlayerAbility.m_Attack + (int)(m_PlayerAbility.m_Attack * (m_LisSkill[index].m_nDamege *0.01f)), m_ParticleSystem, index, m_PlayerAbility.m_Critical);
    }

    public IEnumerator ShieldContinue(Skill skill)
    {
        int _nDefense = m_PlayerAbility.m_Defense;

        m_PlayerAbility.SetDefense(m_PlayerAbility.m_Defense + (int)(m_PlayerAbility.m_Defense * (skill.m_fDefense *0.01f)));

        yield return new WaitForSeconds(skill.m_fTime);

        m_ParticleShield.Stop();
        m_PlayerAbility.SetDefense(_nDefense);
    }

    public override void GetPlayer(int index, Vector3 skillPos)
    {
        float _nTime = 0;

        StartSkill(index);
        m_ParticleSystem.transform.position = skillPos;
        switch (index)
        {
            case 3:
                _nTime = 4.0f;
                break;
            case 4:
                _nTime = 1.5f;
                break;
        }
        StartCoroutine(StopSkill(_nTime));
    }

    public IEnumerator StopSkill(float time)
    {
        yield return new WaitForSeconds(time);
        m_ParticleSystem.Stop();
    }

    //void OnDrawGizmos()
    //{
    //    //기즈모색 설정
    //    Gizmos.color = Color.red;
    //    //구체 모양의 기즈모생성
    //    Gizmos.DrawCube(m_Attackbounds.center, m_Attackbounds.size);
    //}
}
