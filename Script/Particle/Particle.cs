using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private float m_fRemove;
    private ParticleSystem m_ParticleSystemp;
    private float m_fSpeed=8.5f;

    void Start()
    {
        m_ParticleSystemp = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        m_fRemove += Time.deltaTime;

        transform.Translate(Vector3.forward * Time.deltaTime * m_fSpeed, Space.Self);
    }
}
