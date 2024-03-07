using System;
using UnityEngine;

public abstract class IWeapon : MonoBehaviour, IRecoilProvider, IAttackActionSubscriber
{
    public string Description { get { return description; } }
    [TextAreaAttribute]
    public string description = "Lorem Ipsum";
    public Action attackHappened;

    protected bool tryingToAttack, attackQueued;
    public bool AttackQueued { get{ return attackQueued; } }

    public bool IsAttacking, OnTarget;
    protected abstract bool CanAttack { get; }

    public event Action<Vector3> Recoil;

    protected void InvokeRecoil(Vector3 recoilVector) => Recoil?.Invoke(recoilVector);

    public abstract void Subscribe(ButtonAction attackActions);
    public abstract void Unsubscribe(ButtonAction attackActions);

    protected void StartTryingToFire()
    {
        tryingToAttack = true;
        if (CanAttack)
        {
            attackQueued = true;
        }

    }
    protected void StopTryingToFire()
    {
        tryingToAttack = false;
    }
}

public interface IAttackActionSubscriber
{
    public void Subscribe(ButtonAction attackActions);
    public void Unsubscribe(ButtonAction attackActions);
}

public interface IRecoilProvider {
    public event Action<Vector3> Recoil;
}
