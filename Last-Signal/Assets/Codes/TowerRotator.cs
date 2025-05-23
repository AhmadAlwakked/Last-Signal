using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRotator : MonoBehaviour
{
    public float speed;
    public float range; // Added missing range variable
    private Transform target; // Added target as a class field

    // Update is called once per frame
    void Update()
    {
        target = FindNearestEnemy();

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
        }
    }

    protected Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Fixed typo in GameObject
        Transform closest = null;
        float shortestDistance = Mathf.Infinity; // Fixed Math to Mathf

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= range)
            {
                shortestDistance = distance;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue; // Fixed typo in Color.blue and OnDrawGizmosSelected
        Gizmos.DrawWireSphere(transform.position, range); // Fixed method name and parameters
    }
}