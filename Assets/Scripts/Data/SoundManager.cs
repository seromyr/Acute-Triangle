using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sound;

    // Start is called before the first frame update
    void Awake()
    {
        SingletonizeSoundManager();
        CreateAudioClips();
    }

    private void CreateAudioClips()
    {
        
    }

    private void SingletonizeSoundManager()
    {
        if(sound == null)
        {
            sound = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
