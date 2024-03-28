using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitchController : MonoBehaviour
{

    ArsenalController arsenal;

    private void Awake()
    {
        arsenal = GetComponent<ArsenalController>();

        PlayerInputSO.switch2Pistol.performed += switchToPistolAction;
        PlayerInputSO.switch2Shotgun.performed += switchToShotgunAction;
        PlayerInputSO.switch2Rifle.performed += switchToRifleAction;
        PlayerInputSO.switch2SMG.performed += switchToSMGAction;
        PlayerInputSO.switch2Pistol.Enable();
        PlayerInputSO.switch2Shotgun.Enable();
        PlayerInputSO.switch2Rifle.Enable();
        PlayerInputSO.switch2SMG.Enable();
    }
    void switchToPistolAction(InputAction.CallbackContext obj)
    {
        switchWeapon("pistol");
    }

    void switchToShotgunAction(InputAction.CallbackContext obj)
    {
        switchWeapon("shotgun");
    }
    void switchToRifleAction(InputAction.CallbackContext obj)
    {
        switchWeapon("rifle");
    }

    void switchToSMGAction(InputAction.CallbackContext obj)
    {
        switchWeapon("smg");
    }

    /* Switches weapons based on string passed*/
    void switchWeapon(string weapon)
    {
        disableWeapon();

        switch (weapon)
        {
            case "pistol":
                arsenal.activeWeapon = arsenal.pistol;
                arsenal.OnEnable();
                arsenal.pistol.gameObject.SetActive(true);
                break;
            case "shotgun":
                arsenal.activeWeapon = arsenal.shotgun;
                arsenal.OnEnable();
                arsenal.shotgun.gameObject.SetActive(true);
                break;
            case "rifle":
                arsenal.activeWeapon = arsenal.assault_rifle;
                arsenal.OnEnable();
                arsenal.assault_rifle.gameObject.SetActive(true);
                break;
            case "smg":
                arsenal.activeWeapon = arsenal.smg;
                arsenal.OnEnable();
                arsenal.smg.gameObject.SetActive(true);
                break;
            default:
                // code block
                break;
        }
    }

    /* Disables current weapon */
    void disableWeapon()
    {
        arsenal.activeWeapon.IsAttacking = false;
        ((RangedWeapon)arsenal.activeWeapon).isPastFireRate = true; 
        arsenal.OnDisable();
        arsenal.activeWeapon.gameObject.SetActive(false);
    }
}
