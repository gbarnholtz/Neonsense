using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The checkpoint is set to the current trigger
public class CheckPoint : MonoBehaviour
{
    //public GameObject checkpoint;

    private void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Transform transform1 = transform;
            CheckPointManager.lastCheckPointPos = transform1.position;
            CheckPointManager.lastCheckPointRot = transform1.rotation;
        }
    }

}
