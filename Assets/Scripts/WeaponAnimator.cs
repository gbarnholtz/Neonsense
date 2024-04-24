using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator), typeof(RangedWeapon))]
public class WeaponAnimator : MonoBehaviour
{
    Animator animator;
    RangedWeapon weapon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weapon = GetComponent<RangedWeapon>();
    }

    private void OnEnable()
    {
        weapon.Fired += TriggerShoot;
        weapon.Reloaded += TriggerReload;
    }

    private void OnDisable()
    {
        weapon.Fired -= TriggerShoot;
        weapon.Reloaded -= TriggerReload;
    }

    void TriggerReload() {
        animator.SetTrigger("Reload");
        Debug.Log("reload");
    }

    void TriggerShoot()
    {
        animator.SetTrigger("Shoot");
        Debug.Log("shoot");
    }
}
