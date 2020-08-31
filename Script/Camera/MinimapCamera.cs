using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private GameObject m_PlayerObj;
    private const int m_nHeight=20;

    void Start()
    {
        m_PlayerObj = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(m_PlayerObj.transform.position.x, m_PlayerObj.transform.position.y + m_nHeight, m_PlayerObj.transform.position.z);
    }

    void Update()
    {
        transform.position = new Vector3(m_PlayerObj.transform.position.x, transform.position.y, m_PlayerObj.transform.position.z);
    }
}
