using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOpening : MonoBehaviour
{
    public Transform m_TopPos;
    public Transform m_BottomPos;

    private Vector3 m_VecTop;
    private Vector3 m_VecBottom;

    public Transform m_MoveTopPos;
    public Transform m_MoveBottomPos;

    private float m_fTime;

    private GameObject m_UIPlayerState;
    private GameObject m_JoystickObj;
    private GameObject m_BoxObj;

    public void Init(GameObject box, GameObject joystick, GameObject uiplayer)
    {
        m_VecTop = m_TopPos.position;
        m_VecBottom = m_BottomPos.position;

        m_BoxObj = box;
        m_JoystickObj = joystick;
        m_UIPlayerState = uiplayer;

        m_UIPlayerState.SetActive(false);
        m_JoystickObj.SetActive(false);
        m_BoxObj.SetActive(false);
    }

    void Update()
    {
        m_fTime += Time.deltaTime;
        if (m_fTime < 11f)
        {
            m_TopPos.position = Vector3.Lerp(m_TopPos.transform.position, m_MoveTopPos.transform.position, Time.deltaTime * 1.5f);
            m_BottomPos.position = Vector3.Lerp(m_BottomPos.transform.position, m_MoveBottomPos.transform.position, Time.deltaTime * 1.5f);
        }
        else
        {
            m_TopPos.position = Vector3.Lerp(m_TopPos.transform.position, m_VecTop, Time.deltaTime * 1.5f);
            m_BottomPos.position = Vector3.Lerp(m_BottomPos.transform.position, m_VecBottom, Time.deltaTime * 1.5f);
        }

        if (m_fTime > 13f)
        {
            m_UIPlayerState.SetActive(true);
            m_JoystickObj.SetActive(true);
            m_BoxObj.SetActive(true);
        }
    }
}
