using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Tilemaps;
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
    //public float duration = 3f;
    //public Vector2 maxDistance = new Vector2(0, 30);
    //public Vector2 startDistance = new Vector2(0, 0);
    //public Transform startPos;         //Needed for repeat patrol movement
    //public Transform moveToPos;
    //Transform endPos;           //Needed for repeat patrol movement
    //bool reverse = false;
    //float timeElapsed;
    //float lerpDuration = 3f;
    Transform valueToLerp;
    Transform moveToPos;

    private float startTime;


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
        if (Input.GetKeyDown(KeyCode.P))
        {
            FireBaguette();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(Slices());
            //FireCroissant();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            FireBroische();
        }

        if(Input.GetKeyDown(KeyCode.C))
            StartCoroutine(Cromerang2());

        //if (Input.GetKeyDown(KeyCode.K))
        //    StartCoroutine(Croissant());

        if (Input.GetKeyDown(KeyCode.B))
        {
            Boomerang();
            
        }
    }

    void FireBaguette()
    {
        GameObject breadInstantiate = Instantiate(breadType[UnityEngine.Random.Range(0, breadType.Length)], breadSpawner[UnityEngine.Random.Range(0, breadSpawner.Length)].position, breadSpawner[UnityEngine.Random.Range(0, breadSpawner.Length)].rotation);
        breadInstantiate.GetComponent<Rigidbody2D>().AddForce((_PC.transform.position - breadSpawner[UnityEngine.Random.Range(0, breadSpawner.Length)].position) * baguetteSpeed);
    }

    //void FireCroissant()
    //{
    //    GameObject breadInstantiate = Instantiate(breadType[0], breadSpawner[0].position, breadSpawner[0].rotation);
    //    breadInstantiate.GetComponent<Rigidbody2D>().AddForce(breadSpawner[0].right * -baguetteSpeed);
    //}

    //IEnumerator Cromerang()
    //{
    //    float timeElapsed = 0;
    //    GameObject cromerangInstantiate = Instantiate(croissant[0], breadSpawner[2].position, breadSpawner[2].rotation);
    //    Vector2 originPoint = breadSpawner[2].position;
    //    while (timeElapsed < cromerangTime)
    //        {
          
    //            cromerangInstantiate.transform.position = Vector2.Lerp(originPoint, _PC.transform.position, timeElapsed / cromerangTime);
    //            timeElapsed += Time.deltaTime;
    //            yield return null;
    //        }
    //    valueToLerp = _PC.transform;
    //}

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

    public IEnumerator Cromerang2()
    {
        Vector3 endPos =  _PC.transform.position;
        GameObject cromerangInstantiate = Instantiate(croissant[0], breadSpawner[2].position, breadSpawner[2].rotation);
        Vector3 originPoint = breadSpawner[2].transform.position;
        while (Vector3.Distance(originPoint, endPos) > 0.3f)
        {
            Debug.Log("CROOMERINGTIME");
            cromerangInstantiate.transform.position = Vector3.MoveTowards(cromerangInstantiate.transform.position, endPos, Time.deltaTime * croissantSpeed);

            //moveToPos = reverse ? originPoint : endPos;
            //reverse = !reverse;
            //break;

            yield return null;
        }
    }

    public void Boomerang()
    {
        GameObject cromerangInstantiate = Instantiate(croissant[0], breadSpawner[2].position, breadSpawner[2].rotation);
    }

    public void StopCromerang()
    {
        StopCoroutine(Cromerang2());
    }

    //IEnumerator Croissant()
    //{
    //    float timeElapsed = 0;
    //    Vector2 originPoint = breadSpawner[2].position;
    //    while (timeElapsed < cromerangTime)
    //    {
    //        //croissant = Instantiate(croissant, breadSpawner[2].position, breadSpawner[2].rotation);
    //        //croissant.transform.position = Vector2.Lerp(originPoint, _PC.transform.position, timeElapsed / cromerangTime);
    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    valueToLerp = _PC.transform;
    //}


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

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void EndGame()
    {
        audioSource.Stop();
        _UI.ActivateWinPanel();
        StopAllCoroutines();
    }

}
