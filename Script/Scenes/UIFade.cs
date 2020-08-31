using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
   private float m_fTime;
    public Image m_FadeImg;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        SceneMng.m_uIScreen = this;
    }

    public void Execute(float time)
    {
        StartCoroutine(FadeIn(time));
    }

    IEnumerator FadeIn(float time)
    {
        while (true)
        {
            m_fTime += Time.deltaTime;

            m_FadeImg.color = new Color(m_FadeImg.color.r, m_FadeImg.color.g, m_FadeImg.color.g,m_fTime);
            yield return null;
    
            if (m_fTime>time)
            {
                StartCoroutine("FadeOut");
                break;
            }
        }
    }

    IEnumerator FadeOut()
    {
        while (true)
        {
            m_fTime -= Time.deltaTime;

            m_FadeImg.color = new Color(m_FadeImg.color.r, m_FadeImg.color.g, m_FadeImg.color.g, m_fTime);
            yield return null;

            if (m_fTime <= 0)
            {
                m_fTime = 0;
                break;
            }
        }
    }
}
