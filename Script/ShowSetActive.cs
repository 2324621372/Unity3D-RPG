using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSetActive : MonoBehaviour
{
    public Bounds ShowRenderingBounds;
    public GameObject[] m_arrRendering;
    private PlayerController m_PlayerController;

    private bool m_bSwich = true;

    public int a;
    private void Start()
    {
        ShowRenderingBounds.center = transform.position;
        m_PlayerController = FindObjectOfType <PlayerController>();
     }

    private void Update()
    {
        if (ShowRenderingBounds.Intersects(m_PlayerController.m_Bodybounds))
        {
            for (int i=0;i< m_arrRendering.Length;++i)
            {
                m_arrRendering[i].SetActive(true);
            }
            m_bSwich = true;
        }
        else
        {
            if (m_bSwich)
            {
                for (int i = 0; i < m_arrRendering.Length; ++i)
                {
                    m_arrRendering[i].SetActive(false);
                }
                m_bSwich = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        switch (a)
        {
            case 0:
                Gizmos.color = Color.red;
                break;
            case 1:
                Gizmos.color = Color.blue;
                break;
            case 2:
                Gizmos.color = Color.yellow;
                break;
        }
        //구체 모양의 기즈모생성
        Gizmos.DrawCube(ShowRenderingBounds.center, ShowRenderingBounds.size);
    }
}
