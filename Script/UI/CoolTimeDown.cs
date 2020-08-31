using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeDown : MonoBehaviour
{
    private Image m_Image;
    private float m_fTime;
    private float m_fEndTime;

    public void Init(float time)
    {
        m_Image = GetComponent<Image>();
        m_fEndTime = time;
        m_fTime = 0;
        m_Image.fillAmount = 1;
    }

    private void OnDisable()
    {
        m_fTime = 0;
        m_Image.fillAmount = 1;
    }

    void Update()
    {
        m_fTime += Time.deltaTime;
        m_Image.fillAmount = 1 - m_fTime / m_fEndTime;
    }
}
