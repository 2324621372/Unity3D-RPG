using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIController : AIController
{
    public Transform m_WeaponPos;
    public int m_SkillIndex;
    public GameObject m_TombObj;

    public override void AttackAction()
    {
        m_AttackAction.Attack_Boss();

        if (!m_Attackbounds.Intersects(m_PlayerBody))
        {
            actionType = ActionType.Idle;
        }
    }

    public IEnumerator Interval(int skillIndex)
    {
        ParticleSystem _ParticleSystem = m_ParticleManager.SetMonster(skillIndex, m_WeaponPos.transform.position);
        m_Ani.speed = 0.0f;
        yield return new WaitForSeconds(2.0f);
        m_Ani.speed = 1;
        Destroy(_ParticleSystem.gameObject);
    }

    public override void DieAction()
    {
        if (m_bDie)
        {
            m_Ani.SetTrigger("Die");
            m_bDie = false;
        }
    }

    //스킬 사용 시 카메라 흔드는 함수
    public void ShakeCamera()
    {
        m_CollisionHit.HitShakeCamera(0.5f, 0.4f);
    }

    public void MakeDieObj()
    {
        m_bDie = true;
        GameObject _tomb = Instantiate(m_TombObj, transform.position, Quaternion.identity);

        _tomb.transform.LookAt(m_PlayerBody.center);
       
        m_MonsterManager.DestroyBoss(m_nMonsterId);
        Destroy(gameObject);
    }

    public void PlaySounds(int index)
    {
        AudioManager.Instance.PlayEffect(index);
    }

    public void SkillandAttack(int index)
    {
        if (m_Attackbounds.Intersects(m_PlayerBody))
        {
            if (index == 0)
            {
                m_CollisionHit.plaseHit(m_MonsterAbility.m_Attack,this);
            }
            else
            {
                m_CollisionHit.plaseHit(m_MonsterAbility.m_Attack + (int)(m_MonsterAbility.m_Attack * 0.5f), this);
            }
        }
    }
}
