using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AskScreen : ASK
{
    [SerializeField]
    private Image m_BeforeImg;
    [SerializeField]
    private Text m_BeforeTxt;
    [SerializeField]
    private Text m_BeforeName;

    [SerializeField]
    private Image m_AfterImg;
    [SerializeField]
    private Text m_AfterTxt;
    [SerializeField]
    private Text m_AfterName;

    public List<GameObject> m_EquipmentInstall = new List<GameObject>();

    private Sprite AfterSprite;

    int m_nInstall;
    int m_nIndex;

    int m_nTableIndex;
    int m_nTable;
    private GameObject m_ItemObj;
    [HideInInspector]
    public GameObject[] m_BeforeOtherObj=new GameObject[7];
    public Dictionary<int, int> m_DicBeforeIndex = new Dictionary<int, int>();

    int m_nText;

    public override void YesBtn()
    {
        if (m_EquipmentInstall[m_nInstall].transform.childCount >= 2)
        {
            m_EquipmentInstall[m_nInstall].transform.GetChild(1).position = m_LisScreenObj[m_nTable].transform.position;
            m_EquipmentInstall[m_nInstall].transform.GetChild(1).SetParent(m_LisScreenObj[m_nTable].transform);
        }

        gameObject.SetActive(false);

        m_EquipmentInstall[m_nInstall].transform.GetChild(0).gameObject.SetActive(false);

        m_ItemObj.transform.SetParent(m_EquipmentInstall[m_nInstall].transform);

        m_ItemObj.transform.position = m_EquipmentInstall[m_nInstall].transform.GetChild(0).position;

        if (m_nInstall == 5 || m_nInstall == 6)
        {
            m_InstallScreen.SetInstallPortion(m_nTableIndex, m_nInstall, m_ItemObj);
        }
        else
        {
            m_InstallScreen.SetInstall(m_nTableIndex, m_nInstall);
            m_OtherObj.SetActive(false);
        }

        m_DicBeforeIndex[m_nInstall] = m_nTableIndex;

        if (m_BeforeOtherObj[m_nInstall] != null)
        {
            m_BeforeOtherObj[m_nInstall].SetActive(true);
        }
        m_BeforeOtherObj[m_nInstall] = m_OtherObj;

        AudioManager.Instance.PlayEffect(9);
    }

    public override void NoBtn()
    {
        gameObject.SetActive(false);

        AudioManager.Instance.PlayEffect(0);
    }

    public void SetEquipment(Image image, int index, GameObject Obj, int install, GameObject other)
    {
        TableType _text = TableType.Nono;

        string _sProperty = string.Empty;
        int _nBeforeSolt = 0;
        m_nInstall = install;

        if (!m_DicBeforeIndex.ContainsKey(install))
        {
            m_DicBeforeIndex.Add(install, 1);
        }

        if (m_EquipmentInstall[m_nInstall].transform.childCount >= 2)
        {
            _nBeforeSolt = 1;
        }

        if (install < 3)
        {
            _text = TableType.ArmorTable;
            _sProperty = "DEFENSE";
            m_nTable = 1;
            m_BeforeTxt.text = "방어력" + " + " + DataMng.Get(_text).ToS(m_DicBeforeIndex[install], _sProperty);
            m_AfterTxt.text = "방어력" + " + " + DataMng.Get(_text).ToS(index, _sProperty);
        }
        else if (install == 3)
        {
            string _sCritical = string.Empty;
            _text = TableType.WeaponTable;
            _sProperty = "ATTACK";
            _sCritical = "CRITICAL";
            m_nTable = 0;
            m_BeforeTxt.text = "공격력" + " + " + DataMng.Get(_text).ToS(m_DicBeforeIndex[install], _sProperty)+ "\n크리티컬 + " + DataMng.Get(_text).ToS(m_DicBeforeIndex[install], _sCritical)+" %";
            m_AfterTxt.text = "공격력" + " + " + DataMng.Get(_text).ToS(index, _sProperty) + "\n크리티컬 + " + DataMng.Get(_text).ToS(index, _sCritical)+ " %";
        }
        else if (install == 5 || install == 6)
        {
            _text = TableType.PortionTable;
            _sProperty = "Content";
            m_BeforeTxt.text = DataMng.Get(_text).ToS(m_DicBeforeIndex[install], _sProperty);
            m_AfterTxt.text = DataMng.Get(_text).ToS(index, _sProperty);
            m_nTable = 3;
        }
        else
        {
            _text = TableType.AccessoryTable;
            _sProperty = "Content";
            m_BeforeTxt.text = DataMng.Get(_text).ToS(m_DicBeforeIndex[install], _sProperty);
            m_AfterTxt.text = DataMng.Get(_text).ToS(index, _sProperty);
            m_nTable = 2;
        }

        //예외처리
        if (_text == TableType.Nono)
        {
            return;
        }

        //바꾸기전
        Image _image = m_EquipmentInstall[m_nInstall].transform.GetChild(_nBeforeSolt).GetComponent<Image>();
        m_BeforeImg.sprite = _image.sprite;

        m_BeforeName.text = DataMng.Get(_text).ToS(m_DicBeforeIndex[install], "K_NAME");

        if (m_DicBeforeIndex[install] == 1 &&
            m_EquipmentInstall[m_nInstall].transform.childCount <= 1)
        {
            m_BeforeName.text = " ";
            m_BeforeTxt.text = " ";
        }

        //바꾸기 후
        m_AfterImg.sprite = image.sprite;
        m_AfterName.text = DataMng.Get(_text).ToS(index, "K_NAME");

        AfterSprite = image.sprite;
        m_nTableIndex = index;

        m_ItemObj = Obj;

        m_OtherObj = other;
    }
}
