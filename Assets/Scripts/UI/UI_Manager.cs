using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MagicPigGames;

public class UI_Manager : MonoBehaviour
{
    public TMP_Text CurrentAmmo_Text;
    public TMP_Text MaxAmmo_Text;
    public ProgressBar healthBar;

    public TMP_Text PlaceCharge_Text;

    public TMP_Text CurrentWeapon_Text;

    private GameObject player;
    private Health playerHealth;


    private RangedWeapon weapon;
    private ArsenalController arsenal;
    private Health health;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        arsenal = player.GetComponent<ArsenalController>();
        health = player.GetComponent<Health>();
    }

    // Update is called once per frame

    void Update()
    {
        /* Gets the weapon player is using */
        if (ArsenalController.activeWeapon != null) // In case player hasn't picked up weapon
        {
            weapon = (RangedWeapon)ArsenalController.activeWeapon;
            CurrentWeapon_Text.text = weapon.gameObject.name;
            CurrentAmmo_Text.text = weapon.AmmoLoaded.ToString();
            MaxAmmo_Text.text = weapon.AmmoPool.ToString();
        }
        healthBar.SetProgress(playerHealth.GetHealth()*0.01f);
    }

    public void ActivatePlaceChargeText()
    {
        //PlaceCharge_Text.gameObject.SetActive(true);
    }

    public void DeactivatePlaceChargeText()
    {
        //PlaceCharge_Text.gameObject.SetActive(false);
    }
}
