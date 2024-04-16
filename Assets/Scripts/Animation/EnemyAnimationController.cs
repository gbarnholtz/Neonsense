using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;


    [SerializeField] private float BlendSpeed;
    float y_amount, x_amount;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        animator.SetFloat("MovementY", 0.0f);
        animator.SetFloat("MovementX", 0.0f);
    }

    private void Update()
    {
        /* If enemy is strafing left (player pov) */

        /* If enemy is strafing right (player pov) */

        /* If enemy is reloading (move back) */

        /* If enemy is heading towards player */
        BlendX(0.0f);
        BlendY(1.0f);

        SetMovement();
    }

    private void BlendX(float blendTo)
    {
        if (x_amount < blendTo) x_amount += BlendSpeed * Time.deltaTime;
        if (x_amount > blendTo) x_amount -= BlendSpeed * Time.deltaTime;
    }
    private void BlendY(float blendTo)
    {
        if (y_amount < blendTo) y_amount += BlendSpeed * Time.deltaTime;
        if (y_amount > blendTo) y_amount -= BlendSpeed * Time.deltaTime;
    }

    private void SetMovement()
    {
        /* Makes sure amounts are with [0,1] */
        if (y_amount > 1) y_amount = 1;
        if (x_amount > 1) x_amount = 1;
        if (y_amount < 0) y_amount = 0;
        if (x_amount < 0) x_amount = 0;

        animator.SetFloat("MovementY", y_amount);
        animator.SetFloat("MovementX", x_amount);
    }
}
