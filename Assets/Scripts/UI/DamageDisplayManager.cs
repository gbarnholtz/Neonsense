using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDisplayManager : SerializedMonoBehaviour
{
    [SerializeField] private UI_DamageNumDisplay dmgDisplay;
    [SerializeField] private Transform healthLayer, dmgLayer;

    private Camera viewCamera;
    private void Awake()
    {
        GameEventManager.OnHit += UpdateOnHitDisplay;
    }

    private void Start()
    {
        viewCamera = GameStateManager.Instance.Camera;
    }

    private void OnDisable()
    {
        GameEventManager.OnHit -= UpdateOnHitDisplay;
    }

    private void UpdateOnHitDisplay(DamageInstance dmg, Transform obj)
    {
        UI_DamageNumDisplay dmgNumDisp = PooledObject.Create(dmgDisplay, obj.position, Quaternion.identity);

        dmgNumDisp.transform.SetParent(dmgLayer);
        dmgNumDisp.Bind(obj, dmg, viewCamera);
    }
}
