using System;
using UnityEngine;

public class StartArenaMusic : MonoBehaviour
{
    [SerializeField] private AudioSource levelMusic;
    [SerializeField] private AudioSource arenaMusic;
    //[SerializeField][Range(0,1)] private float arenaMusicVolume = 0.7f;
    private BoxCollider _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            levelMusic.Pause();
            arenaMusic.Play();

            enabled = false;
            _boxCollider.enabled = false;
        }
    }
}
