using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public enum BreadType
    {
        Baguette,
        Croissant,
        Broiche
    }

    public Transform[] breadSpawner;    //spawn points for the bread projectile
    public GameObject[] breadType;      //contains all the different bread type
    public List<GameObject> bread;      //A list of all the bread in the scene

    public float spawnCount = 10f;

    public Vector2 positionToMoveTo;

    [Header("Baguette")]
    public float baguetteSpeed = 1000f;

    [Header("Croissant")]
    public float croissantSpeed = 1000f;
    //public float duration = 3f;
    //public Vector2 maxDistance = new Vector2(0, 30);
    //public Vector2 startDistance = new Vector2(0, 0);
    //public Transform startPos;         //Needed for repeat patrol movement
    //public Transform moveToPos;
    //Transform endPos;           //Needed for repeat patrol movement
    //bool reverse = false;
    //float timeElapsed;
    //float lerpDuration = 3f;
    //Transform valueToLerp;

    private float startTime;

    [Header("Broiche")]
    public float broischeSpeed = 1000f;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FireBaguette();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            FireCroissant();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            FireBroische();
        }
    }

    void FireBaguette()
    {
        GameObject breadInstantiate = Instantiate(breadType[0], breadSpawner[0].position, breadSpawner[0].rotation);
        breadInstantiate.GetComponent<Rigidbody2D>().AddForce(breadSpawner[0].right * -baguetteSpeed);
    }

    void FireCroissant()
    {
        GameObject breadInstantiate = Instantiate(breadType[0], breadSpawner[0].position, breadSpawner[0].rotation);
        breadInstantiate.GetComponent<Rigidbody2D>().AddForce(breadSpawner[0].right * -baguetteSpeed);
    }

    //IEnumerator Croissant()
    //{
    //    float timeElapsed = 0;
    //    while (timeElapsed < lerpDuration)
    //    {
    //        GameObject breadInstantiate = Instantiate(breadType[0], breadSpawner[0].position, breadSpawner[0].rotation);
    //        breadInstantiate.transform.position.Lerp(startPos, moveToPos, timeElapsed / lerpDuration);
    //        valueToLerp = Vector2.Lerp(startPos, moveToPos, timeElapsed / lerpDuration);
    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    valueToLerp = moveToPos;
    //}

    void FireBroische()
    {
        GameObject breadInstantiate = Instantiate(breadType[1], breadSpawner[0].position, breadSpawner[0].rotation);
        breadInstantiate.GetComponent<Rigidbody2D>().AddForce((_PC.transform.position - breadSpawner[0].position) * broischeSpeed);
    }

    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

}
