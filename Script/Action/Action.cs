using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Action : MonoBehaviour
{
    protected NavMeshAgent m_Agent;

    protected Animator m_Ani;
  
    private void Start()
    {       
        m_Ani = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        Init();
    }

    public virtual void Init() { }
}
