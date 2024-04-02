using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchWeaponController : MonoBehaviour
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

    /* Switches weapons based on string passed*/
    public void switchWeapon(string weapon)
    {
        if (arsenal.activeWeapon != null)
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

    /* ------- Methods below are triggered when player presses their respective key ----------- */

    void switchToPistolAction(InputAction.CallbackContext obj)
    {
        /* Only switches weapon if player is not reloading */
        if (((RangedWeapon)arsenal.activeWeapon).AmmoLoaded > 0
            && ArsenalController.PistolPickedUp) // Checks if player has picked up weapon
        {
            switchWeapon("pistol");
        }
    }

    void switchToShotgunAction(InputAction.CallbackContext obj)
    {
        if (((RangedWeapon)arsenal.activeWeapon).AmmoLoaded > 0
            && ArsenalController.ShotgunPickedUp)
        {
            switchWeapon("shotgun");
        }
    }
    void switchToRifleAction(InputAction.CallbackContext obj)
    {
        if (((RangedWeapon)arsenal.activeWeapon).AmmoLoaded > 0
            && ArsenalController.RiflePickedUp)
        {
            switchWeapon("rifle");
        }
    }

    void switchToSMGAction(InputAction.CallbackContext obj)
    {
        if (((RangedWeapon)arsenal.activeWeapon).AmmoLoaded > 0
            && ArsenalController.SMGPickedUp)
        {
            switchWeapon("smg");
        }
    }
}
