using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeData", menuName = "Asteroids/TimeData")]
public class TimeData : ScriptableObject
{
    public float startingSeconds;
    public float longestTimeLastest = 0;
}
