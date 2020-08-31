using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임메니져에 있으며 여기서 인벤토리에서 받으면 실제로 검을 만들어 장착 시켜준다.
public class InstallEquipment : MonoBehaviour
{
    private GameObject m_Hand_RObj;
    private GameObject m_InventoryObj;

    private Dictionary<int, GameObject> m_DicWeaponObj = new Dictionary<int, GameObject>();

    private Dictionary<string, int> m_DicIndex = new Dictionary<string, int>();
    public Dictionary<int, GameObject> m_DicPortion = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> m_DicPortionObj = new Dictionary<int, GameObject>();

    //방어구에 따라 감소나 증가를 하기 위함
    private Dictionary<int, int> m_DicStall = new Dictionary<int, int>();

    private int m_nArmorIndex = 1;
    private int m_nKey = 0;
    private int m_nAccessoryState;
    private int m_nAccessoryIncrease;

    private JoyStickBtn m_JoyStickBtn;
    private GetWeaponTrail m_GetWeaponTrail;
    private UIPlayerState m_UIPlayerState;

    void Start()
    {
        Object[] _obj;
        m_Hand_RObj = GameObject.FindGameObjectWithTag("Hand_R");
        m_UIPlayerState = FindObjectOfType<UIPlayerState>();
        m_JoyStickBtn = FindObjectOfType<JoyStickBtn>();
        m_GetWeaponTrail = m_Hand_RObj.transform.root.gameObject.GetComponent<PlayerController>();
        int _index = m_GetWeaponTrail.Setint();

        switch (_index)
        {
            case 1:
                m_nKey = 0;
                _obj = Resources.LoadAll("Weapon/2Hand-Sword");
                for (int i = 0; i < _obj.Length; ++i)
                {
                    m_DicWeaponObj.Add(i, _obj[i] as GameObject);
                }
                break;
            case 2:
                m_nKey = 4;
                //전사의 무기 갯수는 4개이기 떄문에 +4
                _obj = Resources.LoadAll("Weapon/Magician");
                for (int i = 0; i < _obj.Length; ++i)
                {
                    if (i == 0)
                    {
                        m_DicWeaponObj.Add(i, _obj[i] as GameObject);
                    }
                    else
                    {
                        m_DicWeaponObj.Add(i + 4, _obj[i] as GameObject);
                    }
                }
                break;
        }

        for (int i = 0; i < m_DicWeaponObj.Count; ++i)
        {
            if (i == 0)
            {
                m_DicIndex.Add(m_DicWeaponObj[i].name, i + 1);
            }
            else
            {
                m_DicIndex.Add(m_DicWeaponObj[m_nKey + i].name, i + 1);
            }

        }
    }

    public void InstallWeapon(int index, Inventory inventory, InterfaceCharacter inventoryCharacter)
    {
        for (int i = 0; i < m_Hand_RObj.transform.childCount; ++i)
        {
            if (m_Hand_RObj.transform.GetChild(i).tag.Equals("Weapon"))
            {
                inventory.m_Ability.SetAttack(inventory.m_Ability.m_Attack - DataMng.Get(TableType.WeaponTable).ToI(m_DicIndex[m_Hand_RObj.transform.GetChild(i).name], "ATTACK"));
                inventory.m_Ability.SetCritical(inventory.m_Ability.m_Critical - DataMng.Get(TableType.WeaponTable).ToI(m_DicIndex[m_Hand_RObj.transform.GetChild(i).name], "CRITICAL"));
                Destroy(m_Hand_RObj.transform.GetChild(i).gameObject);
            }
        }

        GameObject Weapon = Instantiate(m_DicWeaponObj[index], m_Hand_RObj.transform);
        Weapon.name = m_DicWeaponObj[index].name;
        m_GetWeaponTrail.ChangeWeaponTrail(Weapon.transform.GetChild(0).gameObject);
        inventoryCharacter.InstallInventory(Weapon.name);

        //index+1한 이유는 테이블이기 떄문에 다시 +1를 하였음
        inventory.m_Ability.SetAttack(inventory.m_Ability.m_Attack + DataMng.Get(TableType.WeaponTable).ToI(index + 1, "ATTACK"));
        inventory.m_Ability.SetCritical(inventory.m_Ability.m_Critical + DataMng.Get(TableType.WeaponTable).ToI(index + 1, "CRITICAL"));
    }
    //방어구들의 능력치가 공유된다 따라서 헬멧 몸 신발을 구분해서 넣을수 있게 해야함
    //몸 머리 신발 
    public void InstallArmor(int index, Inventory inventory, int install)
    {
        if (m_DicStall.ContainsKey(install))
        {
            inventory.m_Ability.SetDefense(inventory.m_Ability.m_Defense - DataMng.Get(TableType.ArmorTable).ToI(m_DicStall[install], "DEFENSE"));
            m_DicStall.Remove(install);
        }

        inventory.m_Ability.SetDefense(inventory.m_Ability.m_Defense + DataMng.Get(TableType.ArmorTable).ToI(index + 1, "DEFENSE"));
        m_nArmorIndex = index + 1;

        //머리, 몸 , 신발중 어디인지 구분하기 위한 부분이며  값은 그 방어 값을 빼기 위한 값이다.
        m_DicStall.Add(install, m_nArmorIndex);
    }


    public void InstallAccessory(int index, Inventory inventory, int install)
    {
        //있던 보석의 능력치를 제거
        switch (m_nAccessoryState)
        {
            case 1:
                inventory.m_Ability.SetAllHp(inventory.m_Ability.m_AllHp - m_nAccessoryIncrease);
                m_UIPlayerState.SetAllHp();
                m_UIPlayerState.SetHp();
                break;
            case 2:
                inventory.m_Ability.SetAllMp(inventory.m_Ability.m_AllMp - m_nAccessoryIncrease);
                m_UIPlayerState.SetAllMp();
                m_UIPlayerState.SetMp();
                break;
            case 3:
                inventory.m_Ability.SetDefense(inventory.m_Ability.m_Defense - m_nAccessoryIncrease);
                break;
            case 4:
                inventory.m_Ability.SetAttack(inventory.m_Ability.m_Attack - m_nAccessoryIncrease);
                break;
        }
        //장착하여 보석의 능력치를 생성
        switch (index)
        {
            case 1:
                m_nAccessoryIncrease = (int)(inventory.m_Ability.m_AllHp * (DataMng.Get(TableType.AccessoryTable).ToF(index, "INCREASE") *0.01f));
                inventory.m_Ability.SetAllHp(inventory.m_Ability.m_AllHp + m_nAccessoryIncrease);
                m_UIPlayerState.SetAllHp();
                m_UIPlayerState.SetHp();
                break;
            case 2:
                m_nAccessoryIncrease = (int)(inventory.m_Ability.m_AllMp * (DataMng.Get(TableType.AccessoryTable).ToF(index, "INCREASE") * 0.01f));
                inventory.m_Ability.SetAllMp(inventory.m_Ability.m_AllMp + m_nAccessoryIncrease);
                m_UIPlayerState.SetAllMp();
                m_UIPlayerState.SetMp();
                break;
            case 3:
                m_nAccessoryIncrease = (int)(inventory.m_Ability.m_Defense * (DataMng.Get(TableType.AccessoryTable).ToF(index, "INCREASE") * 0.01f));
                inventory.m_Ability.SetDefense(inventory.m_Ability.m_Defense + m_nAccessoryIncrease);
                break;
            case 4:
                m_nAccessoryIncrease = (int)(inventory.m_Ability.m_Attack * (DataMng.Get(TableType.AccessoryTable).ToF(index, "INCREASE") * 0.01f));
                inventory.m_Ability.SetAttack(inventory.m_Ability.m_Attack + m_nAccessoryIncrease);
                break;
        }
        m_nAccessoryState = index;
    }

    public void InstallPortion(int index, Inventory inventory, int install, GameObject obj)
    {
        if (!m_DicPortion.ContainsKey(install))
        {
            m_DicPortion.Add(install, Instantiate(obj));
        }
        else
        {
            m_DicPortion[install] = Instantiate(obj);
        }
        m_DicPortion[install].AddComponent<Portion>();
        Portion _protion = m_DicPortion[install].GetComponent<Portion>();
        _protion.Init(inventory.m_Ability, m_UIPlayerState, obj);

        //넘어오는 곳에서 -1를했기 때문에 다시 1를 하여 정상 복귀시킴
        if (!m_DicPortionObj.ContainsKey(index + 1))
        {
            m_DicPortionObj.Add(index + 1, m_DicPortion[install]);
        }

        switch (install)
        {
            case 5:
                if (m_JoyStickBtn.m_Portion_1.transform.childCount > 0)
                {
                    Destroy(m_JoyStickBtn.m_Portion_1.transform.GetChild(0).gameObject);
                }
                m_DicPortion[install].transform.SetParent(m_JoyStickBtn.m_Portion_1.transform);
                break;
            case 6:
                if (m_JoyStickBtn.m_Portion_2.transform.childCount > 0)
                {
                    Destroy(m_JoyStickBtn.m_Portion_2.transform.GetChild(0).gameObject);
                }
                m_DicPortion[install].transform.SetParent(m_JoyStickBtn.m_Portion_2.transform);
                break;
        }
        m_DicPortion[install].transform.localScale = new Vector3(1.4f, 0.26f, 1);
        m_DicPortion[install].transform.localPosition = Vector2.zero;
    }

}