using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArsenalController : SerializedMonoBehaviour
{
    [OdinSerialize] private IInputProvider inputProvider;
    [SerializeField] public static IWeapon activeWeapon;

    [SerializeField] public IWeapon pistol;
    [SerializeField] public IWeapon shotgun;
    [SerializeField] public IWeapon assault_rifle;
    [SerializeField] public IWeapon smg;

    SwitchWeaponController switchController;

    /* Variables are static because we want them to survive between scenes and player death */
    public static bool PistolPickedUp = false;
    public static bool ShotgunPickedUp = false;
    public static bool RiflePickedUp = false;
    public static bool SMGPickedUp = false;

    private void Start()
    {
        switchController = gameObject.GetComponent<SwitchWeaponController>();

        if (PistolPickedUp)
            switchController.switchWeapon("pistol");
    }

    [Button("PickupWeapon")]
    public void PickupWeapon(string weapon)
    {
        if (weapon == "pistol") PistolPickedUp = true;
        if (weapon == "shotgun") ShotgunPickedUp = true;
        if (weapon == "rifle") RiflePickedUp = true;
        if (weapon == "smg") SMGPickedUp = true;

        /* Set active weapon to the weapon that was just picked up */
        switchController.switchWeapon(weapon);
    }

    public void OnEnable()
    {
        if (activeWeapon != null)
            activeWeapon.Subscribe(inputProvider.Primary);
    }
    public void OnDisable()
    {
        if (activeWeapon != null)
            activeWeapon.Unsubscribe(inputProvider.Primary);
    }
}
