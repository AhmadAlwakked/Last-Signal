//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PointSystem : MonoBehaviour
//{
//    public static PointSystem Instance { get; private set; }

//    [SerializeField] private int startingPoints = 100;
//    [SerializeField] private int pointsPerWave = 50;
//    [SerializeField] private float hpToPointsRatio = 0.5f; // 1 HP = 0.5 points
//    [SerializeField] private int towerCost = 25; // Kosten per toren

//    private int currentPoints;
//    public UnityEvent<int> OnPointsChanged = new UnityEvent<int>();

//    private void Awake()
//    {
//        // Singleton pattern
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }

//        currentPoints = startingPoints;
//        OnPointsChanged.Invoke(currentPoints);
//    }

//    // Converteer HP van gouden unit naar punten
//    public bool ConvertHpToPoints(float hpAmount)
//    {
//        int pointsToAdd = Mathf.FloorToInt(hpAmount * hpToPointsRatio);
//        if (pointsToAdd > 0)
//        {
//            currentPoints += pointsToAdd;
//            OnPointsChanged.Invoke(currentPoints);
//            return true;
//        }
//        return false;
//    }

//    // Voeg punten toe na een wave
//    public void AddWavePoints()
//    {
//        currentPoints += pointsPerWave;
//        OnPointsChanged.Invoke(currentPoints);
//    }

//    // Probeer een toren te bouwen
//    public bool TryBuildTower()
//    {
//        if (currentPoints >= towerCost)
//        {
//            currentPoints -= towerCost;
//            OnPointsChanged.Invoke(currentPoints);
//            return true;
//        }
//        return false;
//    }

//    // Huidige punten opvragen
//    public int GetCurrentPoints()
//    {
//        return currentPoints;
//    }

//    // Voor debug of speciale gevallen: directe punten toevoegen
//    public void AddPoints(int amount)
//    {
//        currentPoints += amount;
//        if (currentPoints < 0) currentPoints = 0;
//        OnPointsChanged.Invoke(currentPoints);
//    }

//    // Voor UI of andere systemen die punten willen checken
//    public bool HasEnoughPoints(int requiredPoints)
//    {
//        return currentPoints >= requiredPoints;
//    }
//}