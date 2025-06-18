using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseClass : MonoBehaviour
{
    [SerializeField, Tooltip("Maximale gezondheid van het object. Moet groter zijn dan 0.")]
    protected float maxHP = 100f; // Maximale gezondheid, aanpasbaar in Inspector
    protected float currentHP; // Huidige gezondheid

    // Publieke properties om toegang te bieden
    public float MaxHP => maxHP; // Read-only toegang tot maxHP
    public float CurrentHP => currentHP; // Read-only toegang tot currentHP

    // Start: eenmalige initialisatie
    protected virtual void Start()
    {
        // Valideer maxHP en stel currentHP in
        maxHP = Mathf.Max(1f, maxHP); // Voorkom ongeldige maxHP
        currentHP = maxHP;
        Debug.Log($"[{gameObject.name}] BaseClass: Geïnitialiseerd met {currentHP}/{maxHP} HP");
        Initialize(); // Laat kindklassen hun eigen opstart toevoegen
    } // Einde Start

    // Update: frame-voor-frame logica
    protected virtual void Update()
    {
        UpdateLogic(); // Laat kindklassen hun eigen logica toevoegen
    } // Einde Update

    // Virtuele functie voor initialisatie (bijv. NavMesh voor vijanden)
    protected virtual void Initialize()
    {
        // Leeg, voor kindklassen zoals BaseTower of BaseEnemy
    } // Einde Initialize

    // Virtuele functie voor frame-elijke logica (bijv. bewegen, aanvallen)
    protected virtual void UpdateLogic()
    {
        // Leeg, voor kindklassen zoals toren schieten, vijand bewegen
    } // Einde UpdateLogic

    // Virtuele functie voor reactie op schade (bijv. visuele effecten)
    protected virtual void OnDamageTaken(float damage)
    {
        // Leeg, voor kindklassen zoals rode flits of geluid
    } // Einde OnDamageTaken

    // Schade ontvangen en dood controleren
    public virtual void TakeDamage(float damage)
    {
        if (damage <= 0)
        {
            Debug.LogWarning($"[{gameObject.name}] BaseClass: Ongeldige schade ({damage}) genegeerd");
            return; // Voorkom negatieve of nul schade
        }
        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP); // Clamp HP
        Debug.Log($"[{gameObject.name}] BaseClass: Schade ({damage}), HP nu {currentHP}/{maxHP}");
        OnDamageTaken(damage); // Roep schade-effect aan
        if (currentHP <= 0)
        {
            Die();
        }
    } // Einde TakeDamage

    // Gedrag bij dood
    protected virtual void Die()
    {
        Debug.Log($"[{gameObject.name}] BaseClass: Object vernietigd (HP = 0)");
        Destroy(gameObject); // Standaard: vernietig object
    } // Einde Die
}