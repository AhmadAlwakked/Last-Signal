using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLookAtEnemy : MonoBehaviour
{
    private Transform target; // De vijand waar de toren naar kijkt
    public float range = 10f; // Bereik van de toren
    public string enemyTag = "Enemy"; // Tag van de vijand
    public Transform partToRotate; // Optioneel: het deel van de toren dat roteert (bijv. de turret)

    void Start()
    {
        // Zoek elke 0.5 seconden naar een nieuwe vijand
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        // Vind alle gameobjects met de tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        // Zoek de dichtstbijzijnde vijand binnen bereik
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // Stel de target in als er een vijand is gevonden, anders geen target
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        // Als er een target is, laat de toren ernaar kijken
        if (target != null)
        {
            // Bereken de richting naar de vijand
            Vector3 dir = target.position - (partToRotate != null ? partToRotate.position : transform.position);
            // Maak een rotatie die naar de vijand kijkt
            Quaternion lookRotation = Quaternion.LookRotation(dir);

            // Gebruik SmoothDamp voor soepele rotatie (optioneel)
            Quaternion rotation = Quaternion.RotateTowards(
                partToRotate != null ? partToRotate.rotation : transform.rotation,
                lookRotation,
                Time.deltaTime * 360f // Rotatiesnelheid (graden per seconde)
            );

            // Pas de rotatie toe
            if (partToRotate != null)
            {
                partToRotate.rotation = rotation;
            }
            else
            {
                transform.rotation = rotation;
            }
        }
    }

    // Optioneel: Visualiseer het bereik in de editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}