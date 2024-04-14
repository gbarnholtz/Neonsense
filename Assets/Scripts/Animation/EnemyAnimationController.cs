using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float x = animator.GetFloat("MovementX");
        float y = animator.GetFloat("MovementY");
        animator.SetFloat("MovementY", 1.0f);
        animator.SetFloat("MovementX", 0.0f);
    }
}
