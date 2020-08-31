using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSkillObj : MonoBehaviour
{
    private Bounds m_AttackBounds;
    bool m_bHit;
    private int m_nCritical;
    private float m_fShake;

    public bool Critical()
    {
        int _nRandom = Random.Range(0, 100);

        if (_nRandom <= m_nCritical)
        {
            return true;
        }
        return false;
    }

    public void Damage(Vector3 vec, PlayerChain playerChain, CollisionHit collisionHit, float damage, float deley, float stop, int index, int critical)
    {
        bool _bCritical;
        m_AttackBounds.center = transform.position;
        m_AttackBounds.extents = vec;

        playerChain.GetPlayer(index, transform.position);
        m_nCritical = critical;
        _bCritical = Critical();

        switch (index)
        {
            case 3:
                StartCoroutine(DeleyDamege(collisionHit, damage, deley, index, _bCritical));
                break;
            case 4:
                List<Vector3> vector = collisionHit.ListCollisions(m_AttackBounds);

                if (vector.Count > 0)
                {
                    for (int i = 0; i < vector.Count; ++i)
                    {
                        if (_bCritical)
                        {
                            AIController _aIController = collisionHit.PhysicsEffs((vector[i] - transform.position).normalized * 0.5f, i, 0, (int)damage*2, 2, 1, true);
                            m_fShake = 0.13f;
                        }
                        else
                        {
                            AIController _aIController = collisionHit.PhysicsEffs((vector[i] - transform.position).normalized * 0.1f, i, 0, (int)damage, 2, 1, false);
                            m_fShake = 0.09f;
                            collisionHit.Freezing(_aIController);
                        }
                        collisionHit.HitShakeCamera(0.1f, m_fShake);
                    }
                }
                break;
        }
        Destroy(gameObject, stop);
    }

    //지속 공격
    public IEnumerator DeleyDamege(CollisionHit collisionHit, float damage, float deley, int index,bool critical)
    {
        m_bHit = true;

        yield return new WaitForSeconds(deley);

        List<Vector3> vector = collisionHit.ListCollisions(m_AttackBounds);

        if (vector.Count > 0 && m_bHit)
        {
            for (int i = 0; i < vector.Count; ++i)
            {
                if (critical)
                {
                    collisionHit.PhysicsEffs((vector[i] - transform.position).normalized * 0.5f, i, 0, (int)damage*2, 2, 0,true);
                    m_fShake = 0.13f;
                }
                else
                {
                    collisionHit.PhysicsEffs((vector[i] - transform.position).normalized * 0.1f, i, 0, (int)damage, 2, 0, false);
                    m_fShake = 0.09f;
                }
                collisionHit.HitShakeCamera(0.1f, m_fShake);
            }
            m_bHit = false;
        }
        critical = Critical();
        StartCoroutine(DeleyDamege(collisionHit, damage, deley, index, critical));
    }


}
