using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstallScreen : MonoBehaviour
{
    [HideInInspector]
    public InstallEquipment m_InstallEquipment;
    public List<GameObject> m_LisEquipmentObj = new List<GameObject>();
    private Inventory m_Inventory;
    private InterfaceCharacter m_InventoryCharacter;

    private void Start()
    {
        m_Inventory = FindObjectOfType<Inventory>();
        m_InstallEquipment = FindObjectOfType<InstallEquipment>();
        m_InventoryCharacter = FindObjectOfType<InterfaceCharacter>();
    }

    public void SetInstall(int equipment, int install)
    {
        //리스트는 0부터 테이블은 1부터 가기 떄문에 -1를 하였음
        if (install == 3)
        {
            m_InstallEquipment.InstallWeapon(equipment - 1, m_Inventory, m_InventoryCharacter);
        }
        else if (install == 4)
        {
            m_InstallEquipment.InstallAccessory(equipment, m_Inventory, install);
        }
        else
        {
            m_InstallEquipment.InstallArmor(equipment - 1, m_Inventory, install);
        }
    }

    public void SetInstallPortion(int equipment, int install, GameObject obj)
    {
        m_InstallEquipment.InstallPortion(equipment - 1, m_Inventory, install, obj);
    }
}