using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseClass : MonoBehaviour
{

    public float maxHealth = 100;
    public float currentHealth;


    virtual public void Start()
    {
        currentHealth = maxHealth;
    }

    virtual public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        
        
        
        //if (currentHealth <= 0)
        //{
        //    Destroy(this.gameObject);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
