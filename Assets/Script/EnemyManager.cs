using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public enum BreadType
    {
        Baguette,
        Croissant,
        Broiche,
        Pain
    }

    public Transform[] breadSpawner;    //spawn points for the bread projectile
    public GameObject[] breadType;      //contains all the different bread type
    public List<GameObject> bread;      //A list of all the bread in the scene

    public GameObject[] croissant;

    public float spawnCount = 10f;

    public Vector2 positionToMoveTo;

    [Header("Baguette")]
    public float baguetteSpeed = 1000f;

    [Header("Croissant")]
    public float croissantSpeed = 1000f;
    public float cromerangTime = 5f;


    [Header("Broiche")]
    public float broischeSpeed = 300f;
    public float slicingTime = 2;

    Animator anim;

    public AudioSource audioSource;

    private void Start()
    {
        audioSource.Play();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            EndGame();

        }
    }

    //Instantiate a random projectile and aim it towards the general direction of the player. This function is called through the animator
    void FireBaguette()
    {
        GameObject breadInstantiate = Instantiate(breadType[UnityEngine.Random.Range(0, breadType.Length)], breadSpawner[UnityEngine.Random.Range(0, breadSpawner.Length)].position, breadSpawner[UnityEngine.Random.Range(0, breadSpawner.Length)].rotation);
        breadInstantiate.GetComponent<Rigidbody2D>().AddForce((_PC.transform.position - breadSpawner[UnityEngine.Random.Range(0, breadSpawner.Length)].position) * baguetteSpeed);
    }

    //Instantiate bread slices at a fast rate. This function is called through the animator
    public IEnumerator Slices()
    {
        float timeElapsed = 0;
        while (timeElapsed < slicingTime)
        {
            timeElapsed += Time.deltaTime;
            GameObject sliceInstantiate = Instantiate(breadType[1], breadSpawner[Random.Range(0, breadSpawner.Length)].position, breadSpawner[Random.Range(0, breadSpawner.Length)].rotation);
            sliceInstantiate.GetComponent<Rigidbody2D>().AddForce((_PC.transform.position - breadSpawner[Random.Range(0, breadSpawner.Length)].position) * broischeSpeed);
            yield return new WaitForSeconds(0.4f);
        }
        yield break;
    }

    //Old unuse script to make a boomerang fucntion, it kills the player instantly
    public IEnumerator Cromerang2()
    {
        Vector3 endPos =  _PC.transform.position;
        GameObject cromerangInstantiate = Instantiate(croissant[0], breadSpawner[2].position, breadSpawner[2].rotation);
        Vector3 originPoint = breadSpawner[2].transform.position;
        while (Vector3.Distance(originPoint, endPos) > 0.3f)
        {
            Debug.Log("CROOMERINGTIME");
            cromerangInstantiate.transform.position = Vector3.MoveTowards(cromerangInstantiate.transform.position, endPos, Time.deltaTime * croissantSpeed);

            yield return null;
        }
    }

    //Instantiate a boomerang into the scene. The movement is controlled by its own script
    public void Boomerang()
    {
        GameObject cromerangInstantiate = Instantiate(croissant[0], breadSpawner[2].position, breadSpawner[2].rotation);
    }

    //old unused script
    public void StopCromerang()
    {
        StopCoroutine(Cromerang2());
    }

    //another old script for the boomerang fucntion
    void Cromerang1()
    {
        float timeElapsed = 0;
        while (timeElapsed < cromerangTime)
        {
            GameObject cromerangInstantiate = Instantiate(breadType[1], breadSpawner[2].position, breadSpawner[2].rotation);
            Vector2 originPoint = breadSpawner[2].position;
            cromerangInstantiate.transform.position = Vector2.Lerp(originPoint, _PC.transform.position, timeElapsed / cromerangTime);
            timeElapsed += Time.deltaTime;
            return;
        }
    }

    //Unused because i decided that instead of having to decide what bread spawn each time, i wanted it to be randomized except for a few types
    void FireBroische()
    {
        GameObject breadInstantiate = Instantiate(breadType[1], breadSpawner[0].position, breadSpawner[0].rotation);
        breadInstantiate.GetComponent<Rigidbody2D>().AddForce((_PC.transform.position - breadSpawner[0].position) * broischeSpeed);
    }


    public void StopMusic()
    {
        audioSource.Stop();
    }

    //Ends the game, gets called at the end of the animation
    public void EndGame()
    {
        audioSource.Stop();
        _UI.ActivateWinPanel();
        StopAllCoroutines();
        Destroy(gameObject);
    }

}
