using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceCharacter : MonoBehaviour
{
    [SerializeField]
    private Animator m_Ani;

    public GameObject m_HandObj;

    public List<ParticleSystem> PS = new List<ParticleSystem>();
    private Vector3[] m_arrPSPos;

    public GameObject[] m_Weapon = new GameObject[4];

    //전사 강화부분
    public Material m_Reinforcement;
    public SkinnedMeshRenderer m_Skin;
    public Material m_Origin;

    public void Start()
    {
        m_arrPSPos = new Vector3[PS.Count];

        for (int i = 0; i < PS.Count; ++i)
        {
            m_arrPSPos[i] = PS[i].transform.position;
        }
    }

    public void InstallInventory(string obj)
    {
        for (int i = 0; i < m_Weapon.Length; ++i)
        {
            if (m_Weapon[i].name.Equals(obj))
            {
                m_Weapon[i].SetActive(true);
            }
            else
            {
                m_Weapon[i].SetActive(false);
            }
        }
    }

    public void PSstart(int index)
    {
        if (m_Origin != null)
        {
            m_Skin.material = m_Origin;
        }

        for (int i = 0; i < PS.Count; ++i)
        {
            PS[i].gameObject.SetActive(false);
        }

        PS[index].gameObject.transform.position = m_arrPSPos[index];
        PS[index].gameObject.SetActive(true);
        PS[index].Play();
    }

    public void ChangeIdle()
    {
        m_Ani.SetTrigger("Idle");

        if (m_Origin != null)
        {
            m_Skin.material = m_Origin;
        }
    }
    public void Reinforcement()
    {
        m_Skin.material = m_Reinforcement;
    }
}
