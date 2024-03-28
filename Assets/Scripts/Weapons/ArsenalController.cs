using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArsenalController : SerializedMonoBehaviour
{
    [OdinSerialize] private IInputProvider inputProvider;
    [SerializeField] public IWeapon activeWeapon;

    [SerializeField] public IWeapon pistol;
    [SerializeField] public IWeapon shotgun;
    [SerializeField] public IWeapon assault_rifle;
    [SerializeField] public IWeapon smg;

    public void OnEnable()
    {
        activeWeapon.Subscribe(inputProvider.Primary);
    }
    public void OnDisable()
    {
        activeWeapon.Unsubscribe(inputProvider.Primary);
    }
}
