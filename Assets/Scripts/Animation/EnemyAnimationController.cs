using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;

    private Vector3 position;

    float lastX, lastZ;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        position = gameObject.transform.parent.position;



        animator.SetFloat("MovementY", 1.0f);
        animator.SetFloat("MovementX", 0.0f);
    }

    private void Update()
    {
    }
}
