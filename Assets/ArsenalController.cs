using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalController : SerializedMonoBehaviour
{
    [OdinSerialize] private IInputProvider inputProvider;
    [SerializeField] private IWeapon activeWeapon;

    public void OnEnable()
    {
        activeWeapon.Subscribe(inputProvider.Primary);
    }
    public void OnDisable()
    {
        activeWeapon.Unsubscribe(inputProvider.Primary);
    }
}
