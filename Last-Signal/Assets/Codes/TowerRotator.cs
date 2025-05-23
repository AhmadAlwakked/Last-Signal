using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRotator : MonoBehaviour
{

    public float speed;

    // Update is called once per frame
    void Update()
    {

        target = FindNearestEnemy();

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quateratnion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transfer.rotation, lookRotation, Time.deltaTime * speed)
        }
    }

    protected Transform FindNearestEnemy()
    {

        GameObject[] enemies = Gameobject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float shortestDistance = Math.Infinity;

        foreach(GameObject enemy in enemies)
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

        OnDrawGizmosSelected.color = Color.bleu
        OnDrawGizmosSelected(transform.postion, range);

    }

}
