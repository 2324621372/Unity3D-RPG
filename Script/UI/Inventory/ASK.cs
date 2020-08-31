using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASK : MonoBehaviour
{
    [SerializeField]
    protected List <GameObject> m_LisScreenObj=new List<GameObject>();

    public InstallScreen m_InstallScreen;
    protected GameObject m_OtherObj;

    public abstract void YesBtn();
    public abstract void NoBtn();
}
