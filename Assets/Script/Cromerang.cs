using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cromerang : GameBehaviour
{

    [Header("Boomerang Variables")]
    bool isBoomin;
    GameObject startPos;
    GameObject player;

    private void Start()
    {
        isBoomin = false;
        player = GameObject.Find("Player");
        startPos = GameObject.Find("hands2");
        StartCoroutine(Boomerang());
    }

    private void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * 100);

        if(isBoomin)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 20);
        }

        if(!isBoomin)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos.transform.position, Time.deltaTime * 20);
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(startPos.transform.position.x, transform.position.y), Time.deltaTime * 20);
        }

        if (!isBoomin && Vector2.Distance(startPos.transform.position, transform.position) < 1.3f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Boomerang()
    {
        isBoomin = true;
        yield return new WaitForSeconds(.3f);
        isBoomin = false;
    }
}
