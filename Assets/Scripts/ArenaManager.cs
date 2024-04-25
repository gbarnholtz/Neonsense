using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : SerializedMonoBehaviour
{
    public readonly GameObject ElevatorTrigger;
    public readonly Animator ElevatorDoors;
    public readonly GameObject RemoveBarriers;

    [SerializeField] private AudioSource levelMusic;
    [SerializeField] private AudioSource arenaMusic;

    private void Start()
    {
        if (ElevatorTrigger != null)
            ElevatorTrigger.SetActive(false);
        if (ElevatorDoors != null)
            ElevatorDoors.enabled = false;
    }

    private bool enemiesAreDefeated = false;

    private void Update()
    {
        if (!enemiesAreDefeated)
        {
            CheckIfEnemiesDefeated();
        }
    }

    public void CheckIfEnemiesDefeated()
    {
        /* If Arena gameobject has one child,
         * that child was just killed,
         * so all enemies in arena are dead*/
        if (transform.childCount <= 1)
        {
            enemiesAreDefeated = true;
            Debug.Log("All enemies in the arena are dead");
            if (ElevatorTrigger != null)
                ElevatorTrigger.SetActive(true);
            if (ElevatorDoors != null)
                ElevatorDoors.enabled = true;
            if (RemoveBarriers != null)
                RemoveBarriers.SetActive(false);
           
            arenaMusic.Stop();
            levelMusic.UnPause();

            Destroy(gameObject);
        }
    }
}
