using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject enemy;

    // Update is called once per frame
    void Update()
    {
        Health enemyHealth = enemy.GetComponent<Health>();
        healthSlider.value = enemyHealth.Current / enemyHealth.Max;
    }
}
