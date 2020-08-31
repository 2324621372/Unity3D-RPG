using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIManager m_UIManager;
    public GameObject m_TowHandSwordObj;
    public GameObject m_MagicianObj;
    private GameObject _Obj = null;

    private void Awake()
    {
        DataMng.AddTable(TableType.PlayerTable);
        DataMng.AddTable(TableType.EXPTable);
        DataMng.AddTable(TableType.MonsterTable);
        m_UIManager.Init();
        switch (SelectScene.m_nIndex)
        {
            case 1:
                _Obj = Instantiate(m_TowHandSwordObj);
                _Obj.transform.position = new Vector3(0, 0.068f, 0);
                _Obj.name = m_TowHandSwordObj.name;
                _Obj.GetComponent<TowHandController>().Init();
                break;
            case 2:
                _Obj = Instantiate(m_MagicianObj);
                _Obj.transform.position = new Vector3(0, 0.068f, 0);
                _Obj.name = m_MagicianObj.name;
                _Obj.GetComponent<MagicianController>().Init();
                break;
        }
    }

}
