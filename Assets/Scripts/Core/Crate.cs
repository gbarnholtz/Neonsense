using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.tag == "ammo_pickup")
            {
                ((RangedWeapon)ArsenalController.activeWeapon).AmmoPool += 100;
                Destroy(gameObject);    
            }
            if (gameObject.tag == "health_pickup")
            {
                other.gameObject.GetComponent<Health>().AddToHealth(50f);
                Destroy(gameObject);
            }
        }
    }
}
