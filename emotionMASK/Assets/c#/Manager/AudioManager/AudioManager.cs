using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

//外界音频调用的方法：AudioManager.PlayAudio("音频名称",是否等待当前音频播放完毕);

/// <summary>
/// 音频管理器，用来存储所有音频以及播放和停止
/// </summary>
public class AudioManager : MonoBehaviour
{
    [System.Serializable]//序列化一下，再inspector面板显示会更好看
    public class Sound
    {
        [Header("音频名称")]
        public AudioClip clip;

        [Header("音频分组")]
        public AudioMixerGroup outputGroup;
        
        [Header("音频音量")]
        [Range(0f, 1f)]
        public float volume = 1f;

        [Header("音频是否开局播放")]
        public bool PlayerOnAwake = false;

        [Header("音频是否循环播放")]
        public bool loop = false;
    }
    /// <summary>
    /// 列表，存储所有的音频信息
    /// </summary>
    public List<Sound> sounds = new List<Sound>();
    /// <summary>
    /// 每一个音频剪辑的名字对应一个音频组件
    /// </summary>
    private Dictionary<string, AudioSource> audiosDic;
    /// <summary>
    /// 单例模式
    /// </summary>
    private static AudioManager instance;

    //初始化音频组件
    private void Awake()
    {
        instance = this;
        audiosDic = new Dictionary<string, AudioSource>();
        // foreach (Sound sound in sounds)
        // {
        //     AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        //     audioSource.clip = sound.clip;
        //     audioSource.outputAudioMixerGroup = sound.outputGroup;
        //     audioSource.volume = sound.volume;
        //     audioSource.loop = sound.loop;
        //     audiosDic.Add(sound.clip.name, audioSource);
        //     if (sound.PlayerOnAwake)
        //     {
        //         audioSource.Play();
        //     }
        // }
    }
    private void Start()
    {
        foreach (Sound sound in sounds)
        {
            GameObject obj = new GameObject(sound.clip.name);

            obj.transform.SetParent(transform);     //把挂载这个音频管理器的物体作为父物体，在他的子物体创建单个音频组件
            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = sound.clip;                                   //把这个音频赋值给音频组件
            source.outputAudioMixerGroup = sound.outputGroup;           //把这个音频组件分配到对应的音频组，也就是音频分组
            source.volume = sound.volume;                               //音量
            source.loop = sound.loop;                                   //循环播放
            audiosDic.Add(sound.clip.name, source);                      //把音频名字和组件加到字典中，方便后续调用
            if (sound.PlayerOnAwake)
            {
                source.Play();
            }
        }
    }
    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name="name">音频名称</param>
    /// <param name="isWait">是否等待当前音频播放完毕</param>
    public static void PlayAudio(string name,bool isWait = false)
    {
        if(!instance.audiosDic.ContainsKey(name))
        {
            Debug.LogWarning("没有找到对应的音频：" + name);
            return;
        }
        if (isWait)
        {
            if(!instance.audiosDic[name].isPlaying)
            {
                instance.audiosDic[name].Play();
            }
        }
        else
        {
            instance.audiosDic[name].Play();
        }
    }
    /// <summary>
    /// 停止播放某一个音频
    /// </summary>
    /// <param name="name">音频的名字</param>
    public static void StopAudio(string name)
    {
        if (!instance.audiosDic.ContainsKey(name))
        {
            Debug.LogWarning("没有找到对应的音频：" + name);
            return;
        }
        instance.audiosDic[name].Stop();
    }
}
