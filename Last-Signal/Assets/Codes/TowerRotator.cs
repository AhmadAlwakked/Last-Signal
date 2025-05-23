using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        target = FindNearestEnemy();

        if(target !=null)
        {
            Vector3 direction =(target.position-transform.position).normalized;
            Quaternion lookRotation = Quateratnion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion
        }

    }
}
