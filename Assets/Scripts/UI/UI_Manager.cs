using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public TMP_Text CurrentAmmo_Text;
    public TMP_Text MaxAmmo_Text;
    public TMP_Text HP_Text;

    private GameObject player;
    private Health playerHealth;

    private RangedWeapon weapon;
    private ArsenalController arsenal;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        arsenal = player.GetComponent<ArsenalController>();
    }

    // Update is called once per frame

    void Update()
    {
        /* Gets the weapon player is using */
        weapon = (RangedWeapon) arsenal.activeWeapon;
        CurrentAmmo_Text.text = weapon.AmmoLoaded.ToString();
        MaxAmmo_Text.text = weapon.MagazineSize.ToString();
        HP_Text.text = playerHealth.GetHealth().ToString();    
    }
}
