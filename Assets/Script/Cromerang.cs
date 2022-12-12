using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cromerang : GameBehaviour
{
    // Start is called before the first frame update
    int maxHealth = 1;

    private void Start()
    {
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
            _EM.StopCromerang();
            Debug.Log("I hit you");
            Destroy(gameObject);
            
        }

        _EM.StopCromerang();
        Destroy(gameObject);
    }
}
