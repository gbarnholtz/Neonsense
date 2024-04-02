using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        /* If Arena gameobject has no children, 
         * then all enemies in arena are dead*/
        if (transform.childCount == 1)
        {
            Debug.Log("All enemies in first arena are dead");
            /* Call triggerAllEnemiesKilled */
            GameObject.Find("TriggerNextLevel").SetActive(true);
            Destroy(gameObject);
            // set bool to true
        }
    }
}
