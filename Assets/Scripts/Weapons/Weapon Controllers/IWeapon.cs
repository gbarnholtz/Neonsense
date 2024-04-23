using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class IWeapon : MonoBehaviour, IRecoilProvider, IButtonActionSubscriber
{
    public string Description { get { return description; } }
    [TextAreaAttribute]
    public string description = "Lorem Ipsum";

    protected bool tryingToAttack, attackQueued;
    public bool AttackQueued { get{ return attackQueued; } }

    public bool IsAttacking;
    protected abstract bool CanAttack { get; }

    [SerializeField] private AudioClip pickupSound;
    private AudioSource soundAttachedToPlayer;

    private ArsenalController arsenal;
    private GameObject audioSourceObj;

    public event Action<Vector3> Recoil;

    void Start()
    {
        arsenal = GameObject.FindWithTag("Player").GetComponent<ArsenalController>();
        audioSourceObj = GameObject.FindWithTag("player_audio_source");
        soundAttachedToPlayer = audioSourceObj.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            /* Signal to the arsenal controller that weapon has been picked up */
            arsenal.PickupWeapon(description);
            
            soundAttachedToPlayer.PlayOneShot(pickupSound);
            
            /* Destroys this weapon as the real weapon is simply disabled on the arsenal */
            Destroy(gameObject);
        }
    }

    protected void InvokeRecoil(Vector3 recoilVector) => Recoil?.Invoke(recoilVector);

    public abstract void Subscribe(ButtonAction attackActions);
    public abstract void Unsubscribe(ButtonAction attackActions);

    public void StartTryingToFire()
    {
        tryingToAttack = true;
        if (CanAttack) StartCoroutine(Attack());

    }
    public void StopTryingToFire()
    {
        tryingToAttack = false;
    }

    public abstract IEnumerator Attack();
}



public interface IRecoilProvider {
    public event Action<Vector3> Recoil;
}
