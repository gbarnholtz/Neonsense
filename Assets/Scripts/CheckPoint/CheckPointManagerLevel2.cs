using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPointManagerLevel2 : MonoBehaviour
{
    public static Vector3 lastCheckPointPos = new Vector3(0f, -3f, -132f);
    //[SerializeField] public Vector3 lastCheckPointPos;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = lastCheckPointPos;
    }

}
