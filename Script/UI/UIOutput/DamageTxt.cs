using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageTxt : MonoBehaviour
{
    private RectTransform m_DamagePos;
    private Vector3 m_vecSandPos;
    private Vector3 m_vecSandScale;
    private Vector3 m_vecBaseScale;

    private TextMeshProUGUI m_DamageTxt;

    private int m_nShake;
    public void Init()
    {
        m_DamagePos = GetComponent<RectTransform>();
        m_DamageTxt = GetComponent<TextMeshProUGUI>();

        m_vecSandPos = new Vector3(m_DamagePos.position.x, m_DamagePos.position.y + 110, m_DamagePos.position.z);

        m_vecSandScale = new Vector3(4.0f, 4.0f, 4.0f);
        m_vecBaseScale = m_DamagePos.localScale;

        m_DamagePos.localScale = m_vecSandScale;
    }

    private void Update()
    {
        m_DamagePos.position = Vector3.Lerp(m_DamagePos.position, m_vecSandPos, Time.deltaTime * 5);
        m_DamagePos.localScale = Vector3.Lerp(m_DamagePos.localScale, m_vecBaseScale, Time.deltaTime * 6);

        //일반은 6 크리가 생기면 9
        transform.position = transform.position + (Random.insideUnitSphere * 9);
    }

    public void SetTxt(string damage)
    {
        m_DamagePos.localScale = m_vecSandScale;
        m_vecSandPos = new Vector3(m_DamagePos.position.x, m_DamagePos.position.y + 110, m_DamagePos.position.z);

        m_DamageTxt.text = damage;
    }
}
