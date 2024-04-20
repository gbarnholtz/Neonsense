using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static Vector3 lastCheckPointPos = new Vector3(8.5f, -2f, -34f);
    //[SerializeField] public Vector3 lastCheckPointPos;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = lastCheckPointPos;
    }

}
