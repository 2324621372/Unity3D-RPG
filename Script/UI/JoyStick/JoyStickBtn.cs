using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickBtn : MonoBehaviour
{
    private GameObject m_InterfaceObj;

    private GameObject _skillBtnObj;
    private GameObject m_InventoryBtnObj;

    private UIManager m_UIManager;

    public GameObject m_Portion_1;
    public GameObject m_Portion_2;
    private SkillScreen m_SkillScreen;

    private void Start()
    {
        PlayerController _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_InterfaceObj = GameObject.FindGameObjectWithTag("Interface");
        m_UIManager = FindObjectOfType<UIManager>();

        GameObject InventoryObj = Resources.Load("UI/Inventory") as GameObject;
        m_InventoryBtnObj = Instantiate(InventoryObj, m_InterfaceObj.transform);
        m_InventoryBtnObj.name = InventoryObj.name;
        m_InventoryBtnObj.SetActive(false);

        GameObject skillObj = Resources.Load("UI/Skill") as GameObject;
        _skillBtnObj = Instantiate(skillObj, m_InterfaceObj.transform);
        _skillBtnObj.name = skillObj.name;
        m_SkillScreen= _skillBtnObj.GetComponent<SkillScreen>();
        m_SkillScreen.Init(_playerController);
        _skillBtnObj.SetActive(false);

        m_UIManager.Save(m_InventoryBtnObj.GetComponent<Inventory>(), _playerController.m_PlayerAbility);
    }

    public void SkillBtn()
    {
        AudioManager.Instance.PlayEffect(0);
        m_SkillScreen.InputSkill();
        _skillBtnObj.SetActive(true);
    }

    public void InventoryBtn()
    {
        AudioManager.Instance.PlayEffect(0);
        m_InventoryBtnObj.SetActive(true);
    }
}
