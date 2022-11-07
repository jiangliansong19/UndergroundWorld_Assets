using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����������
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { private set; get; }

    //��������
    public enum Sound
    {
        ButtonClick,
        ClickError,
    }

    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();

        //System.Enum.GetValues(typeof(Sound)) ��ȡ��ö�ٵ��ַ������顣
        foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(soundAudioClipDictionary[sound]);
    }
}
