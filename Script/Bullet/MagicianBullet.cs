using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianBullet : Bullet
{
    GameObject m_MakeSkillObj;

    public override void Init()
    {
        m_MakeSkillObj = Resources.Load("Bullet/RainStormBullet") as GameObject;
    }

    //파티클의 번호와 스킬의 번호가 달라 다시 넣어야함   
    public override void IndexChange()
    {
        switch (m_nIndex)
        {
            case 0:
                m_nIndex = 2;
                break;
            case 1:
                m_nIndex = 3;
                break;
            case 2:
                m_nIndex = 4;
                break;
        }
    }

    public override void BulletMove()
    {
        bool _bCritical = Critical();
        Vector3 _vector = m_CollisionHit.collision(m_AttackRange);

        switch (m_nIndex)
        {
            case 2:
                m_AttackRange.extents = new Vector3(0.7f, 0.5f, 0.7f);
                m_ParticleSystem.transform.position = transform.position;

                if (_vector != Vector3.zero)
                {
                    gameObject.SetActive(false);
                    m_ParticleSystem.Stop();

                    if (_bCritical)
                    {
                        m_CollisionHit.OnePhysic((_vector - transform.position).normalized * 0.5f, (int)m_fDamage * 2, 0, true);
                        m_fShake = 0.13f;
                    }
                    else
                    {
                        m_CollisionHit.OnePhysic((_vector - transform.position).normalized * 0.5f, (int)m_fDamage, 0, false);
                        m_fShake = 0.088f;
                    }
                    m_CollisionHit.HitShakeCamera(0.1f, m_fShake);
                }

                if (m_fRemove >= m_fLiveTime)
                {
                    m_ParticleSystem.Stop();
                }
                break;
            case 3:
                m_AttackRange.extents = new Vector3(1.5f, 1.0f, 1.5f);

                if (_vector != Vector3.zero)
                {
                    gameObject.SetActive(false);
                    GameObject obj = Instantiate(m_MakeSkillObj,
                        new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z),
                        Quaternion.identity);
                    obj.GetComponent<MakeSkillObj>().Damage(new Vector3(1.8f, 1.0f, 1.8f), m_PlayerChain, m_CollisionHit, m_fDamage, 1.0f, 4.0f, m_nIndex, m_Critical);
                }
                break;
            case 4:
                m_AttackRange.extents = new Vector3(1.5f, 1.0f, 1.5f);
                if (_vector != Vector3.zero)
                {
                    gameObject.SetActive(false);

                    GameObject obj = Instantiate(m_MakeSkillObj,
                        new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z),
                        Quaternion.identity);
                    obj.GetComponent<MakeSkillObj>().Damage(new Vector3(1.5f, 1.0f, 1.5f), m_PlayerChain, m_CollisionHit, m_fDamage, 0, 0, m_nIndex, m_Critical);
                }
                break;
        }

        if (m_fRemove >= m_fLiveTime)
        {
            gameObject.SetActive(false);
            m_fRemove = 0;
        }

    }
}
