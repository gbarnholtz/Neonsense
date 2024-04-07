using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The checkpoint is set to the current trigger
public class CheckPointLevel2 : MonoBehaviour
{
    //public GameObject checkpoint;

    private void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CheckPointManagerLevel2.lastCheckPointPos = transform.position;
        }
    }

}
