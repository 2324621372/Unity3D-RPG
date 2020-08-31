using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    private Vector3 m_vecOriginPos;

    public Transform m_FollwPos;
    private float m_fTime;
    private const float m_fTimeOut = 8.5f;
    private Coroutine m_CoroutineShake;

    void Start()
    {
        m_vecOriginPos = transform.position;
    }

    private void OnEnable()
    {
        m_fTime = 0;
    }

    private void Update()
    {
        m_fTime += Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, m_FollwPos.position, Time.deltaTime * 0.4f);

        if (m_fTime >= m_fTimeOut)
        {
            m_CoroutineShake= StartCoroutine("ShakeCamera");
            StartCoroutine("RemoveCamera");

            m_fTime = 0;
        }
    }

    public IEnumerator ShakeCamera()
    {
        float passTime = 0.0f;

        while (passTime < 2.0f)
        {
            Vector3 shakePos = Random.insideUnitSphere;

            transform.position += shakePos * 0.25f;

            passTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator RemoveCamera()
    {
        yield return new WaitForSeconds(6.0f);
        transform.position = m_vecOriginPos;
        gameObject.SetActive(false);
    }
}
