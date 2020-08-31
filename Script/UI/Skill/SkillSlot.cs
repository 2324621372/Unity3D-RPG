using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject NextBtn;
    private bool m_bPart = true;

    public Dictionary<GameObject, int> m_DicIndex = new Dictionary<GameObject, int>();
    public List<GameObject> m_LisSlot = new List<GameObject>();

    private Vector2[] m_arrPos;

    private void Start()
    {
        m_arrPos = new Vector2[m_LisSlot.Count];
        for (int i = 0; i < m_LisSlot.Count; ++i)
        {
            m_arrPos[i]= m_LisSlot[i].transform.position;
        }
    }

    public void OnPart()
    {
        if (m_bPart)
        {       
            m_bPart = false;
            for (int i = 0; i < m_LisSlot.Count; ++i)
            {
                if (i < 3)
                {
                    m_LisSlot[i].transform.position = m_arrPos[i + 3];
                }
                else
                {
                    m_LisSlot[i].transform.position = m_arrPos[i - 3];
                }
            }
        }
        else
        {
            m_bPart = true;

            for (int i = 0; i < m_LisSlot.Count; ++i)
            {
                m_LisSlot[i].transform.position = m_arrPos[i];
            }
        }
    }
}
