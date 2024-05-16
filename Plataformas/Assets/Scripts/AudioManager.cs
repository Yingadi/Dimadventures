using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
     public void PlayClip(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    public void SetTheme(AudioClip theme)
    {
        GetComponent<AudioSource>().clip = theme;
        GetComponent<AudioSource>().Play();
    }

    public void StopTheme()
    {
        GetComponent<AudioSource>().Stop();
    }

    public void PlayOne(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
