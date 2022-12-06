using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Hands : GameBehaviour
{
    public GameObject player;

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        transform.LookAt(_PC.transform.position, transform.up);
        transform.Rotate(new Vector3(0, 90, 0), Space.Self);
    }
}
