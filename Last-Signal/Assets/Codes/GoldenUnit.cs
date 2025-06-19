using UnityEngine;

public class GoldenUnit : BaseClass
{
    protected override void Initialize()
    {
        maxHP = 5000f; // Stel maxHP in voor GoldenUnit
    }

    protected override void Start()
    {
        base.Start();
        gameObject.tag = "GoldenUnit"; // Zorgt voor consistente tagging
        Debug.Log($"GoldenUnit geïnitialiseerd met {currentHP}/{maxHP} HP");
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("GoldenUnit vernietigd! Spel verloren.");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameLost();
        }
    }
}