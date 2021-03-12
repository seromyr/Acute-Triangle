using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sound;

    private AudioClip mainMenuBGM;
    private AudioClip menuSelect;
    private AudioClip level01BGM;
    private AudioClip level02BGM;
    private AudioClip level03BGM;
    private AudioClip level04BGM;
    private AudioClip level05BGM;
    private AudioClip level06BGM;
    private AudioClip level07BGM;
    private AudioClip level08BGM;
    private AudioClip level09BGM;
    private AudioClip level10BGM;

    private AudioClip enemy_TakeDamage;

    private AudioClip player_shooting;
    private AudioClip player_TakeDamage;

    // Start is called before the first frame update
    void Awake()
    {
        SingletonizeSoundManager();
        CreateAudioClips();
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

    private void CreateAudioClips()
    {
        mainMenuBGM = Resources.Load<AudioClip>("Musics/Background_music_mixdown.wav");
        menuSelect = Resources.Load<AudioClip>("Musics/menu select_loop");
        level01BGM = Resources.Load<AudioClip>("Musics/Boss_1_mixdown");
        level02BGM = Resources.Load<AudioClip>("Musics/Boss theme2_loop");
        level03BGM = Resources.Load<AudioClip>("Musics/Boss theme3_loop");
        level04BGM = Resources.Load<AudioClip>("Musics/Boss theme4_loop");

        enemy_TakeDamage = Resources.Load<AudioClip>("SFX/enemy_taking_damage_01");

        player_shooting = Resources.Load<AudioClip>("SFX/laser");
        player_TakeDamage = Resources.Load<AudioClip>("SFX/taking_damage");

    }

    public AudioClip GetMainMenuBGM() => mainMenuBGM;

    public AudioClip GetLevel01BGM() => level01BGM;
    public AudioClip GetLevel02BGM() => level02BGM;
    public AudioClip GetLevel03BGM() => level03BGM;
    public AudioClip GetLevel04BGM() => level04BGM;


    public void SetupMainMenuMusic()
    {
        AudioSource music = GameObject.FindGameObjectWithTag("BMG").GetComponent<AudioSource>();
        music.clip = mainMenuBGM;
        music.loop = true;
        music.Play();
        
    }
}
