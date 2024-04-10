using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    [SerializeField] public int interactDistance;
    [SerializeField] public GameObject UI;

    private UI_Manager UI_manager;
    private GameObject player;
    private bool ChargePlaced;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        UI_manager = UI.GetComponent<UI_Manager>();

        ChargePlaced = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerInRange() && !ChargePlaced)
        {
            /* Show UI to place charge */
            UI_manager.ActivatePlaceChargeText();
            ChargePlaced = true;
        } else if (!IsPlayerInRange() && ChargePlaced)
        {
            ChargePlaced = false;
            UI_manager.DeactivatePlaceChargeText();
        }
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) < interactDistance;
    }
}
