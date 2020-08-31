using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum SceneChange
{
    None = -1,
    SelectScene=0,
    MainScene=1,
}

public class SceneMng : MonoBehaviour
{
    public static UIFade m_uIScreen;

    //싱글톤 패턴
    private static SceneMng m_Instance;
    public static SceneMng Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject obj = new GameObject("Scene Manager", typeof(SceneMng));
                m_Instance = obj.GetComponent<SceneMng>();
                DontDestroyOnLoad(obj);
            }
            return m_Instance;
        }
    }

    public void NextScene(SceneChange sceneChange, float time)
    {
        StartCoroutine(IELoadAsyncScene(sceneChange, time));
    }

    private IEnumerator IELoadAsyncScene(SceneChange sceneChange, float time)
    {
        yield return new WaitForSeconds(time);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneChange.ToString());

        while (operation.isDone == false)
        {
            yield return null;
        }
    }
}
