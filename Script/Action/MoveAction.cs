using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAction : Action
{
    private NavMeshPath m_NavPath;

    [SerializeField]
    private GameObject CamPos;
    private Vector3 OriginCamPos;
    private float m_fSpeed = 5.0f;
    int m_comboCount;

    [HideInInspector]
    public string m_sMoveState = string.Empty;

    public void Move_AI(Vector3 a_dirVce, float a_fSpeed, int tergetIndex)
    {
        m_sMoveState = "Move";

        if (m_Agent.enabled == true)
        {
            m_NavPath = new NavMeshPath();
            m_Agent.CalculatePath(a_dirVce, m_NavPath);
        }

        if (m_NavPath.status == NavMeshPathStatus.PathComplete)
        {
            for (int i = 0; i < m_NavPath.corners.Length - 1; i++)
                Debug.DrawLine(m_NavPath.corners[i], m_NavPath.corners[i + 1], Color.red);

            m_Ani.SetInteger("Action", 1);
            transform.LookAt(a_dirVce);
            transform.position = Vector3.MoveTowards(transform.position, a_dirVce, Time.deltaTime * a_fSpeed);

            if (tergetIndex == 0)
            {
                m_Agent.enabled = true;
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);

                Vector2 TruePos = new Vector2(transform.position.x, transform.position.z);

                Vector2 GoalPos = new Vector2(a_dirVce.x, a_dirVce.z);

               float distance = Vector2.Distance(TruePos, GoalPos);
                if (distance <= 1f && m_Agent.velocity == Vector3.zero)
                {
                    m_sMoveState = "Idle";
                }
            }
            else
            {
                m_Agent.enabled = false;
            }
        }
        else
        {
            m_sMoveState = "Idle";
        }
    }
    public void Move_Player(Vector2 dir)
    {
        if (dir.x != 0 || dir.y != 0)
        {
            if (dir.x > 0.9f || dir.x < -0.9f)
            {
                m_fSpeed = 3.0f;
            }
            else
            {
                m_fSpeed = 5.0f;
            }
            Vector3 worldDir = new Vector3(dir.x, 0, dir.y) * Time.deltaTime * m_fSpeed;

            transform.Translate(worldDir, Space.Self);

            m_Ani.SetFloat("MoveX", dir.x);
            m_Ani.SetFloat("MoveZ", dir.y);
        }
        else
        {
            m_Ani.SetFloat("MoveX", 0);
            m_Ani.SetFloat("MoveZ", 0);
        }
    }
}
