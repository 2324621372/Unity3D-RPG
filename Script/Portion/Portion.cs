using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portion : MonoBehaviour
{
    private enum PortionType
    {
        Hp = 1,
        Mp = 2,
        Attack = 3,
        Defense = 4,
    }

    private PlayerAbility m_PlayerAbility;
    private UIPlayerState m_UIPlayerState;

    private int m_nIndex;
    private int m_nType;
    private int m_fEffect;

    private int m_nAllHp;
    private int m_nAllMp;

    private int m_nCount;
    private Text m_CountTxt;
    private Text m_InInvenCountTxt;

    private GameObject m_InInvenObj;

    public void Init(PlayerAbility playerAbility, UIPlayerState uIPlayerState, GameObject obj)
    {
        m_PlayerAbility = playerAbility;
        m_UIPlayerState = uIPlayerState;
        m_nIndex = GetComponent<PortionEquipment>().m_nIndex;
        m_nType = DataMng.Get(TableType.PortionTable).ToI(m_nIndex, "Type");

        m_fEffect = DataMng.Get(TableType.PortionTable).ToI(m_nIndex, "Effect");

        Button _button = GetComponent<Button>();
        _button.onClick.AddListener(Drink);

        m_CountTxt = transform.GetChild(2).GetComponent<Text>();
        m_InInvenCountTxt = obj.transform.GetChild(2).GetComponent<Text>();
        m_nCount = int.Parse(m_CountTxt.text);

        m_InInvenObj = obj;
    }

    public void Drink()
    {
        AudioManager.Instance.PlayEffect(10);
        switch (m_nType)
        {
            case (int)PortionType.Hp:
                m_PlayerAbility.SetHp(m_PlayerAbility.m_Hp + m_fEffect);

                if (m_PlayerAbility.m_AllHp < m_PlayerAbility.m_Hp)
                {
                    m_PlayerAbility.SetHp(m_PlayerAbility.m_AllHp);
                }
                m_UIPlayerState.SetHp();
                break;
            case (int)PortionType.Mp:
                m_PlayerAbility.SetMp(m_PlayerAbility.m_Mp + m_fEffect);

                if (m_PlayerAbility.m_AllMp < m_PlayerAbility.m_Mp)
                {
                    m_PlayerAbility.SetMp(m_PlayerAbility.m_AllMp);
                }
                m_UIPlayerState.SetMp();
                break;
            case (int)PortionType.Attack:
                m_PlayerAbility.buff(m_nType, m_fEffect);

                break;
            case (int)PortionType.Defense:
                m_PlayerAbility.buff(m_nType, m_fEffect);

                break;
            default:
                return;
        }
        --m_nCount;
        m_CountTxt.text = m_nCount.ToString();
        m_InInvenCountTxt.text = m_nCount.ToString();

        if (m_nCount < 1)
        {
            Destroy(gameObject);
            m_InInvenObj.GetComponent<PortionEquipment>().ZeroCount();
        }
    }

}
