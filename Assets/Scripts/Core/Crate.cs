using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{

    [SerializeField] public int AddToAmmo;
    [SerializeField] public float AddToHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.tag == "ammo_pickup")
            {
                ((RangedWeapon)ArsenalController.activeWeapon).AmmoPool += AddToAmmo;
                Destroy(gameObject);    
            }
            if (gameObject.tag == "health_pickup")
            {
                other.gameObject.GetComponent<Health>().AddToHealth(AddToHealth);
                Destroy(gameObject);
            }
        }
    }
}
