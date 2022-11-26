using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : GameBehaviour
{

    //public BreadType breadType;  <-- make EnemyManager a Singleton first!!!

    int maxHealth = 1;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(3, 3);
    }

    public void TakeDamage()
    {
        maxHealth--;

        if (maxHealth <= 0)
            Die();

    }

    void Die()
    {
        Debug.Log(gameObject + "is Dead");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("I hit you");
            Destroy(gameObject);
        }

        Destroy(gameObject);

        
    }





}
