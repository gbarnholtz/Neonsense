using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDTest : MonoBehaviour
{
    [SerializeField] private float P, I, D, saturation, power;

    [SerializeField] private Transform target;
    private PID controller;
    private PID y;
    private Rigidbody rb;

    void Start()
    {
        controller = new PID(P, I, D, saturation);
        y = new PID(P, I, D, saturation);
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 input = new Vector3(0,0,controller.Update(Time.fixedDeltaTime, rb.position.z, target.position.z));
        rb.AddForce(input * power, ForceMode.Acceleration);
    }
}
