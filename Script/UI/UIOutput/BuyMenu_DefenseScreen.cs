using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyMenu_DefenseScreen : MonoBehaviour
{
    int m_nCount = 0;
    private BuyMenu m_BuyMenu;

    //방어 장비의 갯수에 맞쳐 페이지를 계산 후 나온 값이 2임
    private int m_nAllSolt=2;
    private int m_nSoltCount;

    public void Init(List<GameObject> Lisdefense,BuyMenu buyMenu)
    {    
        m_BuyMenu = buyMenu;
    }

    public void NextBtn()
    {
        if (m_nAllSolt > m_nSoltCount)
        {
            ++m_nSoltCount;
            m_nCount += 4;
            m_BuyMenu.NextDefenseItem(m_nCount);
            AudioManager.Instance.PlayEffect(0);
        }
    }

    public void BackBtn()
    {
        if (0< m_nSoltCount)
        {
            --m_nSoltCount;
            m_nCount -= 4;
            m_BuyMenu.NextDefenseItem(m_nCount);
            AudioManager.Instance.PlayEffect(0);
        }
    }

}
