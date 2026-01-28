using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
/// <summary>
/// 设置管理
/// </summary>
public class SettingManager : MonoBehaviour
{
    [Header("音频混响器")]
    public AudioMixer mixer;

    public void SetBGMVolume(float volume)
    {
        mixer.SetFloat("BGM", volume);
    }
    public void SetSEVolume(float volume)
    {
        mixer.SetFloat("SFX", volume);
    }
}
