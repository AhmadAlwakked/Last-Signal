using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer2 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTiem;
   


    private void Update()
    {
        elapsedTiem += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTiem / 60);
        int seconds = Mathf.FloorToInt(elapsedTiem % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);


    }
}
