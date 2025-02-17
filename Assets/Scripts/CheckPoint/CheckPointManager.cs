using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static Vector3 lastCheckPointPos = new Vector3(7.75f, -2f, -34f);
    public static Quaternion lastCheckPointRot = quaternion.identity;
    //[SerializeField] public Vector3 lastCheckPointPos;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = lastCheckPointPos;
        GameObject.FindGameObjectWithTag("Player").transform.rotation = lastCheckPointRot;
    }

}
