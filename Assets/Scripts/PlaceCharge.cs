using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCharge : MonoBehaviour
{
    [SerializeField] private GameObject Bomb;
    private AudioSource audioSource;
    private bool hasChargeBeenPlaced;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        hasChargeBeenPlaced = false;
        Bomb.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasChargeBeenPlaced)
        {
            hasChargeBeenPlaced = true;
            audioSource.Play();
            Bomb.SetActive(true);
            UI_Manager.chargesPlaced++;
        }
    }
}
