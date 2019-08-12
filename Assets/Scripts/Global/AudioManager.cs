using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private static AudioManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        if (instance == null)
            instance = this;
    }

    [SerializeField] AudioSource voiceAS;
    [SerializeField] AudioSource musicAS;

    public static void PlayVoiceOneShot(AudioClip ac, bool stopIfPLaying = false)
    {
        if (instance.voiceAS.isPlaying && stopIfPLaying)
            instance.voiceAS.Stop();
        instance.voiceAS.clip = ac;
        instance.voiceAS.loop = false;
        instance.voiceAS.Play();
    }

    public static void StopVoice()
    {
        instance.voiceAS.Stop();
    }


}
