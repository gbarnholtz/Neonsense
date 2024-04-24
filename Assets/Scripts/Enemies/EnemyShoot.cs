using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class EnemyShoot : SerializedMonoBehaviour, ICharacterInputProvider
{
    [SerializeField] private float outerTarget, innerTarget;
    [SerializeField] private Vector3 targetDirection, randomizedVector;


    private InputState input = new InputState();
        
    private GameObject player;
    [SerializeField] private int distanceToShoot;
    [SerializeField] private int distanceToLookAtPlayer;
    [SerializeField] private RangedWeapon weapon;

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
        player = GameObject.FindWithTag("Player");
    }

    public IEnumerator CycleDirectionVector()
    {
        randomizedVector.z = Random.Range(-1f, 1f);
        randomizedVector.x = Random.Range(-1f, 1f);
        yield return new WaitForSeconds(Random.Range(0.5f, 1));
        StartCoroutine(CycleDirectionVector());
    }

    public InputState GetState()
    {
        return input;
    }

    void Update()
    {
        /* Shoot player if within range */
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToShoot)
        {
            weapon.StartTryingToFire();
        } 
        
        /* Rotate weapon towards player */
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToLookAtPlayer)
        {
            // Old method
            //rotateWeaponTowardsPlayer();
            
            RotateTowardsPlayer();
        }
        else
        {
            weapon.transform.rotation = new Quaternion();
        }
    }

    // Still doesn't work :(
    // Maybe it's a problem with the navmesh or something
    void RotateTowardsPlayer()
    {
        // Makes the enemy face player
        gameObject.transform.LookAt(player.transform);
        
        // Points weapon directly at player
        weapon.transform.LookAt(player.transform);
    }

    // Old method
    void rotateWeaponTowardsPlayer()
    {
        /* Makes the enemy face player */
        Vector3 playerDirection = player.transform.position - transform.position;
        Quaternion quaternion = Quaternion.LookRotation(playerDirection, Vector3.up);
        quaternion.x = 0f;
        quaternion.z = 0f;
        gameObject.transform.rotation = quaternion;

        /* Points weapon directly at player */
        playerDirection = player.transform.position - weapon.transform.position;
        weapon.transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
    }
}