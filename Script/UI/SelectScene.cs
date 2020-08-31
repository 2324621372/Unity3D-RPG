using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectScene : MonoBehaviour
{
    private Animator m_SwordAni;
    private Animator m_MagicianAni;

    public Text m_Content;
    public Text m_JobVaule;

    public static int m_nIndex=2;

    public GameObject m_TowHandSword;
    public GameObject m_Magician;

    public static UIFade m_uIScreen;

    private bool m_bOneClick=false;
    private void Start()
    {
        m_SwordAni = m_TowHandSword.GetComponent<Animator>();
        m_MagicianAni = m_Magician.GetComponent<Animator>();
        m_Content.text = " ";
        m_JobVaule.text = " ";

        if (m_uIScreen == null)
        {
            GameObject uiScreenObj = Instantiate(Resources.Load("UI/UIFade")) as GameObject;
            m_uIScreen = uiScreenObj.GetComponent<UIFade>();
            m_uIScreen.Init();
        }

        AudioManager.Instance.WorldSound(0);
    }

    public void Select(int index)
    {
        m_nIndex = index;

        switch (index)
        {
            case 1:
                m_SwordAni.SetTrigger("Activate");
                m_JobVaule.text = "전사";
                m_Content.text = "강력한 공격력과 방어력을 지닌 전사이며 \n양손검을 사용합니다.";
                AudioManager.Instance.PlayEffect(1);
                break;
            case 2:
                m_MagicianAni.SetTrigger("Activate");
                m_JobVaule.text = "마법사";
                m_Content.text = "공격력이 낮긴 하지만 강력한 마법을 지닌 \n마법사이며 지팡이를 사용합니다.";
                AudioManager.Instance.PlayEffect(2);
                break;
        }
    }

    public void Yes()
    {
        if (!m_bOneClick)
        {
            if (m_nIndex != 0)
            {
                if (SceneMng.m_uIScreen != null)
                {
                    SceneMng.m_uIScreen.Execute(2.0f);
                }
                SceneMng.Instance.NextScene(SceneChange.MainScene, 1.0f);
                AudioManager.Instance.PlayEffect(0);
                AudioManager.Instance.AudioRemove(m_nIndex);
            }
            m_bOneClick = true;
        }
    }
}
