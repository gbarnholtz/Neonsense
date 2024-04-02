using System;
using System.Collections;
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

    public event Action<Vector3> Recoil;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            /* Signal to the arsenal controller that weapon has been picked up */
            ArsenalController arsenal = GameObject.FindWithTag("Player").GetComponent<ArsenalController>();
            arsenal.PickupWeapon(description);
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
