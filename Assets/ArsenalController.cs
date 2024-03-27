using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArsenalController : SerializedMonoBehaviour
{
    [OdinSerialize] private IInputProvider inputProvider;
    [SerializeField] private IWeapon activeWeapon;

    [SerializeField] private IWeapon pistol;
    [SerializeField] private IWeapon shotgun;
    [SerializeField] private IWeapon assault_rifle;
    [SerializeField] private IWeapon smg;

    public void OnEnable()
    {
        activeWeapon.Subscribe(inputProvider.Primary);
    }
    public void OnDisable()
    {
        activeWeapon.Unsubscribe(inputProvider.Primary);
    }

    private void Awake()
    {
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

    /* TODO: Remove this method when proper switching of weapons is implemented in PlayerSO.cs */
    void switchWeapon(string weapon)
    {
        if (!activeWeapon.IsAttacking) {
            activeWeapon.Unsubscribe(inputProvider.Primary);
            activeWeapon.gameObject.SetActive(false);
            if (weapon == "pistol")
            {
                activeWeapon = pistol;
                activeWeapon.Subscribe(inputProvider.Primary);
                pistol.gameObject.SetActive(true);
            }
            else if (weapon == "shotgun")
            {
                activeWeapon = shotgun;
                activeWeapon.Subscribe(inputProvider.Primary);
                shotgun.gameObject.SetActive(true);
            }
            else if (weapon == "rifle")
            {
                activeWeapon = assault_rifle;
                activeWeapon.Subscribe(inputProvider.Primary);
                assault_rifle.gameObject.SetActive(true);
            }
            else if (weapon == "smg")
            {
                activeWeapon = smg;
                activeWeapon.Subscribe(inputProvider.Primary);
                smg.gameObject.SetActive(true);
            }
        }
    }
}
