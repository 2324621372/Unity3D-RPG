using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_HitBox;
    [SerializeField]
    private GameObject m_SkillBox;
    [SerializeField]
    private GameObject m_OtherBox;

    public List<ParticleSystem> m_LisSkill = new List<ParticleSystem>();
    public List<ParticleSystem> m_LisHit = new List<ParticleSystem>();
    public List<ParticleSystem> m_LisOther = new List<ParticleSystem>();
    public List<ParticleSystem> m_LisMonster = new List<ParticleSystem>();

    public Dictionary<int, ParticleSystem> m_DicParticBox = new Dictionary<int, ParticleSystem>();
    public Dictionary<int, ParticleSystem> m_DicOtherBox = new Dictionary<int, ParticleSystem>();
    private Dictionary<int, int> m_DicHitCount = new Dictionary<int, int>();

    private List<List<ParticleSystem>> m_LisChainHit = new List<List<ParticleSystem>>();
    private List<ParticleSystem> m_LisPlayHit_1 = new List<ParticleSystem>();
    private List<ParticleSystem> m_LisPlayHit_2 = new List<ParticleSystem>();
    private List<ParticleSystem> m_LisPlayHit_3 = new List<ParticleSystem>();

    private void Start()
    {
        int _nIndex = FindObjectOfType<PlayerAbility>().m_nIndex;
        m_LisChainHit.Add(m_LisPlayHit_1);
        m_LisChainHit.Add(m_LisPlayHit_2);
        m_LisChainHit.Add(m_LisPlayHit_3);
        switch (_nIndex)
        {
            case 1:
                m_DicHitCount.Add(0, 0);
                m_DicHitCount.Add(1, 0);

                for (int i = 0; i < 5; ++i)
                {
                    for (int j = 0; j < 2; ++j)
                    {
                        ParticleSystem hit = Instantiate(m_LisHit[j], m_HitBox.transform);

                        ++m_DicHitCount[j];
                        m_LisChainHit[j].Add(hit);
                        hit.Stop();

                    }
                }
                for (int i = 0; i < 2; ++i)
                {
                    ParticleSystem skill = Instantiate(m_LisSkill[i], m_SkillBox.transform);
                   skill.Stop();
                    m_DicParticBox.Add(i, skill);
                }
                break;

            case 2:
                m_DicHitCount.Add(1, 0);
                m_DicHitCount.Add(2, 0);

                for (int i = 0; i < 5; ++i)
                {
                    for (int j = 1; j < 3; ++j)
                    {
                        ParticleSystem hit = Instantiate(m_LisHit[j], m_HitBox.transform);

                        ++m_DicHitCount[j];

                        m_LisChainHit[j].Add(hit);
                       hit.Stop();
                    }
                }
                for (int i = 2; i < 6; ++i)
                {
                    ParticleSystem skill = Instantiate(m_LisSkill[i], m_SkillBox.transform);
                   skill.Stop();
                    m_DicParticBox.Add(i, skill);
                }
                break;
        }
        for (int i = 0; i < m_LisOther.Count; ++i)
        {
            ParticleSystem other = Instantiate(m_LisOther[i], m_OtherBox.transform);
            m_DicOtherBox.Add(i, other);
            other.Stop();
        }
    }

    public void HitParticle(int _count, Vector3 MobPos, int _angle, int hit)
    {
        if (m_DicHitCount[hit] < _count)
        {
            for (int i = 0; i < _count - m_HitBox.transform.childCount; ++i)
            {
                ParticleSystem hitObj = Instantiate(m_LisHit[hit], m_HitBox.transform);
                m_LisChainHit[hit].Add(hitObj);
                hitObj.Stop();
            }
        }

        for (int i = 0; i < m_DicHitCount[hit]; ++i)
        {
            if (!m_LisChainHit[hit][i].isPlaying)
            {
                var main = m_LisChainHit[hit][i].main;
                main.startRotation = _angle * Mathf.Deg2Rad;
                m_LisChainHit[hit][i].transform.position = MobPos;
                m_LisChainHit[hit][i].Play();
                break;
            }
        }
    }

    public ParticleSystem MakeParticle(int index)
    {
        if (m_DicParticBox.ContainsKey(index))
        {
            m_DicParticBox[index].Stop();
        }
        else
        {
            ParticleSystem particleSystem = Instantiate(m_LisSkill[index], m_SkillBox.transform);
            particleSystem.name = m_LisSkill[index].name;
            particleSystem.Stop();
            m_DicParticBox.Add(index, particleSystem);
        }
        return m_DicParticBox[index];
    }

    public void SetEXP(Vector3 pos)
    {
        m_DicOtherBox[0].transform.position = pos;
        m_DicOtherBox[0].Play();
    }

    public ParticleSystem SetMonster(int index, Vector3 pos)
    {
        ParticleSystem particleSystem = Instantiate(m_LisMonster[index], m_SkillBox.transform);
        particleSystem.name = m_LisMonster[index].name;

        particleSystem.transform.position = pos;
        particleSystem.Play();
        return particleSystem;
    }
}
