using System.Collections;
using UnityEngine;


public abstract class IRangedWeapon : IWeapon
{
    //[SerializeField] protected WeaponData weaponData;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected int ammoPool = 9999;
    [SerializeField] protected int magazineSize = 10;
    [SerializeField] protected int ammoLoaded;
    
    public int AmmoPool => ammoPool;
    public int MagazineSize => magazineSize;
    public int AmmoLoaded => ammoLoaded;
    public float ReloadProgress => Mathf.Clamp01((Time.time - timeReloadStarted) / reloadTime);
    public float HeatLevel => heatLevel;
    
    [SerializeField] protected float reloadTime = 1;
    protected float autoReloadDelay = 0.1f;

    public float rpm = 60;
    [SerializeField] protected float heatPerShot, heatRecoveryRate;

    [SerializeField] protected Vector3 recoil;
    

    protected bool isPastFireRate = true, isFiring;
    public bool IsReloading => ReloadProgress < 1;
    public bool IsCooling => ReloadProgress < 1;
    public bool IsLoaded => ammoLoaded > 0;
    protected override bool CanAttack { get { return  !IsReloading && !IsCooling && isPastFireRate && !isFiring && IsLoaded; } }

    private float fireRate => 60 / rpm;
    private float heatLevel, timeReloadStarted;

    protected virtual void Awake()
    {
        ammoLoaded = magazineSize;
        timeReloadStarted = -reloadTime;
    }

    public IEnumerator Reload() {
        if (IsReloading) yield return null;
        timeReloadStarted = Time.time;
        ammoPool += ammoLoaded;
        ammoLoaded = 0;
        //if (!reloadStarted.IsNull) AudioManager.PlayOneShotAttached(reloadStarted, gameObject);
        yield return new WaitForSeconds(reloadTime);
        //if (!reloadFinished.IsNull) AudioManager.PlayOneShotAttached(reloadFinished, gameObject);
        int ammoToLoad = Mathf.Clamp(magazineSize, 0, ammoPool);
        ammoLoaded = magazineSize;
        ammoPool -= ammoToLoad;
        ammoLoaded = ammoToLoad;
    }

    public IEnumerator DelayedReload()
    {
        yield return new WaitForSeconds(autoReloadDelay);
        StartCoroutine(Reload());
    }
    public virtual IEnumerator Fire()
    {
        IsAttacking = true;
        Debug.DrawRay(firePoint.position, firePoint.forward, Color.red, 1f);
        attackHappened?.Invoke();
        InvokeRecoil(recoil);

        //Vector3 bulletDirection = (velocity + firePoint.forward * bulletData.speed).normalized;
        //ProjectileFactory.CreateBullet(bulletData, firePoint.position, firePoint.forward);

        ammoLoaded--;
        if (ammoLoaded <= 0)
        {
            StopTryingToFire();
            StartCoroutine(DelayedReload());
        }
        //if (!fireSound.IsNull) AudioManager.PlayOneShotAttached(fireSound, firePoint.gameObject);


        isPastFireRate = false;
        yield return new WaitForSeconds(fireRate);
        isPastFireRate = true;
        IsAttacking = false;
    }

    public override void Subscribe(ButtonAction attackActions)
    {

        attackActions.started += StartTryingToFire;
        attackActions.ended += StopTryingToFire;
    }
    public override void Unsubscribe(ButtonAction attackActions)
    {
        attackActions.started -= StartTryingToFire;
        attackActions.ended -= StopTryingToFire;
    }
}
