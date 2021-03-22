using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = gameObject.AddComponent<AudioSource>();
        sound.clip = Resources.Load<AudioClip>("SFX/enemy_taking_damage_02");
        sound.PlayOneShot(sound.clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (!sound.isPlaying) Destroy(gameObject);
    }
}
