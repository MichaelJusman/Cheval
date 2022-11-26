using System.Collections;
using System.Collections.Generic;
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

    [Header("Baguette")]
    public float baguetteSpeed = 1000f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            FireBaguette();
        }
    }

    void FireBaguette()
    {
        GameObject breadInstantiate = Instantiate(breadType[0], breadSpawner[0].position, breadSpawner[0].rotation);
        breadInstantiate.GetComponent<Rigidbody2D>().AddForce(breadSpawner[0].right * -baguetteSpeed);
    }


}
