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
    public TMP_Text CurrentWeapon_Text;
    public TMP_Text CurrentHealth_Text;
    public TMP_Text MaxHealth_Text;

    private RangedWeapon weapon;
    private ArsenalController arsenal;
    private Health health;
    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        arsenal = player.GetComponent<ArsenalController>();
        health = player.GetComponent<Health>();
    }

    // Update is called once per frame

    void Update()
    {
        //CurrentHealth_Text.text = health.Current.ToString();
        //MaxHealth_Text.text = health.Max.ToString();
        /* Gets the weapon player is using */
        weapon = (RangedWeapon) arsenal.activeWeapon;
        CurrentWeapon_Text.text = weapon.gameObject.name;
        CurrentAmmo_Text.text = weapon.AmmoLoaded.ToString();
        MaxAmmo_Text.text = weapon.MagazineSize.ToString();
    }
}
