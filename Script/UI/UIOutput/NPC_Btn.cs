using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Btn : MonoBehaviour
{
    public int m_nIndex;

    private List<Transform> m_Lischild = new List<Transform>();

    private UIOutput uIOutput;
    public GameObject m_BtnObj;
    public GameObject m_TextObj;

    public void Init(UIOutput Output,Transform obj)
    {
        uIOutput = Output;
        for (int i = 0; i < transform.childCount; ++i)
        {
            m_Lischild.Add(transform.GetChild(i));
        }
                
        m_Lischild.Add(obj);
    }

    public void setMenu()
    {
        for (int i = 1; i < transform.childCount-1; ++i)
        {
            m_Lischild[i].gameObject.SetActive(true);
        }
        AudioManager.Instance.PlayEffect(0);
    }

    public void setCancle()
    { 
        for (int i = 1; i < transform.childCount - 1; ++i)
        {
            m_Lischild[i].gameObject.SetActive(false);
        }
        AudioManager.Instance.PlayEffect(0);
    }

    public void SetShow()
    {
        m_Lischild[m_Lischild.Count-1].gameObject.SetActive(true);
        for (int i = 0; i < transform.childCount; ++i)
        {
            m_Lischild[i].gameObject.SetActive(false);
        }
        AudioManager.Instance.PlayEffect(0);
    }

    public void SetWindow()
    {
        m_Lischild[m_Lischild.Count - 1].gameObject.SetActive(true);
        AudioManager.Instance.PlayEffect(0);
    }
}
