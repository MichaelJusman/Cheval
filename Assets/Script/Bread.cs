using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : GameBehaviour
{

    int maxHealth = 1;

    private void Start()
    {
        //Ignoring collision on the enemy layer
        Physics2D.IgnoreLayerCollision(3, 3);
    }

    public void Destroy()
    {
        maxHealth--;

        if (maxHealth <= 0)
            Die();

    }

    void Die()
    {
        Debug.Log(gameObject + "is Dead");
        Destroy(gameObject);
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
