using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TimeData timeData;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderValueText;
    [SerializeField] private TextMeshProUGUI personalBestText;
    float currentTimeInSeconds = 300f;

    private void Start()
    {
        personalBestText.text = TimeManager.ConvertSecondsToString(timeData.longestTimeLastest);
    }

    public void StartGame() 
    {
        //Save time to scriptable object
        timeData.startingSeconds = currentTimeInSeconds;

        //Load Scene
        SceneManager.LoadScene(1);
    }

    public void CalculateTime() 
    {
        currentTimeInSeconds = slider.value * 30;

        //Format time onto slider
        TimeSpan t = TimeSpan.FromSeconds(currentTimeInSeconds);

        string answer = string.Format("{0:D2}:{1:D2}",
                        t.Minutes,
                        t.Seconds);
        sliderValueText.text = answer;
    }

    public void QuitGame() 
    {
        Application.Quit();
        Debug.Log("QuitGame");
    }
}
