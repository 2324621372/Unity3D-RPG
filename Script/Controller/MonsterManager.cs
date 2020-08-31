using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MakeBoss
{
    void MakeBoss(int index);
}

public class MonsterManager : MonoBehaviour, MakeBoss
{
    [SerializeField]
    private GameObject m_LiveObj_1;
    [SerializeField]
    private GameObject m_LiveObj_2;
    private GameObject[] m_RespawnObj;

    public Dictionary<int, AIController> m_DicAI = new Dictionary<int, AIController>();
    public Dictionary<int, int> m_DicCount = new Dictionary<int, int>();
    private Dictionary<int, Vector3> m_VecBossPos = new Dictionary<int, Vector3>();

    private List<GameObject> m_LisMonsterLoad = new List<GameObject>();
    private List<GameObject> m_LisBossLoad = new List<GameObject>();

    public int m_nMonsterCount_1 = 0;
    public int m_nMonsterCount_2 = 0;

    public int m_nMonsterIndex = 0;

    private float m_fRespawnTime_1;
    private float m_fRespawnTime_2;

    public GameObject m_DieObj;
    public GameObject m_BossCameraObj;

    void Start()
    {
        m_LisMonsterLoad.Add(Resources.Load("Monster/Skeleton") as GameObject);
        m_LisMonsterLoad.Add(Resources.Load("Monster/Tormented_Soul") as GameObject);
        m_LisMonsterLoad.Add(Resources.Load("Monster/Goblin_Shaman") as GameObject);

        m_LisBossLoad.Add(Resources.Load("Monster/GobilnKing") as GameObject);
        m_VecBossPos.Add(0, new Vector3(-164, 0.07f, 19.02f));

        m_RespawnObj = GameObject.FindGameObjectsWithTag("Respawn");
        for (int i = 0; i < 4; ++i)
        {
            RespawnMake_1();
            RespawnMake_2();
        }
    }

    void Update()
    {
        if (m_nMonsterCount_1 < 6)
        {
            m_fRespawnTime_1 += Time.deltaTime;
            if (m_fRespawnTime_1 > 4.0f)
            {
                RespawnMake_1();
                m_fRespawnTime_1 = 0;
            }
        }
        if (m_nMonsterCount_2 < 6)
        {
            m_fRespawnTime_2 += Time.deltaTime;
            if (m_fRespawnTime_2 > 5.0f)
            {
                RespawnMake_2();
                m_fRespawnTime_2 = 0;
            }
        }
    }
    //오브젝트 풀링으로 몬스터를 생성시킴
    private void RespawnMake_1()
    {
        //리스폰 구역을 랜덤하게 실행시킨다.
        int _monPos = Random.Range(0, 7);
        //랜덤으로 몬스터를 생성하기 위해서
        int _randomLoad = Random.RandomRange(0, 2);

        if (m_DieObj.transform.Find(m_LisMonsterLoad[_randomLoad].name))
        {
            GameObject monster = m_DieObj.transform.Find(m_LisMonsterLoad[_randomLoad].name).gameObject;

            monster.transform.parent = m_LiveObj_1.transform;
            monster.transform.position = m_RespawnObj[_monPos].transform.position;

            ++m_nMonsterCount_1;

            monster.SetActive(true);
        }
        else
        {
            GameObject monster = Instantiate(m_LisMonsterLoad[_randomLoad], m_RespawnObj[_monPos].transform.position, Quaternion.identity, m_LiveObj_1.transform);
            monster.name = m_LisMonsterLoad[_randomLoad].name;

            AIController aiController = monster.GetComponent<AIController>();
            aiController.Init();
            m_DicAI.Add(m_nMonsterIndex, aiController);

            aiController.setMob(this, m_nMonsterIndex);

            ++m_nMonsterCount_1;
            ++m_nMonsterIndex;
        }

    }
    private void RespawnMake_2()
    {
        int _monPos = Random.Range(7, 12);
        int _randomLoad = Random.RandomRange(2, 3);

        if (m_DieObj.transform.Find(m_LisMonsterLoad[_randomLoad].name))
        {
            GameObject monster = m_DieObj.transform.Find(m_LisMonsterLoad[_randomLoad].name).gameObject;

            monster.transform.parent = m_LiveObj_2.transform;
            monster.transform.position = m_RespawnObj[_monPos].transform.position;

            ++m_nMonsterCount_2;

            monster.SetActive(true);
        }
        else
        {
            GameObject monster = Instantiate(m_LisMonsterLoad[_randomLoad], m_RespawnObj[_monPos].transform.position, Quaternion.identity, m_LiveObj_2.transform);
            monster.name = m_LisMonsterLoad[_randomLoad].name;

            AIController aiController = monster.GetComponent<AIController>();
            m_DicAI.Add(m_nMonsterIndex, aiController);
            aiController.Init();
            aiController.setMob(this, m_nMonsterIndex);

            ++m_nMonsterCount_2;
            ++m_nMonsterIndex;
        }

    }

    public void setDie(GameObject monster, int id)
    {
        monster.transform.parent = m_DieObj.transform;
    }

    public void MakeBoss(int index)
    {
        GameObject boss = Instantiate(m_LisBossLoad[index]);
        boss.transform.position = m_VecBossPos[index];

        m_BossCameraObj.SetActive(true);

        AIController aiController = boss.GetComponent<BossAIController>();
        aiController.Init();
        m_DicAI.Add(m_nMonsterIndex, aiController);

        aiController.setMob(this, m_nMonsterIndex);
        aiController.m_MonsterAbility.SetHp(aiController.m_MonsterAbility.m_AllHp);

        ++m_nMonsterIndex;
    }

    //보스는 계속 생성되는 것이 아니라 던전에 입장해서 만들어지기 때문에 풀링이 아닌 Destroy하였음
    public void DestroyBoss(int index)
    {
        m_DicAI.Remove(index);
    }
}
