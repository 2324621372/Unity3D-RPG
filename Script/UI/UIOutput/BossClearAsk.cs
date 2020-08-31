using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface PlayerPos
{
    void SavePos(Transform pos);
}

public class BossClearAsk : MonoBehaviour, PlayerPos
{
    private GameObject m_PlayerObj;
    private Vector3 m_SandPos;
    private NavMeshAgent m_PlayerNav;

    public PlayerPos Init()
    {
        m_PlayerObj = GameObject.FindGameObjectWithTag("Player");
        m_PlayerNav = m_PlayerObj.GetComponent<NavMeshAgent>();
        return this;
    }

    public void BackMove()
    {
        StartCoroutine("PlayerMove");
    }

    public void Cancle()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.PlayEffect(0);
    }

    public void SavePos(Transform pos)
    {
        m_SandPos = pos.position;
    }

    public IEnumerator PlayerMove()
    {
        if (SceneMng.m_uIScreen != null)
        {
            SceneMng.m_uIScreen.Execute(2.0f);
        }
        GameObject _tomb = GameObject.FindGameObjectWithTag("Tomb");
        _tomb.GetComponent<NPCRange>().SetRemove();

        yield return new WaitForSeconds(1.0f);

        m_PlayerNav.enabled = false;
        m_PlayerObj.transform.position = m_SandPos;
        m_PlayerNav.enabled = true;

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
