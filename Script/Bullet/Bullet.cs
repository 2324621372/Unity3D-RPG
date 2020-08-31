using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected CollisionHit m_CollisionHit;
    protected Bounds m_AttackRange;

    protected float m_fDamage;

    protected bool m_bHit;
    protected float m_fRemove;

    protected ParticleSystem m_ParticleSystem;

    protected int m_nIndex;
    protected PlayerChain m_PlayerChain;
    protected int m_Critical;

    protected const float m_fSpeed = 8.5f;
    protected const float m_fLiveTime = 0.6f;
    protected float m_fShake;

    public bool Critical()
    {
        int _nRandom = Random.Range(0, 100);

        if (_nRandom <=m_Critical)
        {
            return true;
        }
        return false;
    }

    public void ReceiveSet(CollisionHit a_collisionHit,PlayerChain playerChain)
    {
        m_CollisionHit = a_collisionHit;

        m_AttackRange.extents = new Vector3(0.7f, 0.5f, 0.7f);
        m_PlayerChain = playerChain;
        Init();
    }
   
    public void MakeDamege(float damege, ParticleSystem particleSystem, int index,int critical)
    {
        m_fDamage = damege;
        m_bHit = true;
        m_nIndex = index;
        if (particleSystem != null)
        {
            m_ParticleSystem = particleSystem;
        }
        IndexChange();
        m_Critical = critical;
    }

    private void Update()
    {
        m_fRemove += Time.deltaTime;
        m_AttackRange.center = transform.position;

        transform.Translate(Vector3.forward * Time.deltaTime * m_fSpeed, Space.Self);

        BulletMove();
    }

    public virtual void BulletMove()
    {
        bool _bCritical = Critical();

        if (m_fRemove >= m_fLiveTime)
        {
            gameObject.SetActive(false);
            m_fRemove = 0;
        }

        List<Vector3> _vector = m_CollisionHit.ListCollisions(m_AttackRange);

        if (_vector.Count > 0 && m_bHit)
        {
            for (int i = 0; i < _vector.Count; ++i)
            {
                if(_bCritical)
                {
                    m_CollisionHit.PhysicsEffs((_vector[i] - transform.position).normalized * 0.5f, i, 0, (int)m_fDamage*2, 1, 0,true);
                }
                else
                {
                    m_CollisionHit.PhysicsEffs((_vector[i] - transform.position).normalized * 0.1f, i, 0, (int)m_fDamage, 1, 0,false);
                }
            }
            if (_bCritical)
            {
                m_fShake = 0.13f;
            }
            else
            {
                m_fShake = 0.09f;
            }
            m_CollisionHit.HitShakeCamera(0.1f, m_fShake);
            m_bHit = false;
        }

    }
    public virtual void Init() { }
    public virtual void IndexChange() { }
}
