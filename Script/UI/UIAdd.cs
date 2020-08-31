using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    UIOutput,
    PlayerState,
    JoyStick,
}

public static class UIAdd
{
    private static Dictionary<UIType, GameObject> m_uiDic = new Dictionary<UIType, GameObject>();
    public static void DicClear()
    {
        m_uiDic.Clear();
    }

    public static T Load<T>(UIType uiType, bool active = true) where T : Component
    {
        string path = "UI/" + uiType.ToString();
        GameObject obj = Resources.Load<GameObject>(path);
        if (obj != null)
        {
            obj = GameObject.Instantiate(obj);
            obj.name = uiType.ToString();
            obj.transform.SetParent(GameObject.Find("UIManager").transform);
            obj.SetActive(active);

            m_uiDic.Add(uiType, obj);

            return obj.GetComponent<T>();
        }

        return null;
    }

    public static T Get<T>(UIType uiType) where T : Component
    {
        if (m_uiDic.ContainsKey(uiType))
            return m_uiDic[uiType].GetComponent<T>();

        return null;
    }

    public static void Show(UIType uiType, bool state)
    {
        if (m_uiDic.ContainsKey(uiType))
            m_uiDic[uiType].SetActive(state);
    }

}
