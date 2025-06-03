//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PointsUI : MonoBehaviour
//{
//    private Text pointsText;

//    void Start()
//    {
//        pointsText = GetComponent<Text>();
//        PointSystem.Instance.OnPointsChanged.AddListener(UpdatePointsDisplay);
//    }

//    void UpdatePointsDisplay(int points)
//    {
//        pointsText.text = $"Points: {points}";
//    }
//}
