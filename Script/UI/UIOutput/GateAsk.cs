using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GateAsk : MonoBehaviour
{
    private GameObject m_Player;

    private Vector3 m_SandPos;
    private NavMeshAgent m_PlayerNav;

    private GameObject m_BossOpeningObj;

    private bool m_bMake = false;

    private MakeBoss m_MakeBoss;
    private PlayerPos m_PlayerPos;

    private GameObject m_BoxObj;
    private GameObject m_UIPlayerState;
    private GameObject m_JoystickObj;

    public void Init(MakeBoss makeBoss, PlayerPos pos,GameObject box)
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_BossOpeningObj = Resources.Load("UI/BossOpening") as GameObject;

        //위치를 아예 이동시켰다.
        m_SandPos = new Vector3(-125.34f, 0, 15.92f);

        m_PlayerNav = m_Player.GetComponent<NavMeshAgent>();     
        m_MakeBoss = makeBoss;
        m_PlayerPos = pos;

        m_BoxObj = box;
        m_UIPlayerState = FindObjectOfType<JoyStickBtn>().gameObject;
        m_JoystickObj = FindObjectOfType<UIPlayerState>().gameObject;

    }

    public void GateMove(int index)
    {
        StartCoroutine(PlayerMove(index));
        AudioManager.Instance.PlayEffect(0);
    }

    public void Cancle()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.PlayEffect(0);
    }

    public IEnumerator PlayerMove(int index)
    {
        if (SceneMng.m_uIScreen != null)
        {
            SceneMng.m_uIScreen.Execute(1.0f);
        }
        yield return new WaitForSeconds(1.0f);

        m_PlayerPos.SavePos(m_Player.transform);
    
        m_PlayerNav.enabled = false;
        m_Player.transform.position = m_SandPos;
        m_PlayerNav.enabled = true;

        gameObject.SetActive(false);

        m_bMake = true;

        m_MakeBoss.MakeBoss(index);

        GameObject opening = Instantiate(m_BossOpeningObj);
        opening.GetComponent<BossOpening>().Init(m_BoxObj, m_UIPlayerState, m_JoystickObj);
        Destroy(opening, 15f);
        AudioManager.Instance.PlayEffect(13);
    }
}
