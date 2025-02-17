using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchWeaponController : MonoBehaviour
{

    ArsenalController arsenal;
    
    [SerializeField] private AudioClip swapWeaponSound;
    [SerializeField] private AudioSource audioSource;

    private void reloadWeapon(InputAction.CallbackContext obj)
    {
        if (ArsenalController.activeWeapon == null) return;

        var weapon = (RangedWeapon)ArsenalController.activeWeapon;

        // Don't reload if
        if (weapon == null) return;
        if (weapon.IsReloading) return;
        if (weapon.AmmoLoaded == weapon.MagazineSize) return;
        if (weapon.description == "shotgun" && weapon.AmmoLoaded > 0) return;
        
        ArsenalController.activeWeapon.StopTryingToFire();
        ArsenalController.activeWeapon.StartCoroutine(((RangedWeapon)ArsenalController.activeWeapon).DelayedReload());
    }

    private void Start()
    {
        arsenal = GetComponent<ArsenalController>();

        PlayerInputSO.reload.performed += reloadWeapon;

        PlayerInputSO.switch2Pistol.performed += switchToPistolAction;
        PlayerInputSO.switch2Shotgun.performed += switchToShotgunAction;
        PlayerInputSO.switch2Rifle.performed += switchToRifleAction;
        PlayerInputSO.switch2SMG.performed += switchToSMGAction;
        PlayerInputSO.switch2Pistol.Enable();
        PlayerInputSO.switch2Shotgun.Enable();
        PlayerInputSO.switch2Rifle.Enable();
        PlayerInputSO.switch2SMG.Enable();
    }

    private void OnDisable()
    {
        PlayerInputSO.reload.performed -= reloadWeapon;
        
        PlayerInputSO.switch2Pistol.performed   -= switchToPistolAction;
        PlayerInputSO.switch2Shotgun.performed  -= switchToShotgunAction;
        PlayerInputSO.switch2Rifle.performed    -= switchToRifleAction;
        PlayerInputSO.switch2SMG.performed      -= switchToSMGAction;
    }

    /* Switches weapons based on string passed*/
    public void switchWeapon(string weapon)
    {
        if (ArsenalController.activeWeapon != null)
            disableWeapon();

        switch (weapon)
        {
            case "pistol":
                ArsenalController.activeWeapon = arsenal.pistol;
                arsenal.OnEnable();
                arsenal.pistol.gameObject.SetActive(true);
                audioSource.PlayOneShot(swapWeaponSound);
                break;
            case "shotgun":
                ArsenalController.activeWeapon = arsenal.shotgun;
                arsenal.OnEnable();
                arsenal.shotgun.gameObject.SetActive(true);
                audioSource.PlayOneShot(swapWeaponSound);
                break;
            case "rifle":
                ArsenalController.activeWeapon = arsenal.assault_rifle;
                arsenal.OnEnable();
                arsenal.assault_rifle.gameObject.SetActive(true);
                audioSource.PlayOneShot(swapWeaponSound);
                break;
            case "smg":
                ArsenalController.activeWeapon = arsenal.smg;
                arsenal.OnEnable();
                arsenal.smg.gameObject.SetActive(true);
                audioSource.PlayOneShot(swapWeaponSound);
                break;
            default:
                // code block
                break;
        }
    }

    /* Disables current weapon */
    void disableWeapon()
    {
        if (ArsenalController.activeWeapon == null) return;
        
        ArsenalController.activeWeapon.IsAttacking = false;
        ((RangedWeapon)ArsenalController.activeWeapon).isPastFireRate = true;
        arsenal.OnDisable();
        ArsenalController.activeWeapon.gameObject.SetActive(false);
    }

    /* ------- Methods below are triggered when player presses their respective key ----------- */

    void switchToPistolAction(InputAction.CallbackContext obj)
    {
        if (ArsenalController.activeWeapon == null) return;
        
        /* Only switches weapon if player is not reloading */
        if (((RangedWeapon)ArsenalController.activeWeapon).AmmoLoaded > 0
            && ArsenalController.PistolPickedUp) // Checks if player has picked up weapon
        {
            switchWeapon("pistol");
        }
    }

    void switchToShotgunAction(InputAction.CallbackContext obj)
    {
        if (ArsenalController.activeWeapon == null) return;
        if (((RangedWeapon)ArsenalController.activeWeapon).AmmoLoaded > 0
            && ArsenalController.ShotgunPickedUp)
        {
            switchWeapon("shotgun");
        }
    }
    void switchToRifleAction(InputAction.CallbackContext obj)
    {
        if (ArsenalController.activeWeapon == null) return;
        if (((RangedWeapon)ArsenalController.activeWeapon).AmmoLoaded > 0
            && ArsenalController.RiflePickedUp)
        {
            switchWeapon("rifle");
        }
    }

    void switchToSMGAction(InputAction.CallbackContext obj)
    {
        if (ArsenalController.activeWeapon == null) return;
        
        if (((RangedWeapon)ArsenalController.activeWeapon).AmmoLoaded > 0
            && ArsenalController.SMGPickedUp)
        {
            switchWeapon("smg");
        }
    }
}
