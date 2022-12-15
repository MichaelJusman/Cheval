using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Cromerang : GameBehaviour
{
    // Start is called before the first frame update
    int maxHealth = 1;

    [Header("Boomerang Variables")]
    bool isBoomin;
    Vector2 startPos;
    GameObject player;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(3, 3);
        isBoomin = false;
        StartCoroutine(Boomerang());

    }

    private void Update()
    {
        transform.Rotate(0, Time.deltaTime * 500, 0);

        if(isBoomin)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 40);
        }

        if(!isBoomin)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, Time.deltaTime * 40);
        }

        if (!isBoomin && Vector2.Distance(startPos, transform.position) < 1.3f)
        {
            Die();
        }
    }

    IEnumerator Boomerang()
    {
        isBoomin = true;
        yield return new WaitForSeconds(2f);
        isBoomin = false;
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

    //public IEnumerator Boomerang()
    //{
    //    Transform startPos = _EM.breadSpawner[2];
    //    Transform endPos = _PC.transform;
        

    //    moveToPos = reverse ? startPos : endPos;
    //    reverse = !reverse;
    //    yield return null;
    //}
}
