using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//총알에게 플레이어의 스크립트를 얇게 주기위해
public interface PlayerChain
{
    void GetPlayer(int index, Vector3 skillPos);
}
public interface GetWeaponTrail
{
    int Setint();
    void ChangeWeaponTrail(GameObject obj);
}

public class PlayerController : ControllerManager, PlayerChain, GetWeaponTrail
{
    [HideInInspector]
    public ActionType actionType = ActionType.Idle;
    [HideInInspector]
    public Vector2 dir_normaliezd;
    [SerializeField]
    protected Transform m_Pivot;

    public Transform m_AttackPos;
    protected JoyStickMove m_joyStickMove;
    protected JoyStickAttack m_joyStickAttack;
    protected JoyStickRotate m_joyStickRotate;
    protected GameObject m_WeaponTrailObj;

    public float m_fPosZ;

    protected InstallEquipment m_InstallEquipment;

    public PlayerAbility m_PlayerAbility;

    public List<Skill> m_LisSkill = new List<Skill>();
    private Transform m_RevivalPos;
    protected bool m_bRevival = false;

    private GreyEffectShader m_DieGrey;

    public override void ChildInit()
    {
        m_WeaponTrailObj = GameObject.FindGameObjectWithTag("WeaponTrail");
        m_RevivalPos = GameObject.FindGameObjectWithTag("Revival").transform;

        m_DieGrey = FindObjectOfType<GreyEffectShader>();

        m_joyStickMove = FindObjectOfType<JoyStickMove>();
        m_joyStickAttack = FindObjectOfType<JoyStickAttack>();
        m_joyStickRotate = FindObjectOfType<JoyStickRotate>();
        m_InstallEquipment = FindObjectOfType<InstallEquipment>();

        m_PlayerAbility.Init();
        m_Bodybounds.center = m_Pivot.transform.position;

        JobInit();
    }
    protected virtual void JobInit() { }

    //기본 공격
    public virtual void Target(int angle) { }

    public virtual void SkillTarget(int index) { }

    //파티클 매니져에서 만들거나 찾아서 가지고 오며 스킬 파티클을 사용하는 부분
    public virtual void StartSkill(int index) { }
    public virtual void MakeBullet(int index) { }
    public void OnSkill()
    {
        StartCoroutine(m_joyStickAttack.Cooltime(m_joyStickAttack.m_fSkillIndex));
    }

    protected bool Critical()
    {
        int _nRandom = Random.Range(0, 100);

        if (_nRandom <= m_PlayerAbility.m_Critical)
        {
            return true;
        }
        return false;
    }

    public virtual void GetPlayer(int index, Vector3 skillPos) { }

    //죽고 마을로 돌아가는 부분
    public IEnumerator PlayerMoveRevival()
    {
        StartCoroutine(m_DieGrey.Grey());
        m_bRevival = true;
        AudioManager.Instance.PlayEffect(8);
        m_Ani.SetTrigger("Die");
        m_Ani.SetInteger("Skill",0);
        gameObject.GetComponent<NavMeshAgent>().enabled = false;

        yield return new WaitForSeconds(6.0f);
        if (SceneMng.m_uIScreen != null)
        {
            SceneMng.m_uIScreen.Execute(2.0f);
        }
        m_DieGrey.m_greyAmount = 0;
        yield return new WaitForSeconds(0.8f);
        transform.position = m_RevivalPos.position;
        m_Ani.SetTrigger("Idle");
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        m_PlayerAbility.SetHp((int)(m_PlayerAbility.m_AllHp * 0.5f));
        m_PlayerAbility.SetMp((int)(m_PlayerAbility.m_AllMp *0.5f));

        m_joyStickAttack.m_UIPlayerState.SetHp();
        m_joyStickAttack.m_UIPlayerState.SetMp();

        m_Bodybounds.center = m_Pivot.transform.position; 
        actionType = ActionType.Idle;
        m_bRevival = false;
    }

    public void ChangeWeaponTrail(GameObject obj)
    {
        m_WeaponTrailObj = obj;
    }

    public int Setint()
    {
        return m_PlayerAbility.m_nIndex;
    }

    public void PlaySound(int index)
    {
        AudioManager.Instance.PlayEffect(index);
    }
}
