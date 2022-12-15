using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Cromerang : GameBehaviour
{
    // Start is called before the first frame update
    int maxHealth = 1;

    Transform moveToPos;
    bool reverse = false;

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
            //_EM.StopCromerang();
            Debug.Log("I hit you");
            Destroy(gameObject);
            
        }

        //_EM.StopCromerang();
        Destroy(gameObject);
    }

    public IEnumerator Boomerang()
    {
        Transform startPos = _EM.breadSpawner[2];
        Transform endPos = _PC.transform;
        

        moveToPos = reverse ? startPos : endPos;
        reverse = !reverse;
        yield return null;
    }
}
