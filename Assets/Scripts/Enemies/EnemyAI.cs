using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class EnemyAI : SerializedMonoBehaviour, ICharacterInputProvider
{
    [SerializeField] private float outerTarget, innerTarget;
    [SerializeField] private Vector3 targetDirection, randomizedVector;
    [SerializeField] private GameObject weapon;

    private InputState input = new InputState();
        
    [SerializeField] public GameObject player;

    public ButtonAction Jump => jump;
    private ButtonAction jump;

    public ButtonAction Primary => primary;
    private ButtonAction primary;

    public ButtonAction Secondary => secondary;
    private ButtonAction secondary;

    private void Awake()
    {
        jump = new ButtonAction();
        primary = new ButtonAction();
        secondary = new ButtonAction();
        StartCoroutine(CycleDirectionVector());
    }

    public IEnumerator CycleDirectionVector()
    {
        randomizedVector.z = Random.Range(-1f, 1f);
        //randomizedVector.x = Random.Range(-1f, 1f);
        yield return new WaitForSeconds(Random.Range(0.5f, 1));
        StartCoroutine(CycleDirectionVector());
    }

    public void FixedUpdate()
    {
        /*if (Vector3.Distance(player.transform.position, transform.position) < 5)
        {
            weapon.GetComponent<RangedWeapon>().Shoot();
        }*/

        //targetDirection = player.transform.position - transform.position;
        //input.moveDirection = Vector3.Lerp(randomizedVector.normalized, targetDirection.normalized, Mathf.InverseLerp(innerTarget, outerTarget, targetDirection.magnitude));
        //transform.position = transform.position + input.moveDirection/5;
    }

    public InputState GetState()
    {
        return input;
    }
}