using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Teleports player to given destination
public class tr_teleport : MonoBehaviour
{
    //[SerializeField] private GameObject player;
    [SerializeField] private Transform destination;
    [SerializeField] private AudioSource sound;
    
    [SerializeField] private AudioSource levelMusic;
    [SerializeField] private AudioSource arenaMusic;
    
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (destination != null)
            {
                col.transform.position = destination.position;
                if (sound != null)
                {
                    sound.Play();
                }
                if (levelMusic != null && arenaMusic != null)
                {
                    levelMusic.Pause();
                    arenaMusic.Play();
                }
            }
            else
            {
                Debug.LogWarning("Teleport destination transform is missing!");
            }
        }
    }
}
