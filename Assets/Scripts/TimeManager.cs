using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("REQUIRED")]

    [SerializeField] private MusicPlayer musicPlayer;
    [SerializeField] private TimeData timeStart;

    [SerializeField] private TextMeshPro timeLeftText;
    [SerializeField] private TextMeshPro oilPercentageText;
    [SerializeField] private TextMeshPro timeElapsedText;

    [SerializeField] private float finalMomentsStartingTime = 15;
    [SerializeField] private AudioClip finalMomentsSFX;
    [SerializeField] private Animator finalMomentsAnimation;

    [Space]

    [Header("Parameters")]

    [SerializeField] private float timeGainedPerOil = 20;
    [Tooltip("Effects what 100% is for the oil percentage")]
    [SerializeField] private float oilMaxTimePercentage = 600;

    /// private Variables
    private float secondsLeft = 300;

    private float timeElapsed = 0;

    private bool musicResponsiblity = false;
    private void Start()
    {
        FruitManager.instance.oilDrank += OnDrinkOil;
        secondsLeft = timeStart.startingSeconds;
    }

    void OnDrinkOil(float amount)
    {
        float addedTime = amount * timeGainedPerOil;
        secondsLeft += addedTime;
    }

    // Update is called once per frame
    void Update()
    {
        secondsLeft -= Time.deltaTime;
        timeElapsed += Time.deltaTime;


        timeLeftText.text = ConvertSecondsToString(secondsLeft);

        oilPercentageText.text = CalculateOilLeft().ToString();

        timeElapsedText.text = ConvertSecondsToString(timeElapsed);

        if (secondsLeft < finalMomentsStartingTime)
        {
            if (!musicResponsiblity)
            {
                musicPlayer.OverrideSound(finalMomentsSFX);
                finalMomentsAnimation.Play("Die");
                musicResponsiblity = true;
            }
        }
        else
        {
            if (musicResponsiblity) // So other objects can override the music
            {
                musicPlayer.enabled = true;
                musicResponsiblity = false;
                finalMomentsAnimation.Play("Empty");
            }
        }

        if (secondsLeft <= 0) 
        {
            // Game Over
            if (timeElapsed > timeStart.longestTimeLastest) 
            {
                timeStart.longestTimeLastest = timeElapsed;
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

    }

    int CalculateOilLeft() 
    {
        int percentage = (int)((secondsLeft / oilMaxTimePercentage) * 100); 

        return Math.Clamp(percentage, 0, 100);
    }


    public static string ConvertSecondsToString(float seconds) 
    {
        //Format time onto text
        TimeSpan t = TimeSpan.FromSeconds(seconds);

        string answer = string.Format("{0:D2}:{1:D2}",
                        t.Minutes,
                        t.Seconds);

        return answer;
    }
}
