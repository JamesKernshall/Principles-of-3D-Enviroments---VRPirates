using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] [Range(0, 1)] private float musicVolume = 1f;
    [SerializeField] private float randomMinMusicGap = 20;
    [SerializeField] private float randomMaxMusicGap = 90;
    [SerializeField] private AudioClip[] music;

    [Space]

    [Header("Ambience")]
    [SerializeField] [Range(0, 1)] private float ambienceVolume = 0.6f;
    [SerializeField] private float randomMinSilence = 2;
    [SerializeField] private float randomMaxSilence = 10;
    [SerializeField] private AudioClip[] ambience;

    [Space]

    [Header("REQUIRED")]
    [SerializeField] AudioSource audioSource;
    

    float randomMusicGap = 100;
    float musicGap = 0;
    bool isMusicQueued = false;
    bool isSomethingQueued = false;

    // Start is called before the first frame update
    void Start()
    {
        RestartMusicTimer();
    }

    public void OverrideSound(AudioClip sound = null) 
    {
        this.enabled = false;
        audioSource.Stop();

        if (sound != null) 
        {
            audioSource.PlayOneShot(sound, 0.85f);
        }
    }

    IEnumerator SoundQueueTimer() 
    {
        isSomethingQueued = true;
        
        float randomTime = Random.Range(randomMinSilence, randomMaxSilence);
        yield return new WaitForSecondsRealtime(randomTime);

        if (isMusicQueued)
        {
            audioSource.PlayOneShot(music[Random.Range(0, music.Length)], musicVolume);
            isMusicQueued = false;
        }
        else
        {
            audioSource.PlayOneShot(ambience[Random.Range(0, ambience.Length)], ambienceVolume);
        }
        
        isSomethingQueued = false;
    }

    void RestartMusicTimer() 
    {
        musicGap = 0;
        randomMusicGap = Random.Range(randomMinMusicGap, randomMaxMusicGap);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMusicQueued)
        {
            musicGap += Time.deltaTime;
            if (musicGap > randomMaxMusicGap)
            {
                RestartMusicTimer();
                isMusicQueued = true;
            }
        }

        if (!audioSource.isPlaying && !isSomethingQueued) 
        {
            StartCoroutine(SoundQueueTimer());            
        }
    }
}
