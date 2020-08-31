using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    enum CilpNumber
    {
        Click = 0,
        TowHandSword_Choice = 1,
        Magician_Choice = 2,
        Attack = 3,
        Skill_1=4,
        Skill_2=5,
        Skill_3=6,
        Skill_4=7,
        Die=8,
        ItemInput=9,
        Drink=10,
        questClear=11,
        MonsterHit=12,
        BossOpening=13,
        Cry=14,
        BossAttack=15,
        BossDie=16,
    }

    [SerializeField]
    private AudioSource MusicSource;

    private float m_nMusicVolume = 0.5f;

    private float m_nSoundVolume = 1.0f;

    private List<AudioSource> m_effectSources = new List<AudioSource>();
    public List<AudioClip> m_LisEffectClip = new List<AudioClip>();
    public AudioClip m_WorldClip;

    private int m_nMax = 10;

    //싱글톤으로 사용함
    private static AudioManager m_Instance;
    public static AudioManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject obj = Instantiate(Resources.Load("AudioManager")) as GameObject;
                DontDestroyOnLoad(obj);
                m_Instance = obj.GetComponent<AudioManager>();
            }
            return m_Instance;
        }
    }

    private void Update()
    {
        MusicSource.volume = m_nMusicVolume;

        if (m_nMax <= m_effectSources.Count)
        {
            for (int i = 0; i < m_effectSources.Count; ++i)
            {
                if (m_effectSources[i].isPlaying == false)
                {
                    Destroy(m_effectSources[i].gameObject);
                    m_effectSources.RemoveAt(i);
                    return;
                }

            }

        }

    }

    public void WorldSound(int index)
    {
        MusicSource.clip = m_WorldClip;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void PlayEffect(int index)
    {
        AudioSource audio = null;

        if (m_effectSources.Count > 0)
        {
            for (int i = 0; i < m_effectSources.Count; ++i)
            {
                if (m_effectSources[i].isPlaying == false)
                {
                    audio = m_effectSources[i];
                    break;
                }
            }
        }

        if (audio != null)
        {
            audio.name = m_LisEffectClip[index].name;
            audio.PlayOneShot(m_LisEffectClip[index], m_nSoundVolume);
        }
        else
        {
            GameObject obj = new GameObject(m_LisEffectClip[index].name, typeof(AudioSource));
            obj.transform.parent = gameObject.transform;
            audio = obj.GetComponent<AudioSource>();
            audio.PlayOneShot(m_LisEffectClip[index], m_nSoundVolume);
            m_effectSources.Add(audio);
        }
    }

    public void AudioRemove(int index)
    {
        m_nMusicVolume = 0.2f;
        if (index == 2)
        {
            for (int i = 3; i < 9; ++i)
            {
                m_LisEffectClip.Remove(m_LisEffectClip[3]);
            }
        }
        else
        {
            for (int i = 9; i < 15; ++i)
            {
                m_LisEffectClip.Remove(m_LisEffectClip[9]);
            }
        }
    }
}
