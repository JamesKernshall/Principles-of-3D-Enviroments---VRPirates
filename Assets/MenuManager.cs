using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private ScriptableObject timeData;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderValueText;
    float currentTimeInSeconds = 300f;

    public void StartGame() 
    {
        //Save time to scriptable object
        SceneManager.LoadScene(1);
    }

    public void CalculateTime() 
    {
        currentTimeInSeconds = slider.value * 30;

        TimeSpan t = TimeSpan.FromSeconds(currentTimeInSeconds);

        string answer = string.Format("{0:D2}:{2:D2}",
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
