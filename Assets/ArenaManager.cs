using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : SerializedMonoBehaviour
{
    public readonly GameObject Elevator;
    public readonly GameObject RemoveBarriers;


    private void Start()
    {
        if (Elevator != null)
            Elevator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /* If Arena gameobject has no children, 
         * then all enemies in arena are dead*/
        if (transform.childCount == 0)
        {
            Debug.Log("All enemies in first arena are dead");
            if (Elevator != null)
                Elevator.SetActive(true);
            if (RemoveBarriers != null)
                RemoveBarriers.SetActive(false);

            Destroy(gameObject);
        }
    }
}
