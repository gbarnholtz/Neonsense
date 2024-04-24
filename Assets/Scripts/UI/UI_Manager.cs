using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MagicPigGames;
using Unity.VisualScripting;
using System.Drawing;

public class UI_Manager : MonoBehaviour
{
    public TMP_Text CurrentAmmo_Text;
    public TMP_Text MaxAmmo_Text;
    public ProgressBar healthBar;

    public TMP_Text PlaceCharge_Text;

    public TMP_Text CurrentWeapon_Text;
    public GameObject RevolverIcon;
    public GameObject ShotgunIcon;
    public GameObject SMGIcon;

    private GameObject player;
    private Health playerHealth;

    public GameObject hitMarker;

    private RangedWeapon weapon;
    private ArsenalController arsenal;
    private Health health;

    [SerializeField] private GameObject damageOverlay;
    private Image damage_image;

    [SerializeField] private GameObject reload_popup;
    [SerializeField] private float reload_popup_percentage;

    [SerializeField] private GameObject low_ammo_popup;

    /* Singleton pattern */
    private static UI_Manager instance;
    public static UI_Manager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        arsenal = player.GetComponent<ArsenalController>();
        health = player.GetComponent<Health>();

        damage_image = damageOverlay.GetComponent<Image>();

        /* Makes damage overlay transparent */
        Vector4 color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        damage_image.color = color;


        instance = this;
    }

    // Update is called once per frame

    void Update()
    {
        /* Gets the weapon player is using */
        if (ArsenalController.activeWeapon != null) // In case player hasn't picked up weapon
        {
            weapon = (RangedWeapon)ArsenalController.activeWeapon;
            CurrentWeapon_Text.text = weapon.gameObject.name;
            if (weapon.gameObject.name.Equals("Pistol") || weapon.gameObject.name.Equals("Revolver"))
            {
                ShotgunIcon.SetActive(false);
                SMGIcon.SetActive(false);
                RevolverIcon.SetActive(true);
            } else if (weapon.gameObject.name.Equals("Shotgun"))
            {
                RevolverIcon.SetActive(false);
                SMGIcon.SetActive(false);
                ShotgunIcon.SetActive(true);
            } else if (weapon.gameObject.name.Equals("SMG"))
            {
                ShotgunIcon.SetActive(false);
                RevolverIcon.SetActive(false);
                SMGIcon.SetActive(true);
            } else
            {
                ShotgunIcon.SetActive(false);
                RevolverIcon.SetActive(false);
                SMGIcon.SetActive(false);
            }
            CurrentAmmo_Text.text = weapon.AmmoLoaded.ToString();
            MaxAmmo_Text.text = weapon.AmmoPool.ToString();
            CheckReloadPopup(weapon);
            CheckLowAmmoPopup(weapon);
        }
        healthBar.SetProgress(playerHealth.GetHealth()*0.01f);

        DamageOverlay();
    }

    private void CheckLowAmmoPopup(RangedWeapon weapon)
    {
        if ((float)weapon.AmmoPool / (float)weapon.maxAmmo < reload_popup_percentage)
        {
            low_ammo_popup.SetActive(true);
        }
        else
        {
            low_ammo_popup.SetActive(false);
        }
    }

    private void CheckReloadPopup(RangedWeapon weapon)
    {
        if ((float)weapon.AmmoLoaded / (float)weapon.MagazineSize < reload_popup_percentage)
        {
            reload_popup.SetActive(true);
        } else
        {
            reload_popup.SetActive(false);
        }
    }

    /* Updates damage overlay based on player hp */
    private void DamageOverlay()
    {
        damage_image.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f - (playerHealth.Current / playerHealth.Max));
    }

    private bool IsCoroutineActive = false;

    public void EnableHitMarker(float hurtTime)
    {
        if (!IsCoroutineActive)
        {
            StartCoroutine(HitMarter(hurtTime));
        }
    }

    IEnumerator HitMarter(float hurtTime)
    {
        IsCoroutineActive = true;
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(hurtTime);
        hitMarker.SetActive(false);
        IsCoroutineActive = false;
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
