using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The checkpoint is set to the current trigger
public class CheckPoint : MonoBehaviour
{
    //public GameObject checkpoint;

    private void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            CheckPointManager.lastCheckPointPos = transform.position;
        }
    }

}
