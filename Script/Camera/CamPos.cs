using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPos : MonoBehaviour
{
    private Transform m_PlayerPos;
    private float m_fDist;
    public bool m_bRay;

    private void Start()
    {
        m_PlayerPos = GameObject.FindGameObjectWithTag ("Player").transform;
        m_fDist = Vector3.Distance(m_PlayerPos.position, transform.position);
    }

    void Update()
    {
        RaycastHit _Hitinfo;
        Physics.Raycast(transform.position, (m_PlayerPos.transform.position - transform.position).normalized, out _Hitinfo, m_fDist, 1 << LayerMask.NameToLayer("Obstacle"));

        if (_Hitinfo.point == Vector3.zero)
        {
            m_bRay=true;
        }
        else
        {
            m_bRay = false;
        }
    }
}
