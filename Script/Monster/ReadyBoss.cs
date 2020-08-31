using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyBoss : MonoBehaviour
{
    public GameObject m_WeaponObj;
    public Transform m_RHandObj;

    public void ChangeWeaponPos()
    {
        m_WeaponObj.transform.SetParent(m_RHandObj);
    }

    public void MoveStart()
    {
        GetComponent<BossAIController>().enabled = true;
    }
}
