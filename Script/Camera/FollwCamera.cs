using UnityEngine;
using System.Collections;

public class FollwCamera : MonoBehaviour
{
    private GameObject m_PlayerObj;
    private PlayerController m_PlayerController;
    private CamPos m_CamPos;

    private Transform m_CamPos_1;
    private Transform m_CamPos_2;

    private Vector3 m_OriginPos;

    private Quaternion m_SeeRotate = Quaternion.identity;

    private float m_fDist;
    private const int m_nCamUp = 40;
    private const int m_nCamDown = 23;

    void Start()
    {
        m_PlayerObj = GameObject.FindGameObjectWithTag("Player");
        m_PlayerController = m_PlayerObj.GetComponent<PlayerController>();
        m_CamPos = FindObjectOfType<CamPos>();

        m_CamPos_1 = m_PlayerObj.transform.Find("CameraPos_1");
        m_CamPos_2 = m_PlayerObj.transform.Find("CameraPos_2");

        transform.position = m_CamPos_1.position;
        m_fDist = Vector3.Distance(m_CamPos_2.position, transform.position);
    }


    void LateUpdate()
    {
        if (m_CamPos.m_bRay)
        {
            transform.position = Vector3.Lerp(transform.position, m_CamPos_1.position, Time.deltaTime * 10);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, m_CamPos_2.position, Time.deltaTime * 3);
        }
        if (m_PlayerController.actionType.ToString().Equals("Idle"))
        {
            if (m_PlayerController.dir_normaliezd.y != 0)
            {
                if (m_PlayerController.dir_normaliezd.y < 0)
                {
                    m_SeeRotate.eulerAngles = new Vector3(m_nCamUp, m_PlayerObj.transform.eulerAngles.y, 0);
                }
                else
                {
                    m_SeeRotate.eulerAngles = new Vector3(m_nCamDown, m_PlayerObj.transform.eulerAngles.y, 0);
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, m_SeeRotate, Time.deltaTime * 1f);
            }

            m_OriginPos = transform.position;
        }

        if (transform.eulerAngles.y != m_PlayerObj.transform.eulerAngles.y)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, m_PlayerObj.transform.eulerAngles.y, 0);
        }
    }

    public IEnumerator ShakeCamera(float duration, float magnitudePos)
    {
        float passTime = 0.0f;

        while (passTime < duration)
        {
            Vector3 shakePos = Random.insideUnitSphere;

            transform.position += shakePos * magnitudePos;

            passTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = m_OriginPos;
    }
}

