using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : MonoBehaviour
{
    protected float damage = 10f;
    protected float range = 5f;
    protected float fireRate = 1f; // Schoten per seconde
    protected float fireCooldown = 0f;

    // Virtuele methode voor initialisatie
    public virtual void Initialize(float dmg, float rng, float fr)
    {
        damage = dmg;
        range = rng;
        fireRate = fr;
        Debug.Log($"Tower initialized with damage: {damage}, range: {range}, fireRate: {fireRate}");
    }

    // Virtuele methode voor de aanval
    public virtual void Attack()
    {
        Debug.Log($"Base Tower attacks with damage: {damage} within range: {range}");
    }

    // Virtuele update methode voor vuursnelheid
    protected virtual void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Attack();
            fireCooldown = 1f / fireRate;
        }
    }
}