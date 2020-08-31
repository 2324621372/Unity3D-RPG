using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollisionHit : MonoBehaviour
{
    [SerializeField]
    private Material m_FreezingMaterial;
    [SerializeField]
    private Material m_HitMaterial;

    private List<Vector3> m_LisPos = new List<Vector3>();
    private Dictionary<int, AIController> m_DicAI = new Dictionary<int, AIController>();

    private PlayerController m_PlayerController;
    private MonsterManager m_MonsterManager;

    private ParticleManager m_ParticleManager;
    private FollwCamera m_FollwCamera;
    private UIPlayerState m_PlayerState;
    private AIController m_AI;
    private UIManager m_UIMng;

    private Coroutine m_Coroutine;
    private Coroutine m_PlayerFlush = null;

    private void Start()
    {
        m_PlayerController = FindObjectOfType<PlayerController>();
        m_MonsterManager = FindObjectOfType<MonsterManager>();
        m_ParticleManager = GetComponent<ParticleManager>();
        m_FollwCamera = FindObjectOfType<FollwCamera>();
        m_PlayerState = FindObjectOfType<UIPlayerState>();

        m_UIMng = FindObjectOfType<UIManager>();
    }

    //PlayerController이 요청하여 몬스터 중 자기범위에 있는 몬스터를 찾아줌
    //단수
    public Vector3 collision(Bounds bounds)
    {
        Vector3 vecPos = Vector3.zero;
        for (int i = 0; i < m_MonsterManager.m_DicAI.Count; ++i)
        {
            if (bounds.Intersects(m_MonsterManager.m_DicAI[i].m_Bodybounds))
            {
                m_AI = m_MonsterManager.m_DicAI[i];
                vecPos = m_MonsterManager.m_DicAI[i].transform.position;
                return vecPos;
            }
        }
        return vecPos;
    }
    //복수
    public List<Vector3> ListCollisions(Bounds bounds)
    {
        int _count = 0;
        m_LisPos.Clear();
        m_DicAI.Clear();
        for (int i = 0; i < m_MonsterManager.m_nMonsterIndex; ++i)
        {
            if (m_MonsterManager.m_DicAI.ContainsKey(i))
            {
                if (bounds.Intersects(m_MonsterManager.m_DicAI[i].m_Bodybounds))
                {
                    m_LisPos.Add(m_MonsterManager.m_DicAI[i].transform.position);
                    m_DicAI.Add(_count, m_MonsterManager.m_DicAI[i]);

                    ++_count;
                }
            }
        }
        return m_LisPos;
    }

    //플레이어가 몬스터에게 물리를 적용하는 부분
    //단수
    public void OnePhysic(Vector3 eff, int damage, int kind, bool critical)
    {
        int _sum;
        eff.y = 0;
        m_AI.transform.position += eff;
        StartCoroutine(MonsterFlush(kind, m_AI));
        m_AI.m_MonsterAbility.DecreaseHp(damage, out _sum);

        m_UIMng.m_UIOutput.SetDamageOutput(_sum, Camera.main.WorldToScreenPoint(m_AI.transform.position), critical);
        m_UIMng.m_UIOutput.SetShow(m_AI);

        if (m_AI.m_MonsterAbility.m_Hp <= 0)
        {
            m_PlayerController.m_PlayerAbility.SetLevelUp(m_AI.m_MonsterAbility.m_EXP);

            m_PlayerState.SetEXPUP(m_PlayerController.m_PlayerAbility.m_Level);

            StartCoroutine(m_UIMng.m_UIOutput.TimeActiveFalse(null, 0.0f, m_AI));

            m_UIMng.m_GetQuset.saveQuest(m_AI.m_MonsterAbility.m_nIndex);
        }
        AudioManager.Instance.PlayEffect(12);
    }
    //복수
    public AIController PhysicsEffs(Vector3 eff, int _vecIndex, int _angle, int attack, int hit, int kind, bool critical)
    {
        int _sum;

        eff.y = 0;
        m_ParticleManager.HitParticle(m_DicAI.Count, m_DicAI[_vecIndex].m_Bodybounds.center, _angle, hit);

        m_DicAI[_vecIndex].transform.position += eff;

        m_Coroutine = StartCoroutine(MonsterFlush(kind, m_DicAI[_vecIndex]));

        m_DicAI[_vecIndex].m_MonsterAbility.DecreaseHp(attack, out _sum);

        m_UIMng.m_UIOutput.SetDamageOutput(_sum, Camera.main.WorldToScreenPoint(m_DicAI[_vecIndex].transform.position), critical);

        m_UIMng.m_UIOutput.SetShow(m_DicAI[_vecIndex]);

        AudioManager.Instance.PlayEffect(12);

        if (m_DicAI[_vecIndex].m_MonsterAbility.m_Hp <= 0)
        {
            m_PlayerController.m_PlayerAbility.SetLevelUp(m_DicAI[_vecIndex].m_MonsterAbility.m_EXP);

            m_PlayerState.SetEXPUP(m_PlayerController.m_PlayerAbility.m_Level);

            StartCoroutine(m_UIMng.m_UIOutput.TimeActiveFalse(null, 0.0f, m_DicAI[_vecIndex]));

            m_UIMng.m_GetQuset.saveQuest(m_DicAI[_vecIndex].m_MonsterAbility.m_nIndex);
        }

        return m_DicAI[_vecIndex];
    }

    //몬스터의 범위 중 플레이어가 Seebounds의 범위에 들어왔는지 알려줌 
    public Vector3 seeRange(out Bounds bounds, AIController aIController, out int playerHp)
    {
        bounds = m_PlayerController.m_Bodybounds;
        playerHp = m_PlayerController.m_PlayerAbility.m_Hp;

        return m_PlayerController.transform.position;
    }

    public void SetShow(AIController aIController)
    {
        m_UIMng.m_UIOutput.SetShow(aIController);
    }
    public void ReSetShow(AIController aIController)
    {
        StartCoroutine(m_UIMng.m_UIOutput.TimeActiveFalse(null, 0f, aIController));
    }

    //몬스터가 플레이어 공격 부분
    public void plaseHit(int attack,AIController aIController)
    {
        int _sum;

        if (m_PlayerController.m_PlayerAbility.m_Hp >= 0)
        {
            m_PlayerController.m_PlayerAbility.DecreaseHp(attack, out _sum);
            m_PlayerState.SetHp();
        }
        else
        {
            if (m_PlayerFlush != null)
            {
                StopCoroutine(m_PlayerFlush);
                m_PlayerFlush = null;
            }
        }
    }

    public void HitShakeCamera(float duration, float magnitudePos)
    {
        StartCoroutine(m_FollwCamera.ShakeCamera(duration, magnitudePos));
    }

    public void Freezing(AIController aIController)
    {
        StartCoroutine(Freeze(aIController));
    }

    public IEnumerator Freeze(AIController aiController)
    {
        float _nAniSpeed = 1;

        if (aiController.m_MonsterAbility.m_Hp > 0)
        {
            aiController.enabled = false;
            aiController.m_Ani.speed = 0;

            yield return new WaitForSeconds(4.0f);

            aiController.m_Ani.speed = _nAniSpeed;
            aiController.enabled = true;
        }
    }

    public IEnumerator MonsterFlush(int index, AIController aIController)
    {
        float _time = 0.3f;

        SkinnedMeshRenderer m_MonsterSkin = aIController.m_Skin;
        MeshRenderer m_MonsterMesh = aIController.m_Mesh;

        switch (index)
        {
            case 0:
                if (m_MonsterMesh != null)
                {
                    m_MonsterMesh.material = m_HitMaterial;
                }
                m_MonsterSkin.material = m_HitMaterial;

                aIController.m_Ani.SetFloat("Random", Random.Range(0, 2));

                aIController.m_Ani.SetTrigger("Hit");

                _time = 0.3f;
                break;
            case 1:
                if (m_MonsterMesh != null)
                {
                    m_MonsterMesh.material = m_FreezingMaterial;
                }
                m_MonsterSkin.material = m_FreezingMaterial;

                _time = 4.0f;
                break;
        }

        yield return new WaitForSeconds(_time);

        if (m_MonsterMesh != null)
        {
            m_MonsterMesh.material = aIController.m_OriginMaterial;
        }

        m_MonsterSkin.material = aIController.m_OriginMaterial;

        yield return new WaitForSeconds(0.5f);

        aIController.m_Seebounds.extents = new Vector3(5.0f, 5.0f, 5.0f);
    }
}
