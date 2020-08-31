using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRange : MonoBehaviour
{
    public int m_nNumber;
    public float m_fShowPosY;

    private Bounds m_NPCBounds;
    private PlayerController m_PlayerController;
    private UIOutput m_UIOutput;
    private Vector3 m_VecPos;

    void Start()
    {
        m_NPCBounds.center = transform.position;
        m_NPCBounds.extents = new Vector3(2.5f, 1.0f, 2.5f);

        m_VecPos = new Vector3(transform.position.x, transform.position.y + m_fShowPosY, transform.position.z);
        m_PlayerController = FindObjectOfType<PlayerController>();
        m_UIOutput = UIAdd.Get<UIOutput>(UIType.UIOutput);
    }

    void Update()
    {
        if (m_NPCBounds.Intersects(m_PlayerController.m_Bodybounds))
        {
            if (!m_UIOutput.m_DicNPCObj.ContainsKey(gameObject))
            {
                m_UIOutput.setNPC(gameObject, m_nNumber, m_VecPos);
            }
            else
            {
                m_UIOutput.m_DicNPCObj[gameObject].SetActive(true);
                m_UIOutput.FixPos(gameObject, m_VecPos);
            }
        }
        else
        {
            m_UIOutput.removeNPC(gameObject);
        }
    }

    public void SetRemove()
    {
        m_UIOutput.SetNPCRemove(gameObject);
        Destroy(gameObject);
    }
}
