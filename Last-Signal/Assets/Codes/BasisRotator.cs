using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasisTower : MonoBehaviour
{
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float range = 5f;
    [SerializeField] protected float fireRate = 1f; // Shots per second
    protected float fireCooldown = 0f;

    // Virtual method for initialization
    public virtual void Initialize(float dmg, float rng, float fr)
    {
        damage = dmg;
        range = rng;
        fireRate = Mathf.Max(fr, 0.1f); // Prevent division by zero or negative fire rate
        Debug.Log($"Tower initialized with damage: {damage}, range: {range}, fireRate: {fireRate}");
    }

    // Virtual method for attack
    protected virtual void Attack()
    {
        Debug.Log($"Base Tower attacks with damage: {damage} within range: {range}");
        // Placeholder for actual attack logic (e.g., finding and damaging an enemy)
    }

    // Virtual update method for fire rate
    protected virtual void Update()
    {
        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }
        else
        {
            Attack();
            fireCooldown = 1f / fireRate;
        }
    }

    // Draw the range in the Unity Editor for visualization
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Visual distinction from TowerRotator
        Gizmos.DrawWireSphere(transform.position, range);
    }
}