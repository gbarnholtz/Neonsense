using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : SerializedMonoBehaviour
{
    public readonly GameObject ElevatorTrigger;
    public readonly Animator ElevatorDoors;
    public readonly GameObject RemoveBarriers;


    private void Start()
    {
        if (ElevatorTrigger != null)
            ElevatorTrigger.SetActive(false);
        if (ElevatorDoors != null)
            ElevatorDoors.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        /* If Arena gameobject has no children, 
         * then all enemies in arena are dead*/
        if (transform.childCount == 0)
        {
            Debug.Log("All enemies in the arena are dead");
            if (ElevatorTrigger != null)
                ElevatorTrigger.SetActive(true);
            if (ElevatorDoors != null)
                ElevatorDoors.enabled = true;
            if (RemoveBarriers != null)
                RemoveBarriers.SetActive(false);

            Destroy(gameObject);
        }
    }
}
