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
        PlayerInputSO.switch2Pistol.Enable();
        PlayerInputSO.switch2Shotgun.Enable();
    }

    void switchToPistolAction(InputAction.CallbackContext obj)
    {
        switchWeapon("pistol");
    }

    void switchToShotgunAction(InputAction.CallbackContext obj)
    {
        switchWeapon("shotgun");
    }

    /* TODO: Remove this method when proper switching of weapons is implemented in PlayerSO.cs */
    void switchWeapon(string weapon)
    {
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
    }
}
