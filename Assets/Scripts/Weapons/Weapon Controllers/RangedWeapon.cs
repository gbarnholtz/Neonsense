using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class RangedWeapon : IWeapon
{
    //[SerializeField] protected WeaponData weaponData;
    [SerializeField] protected Transform firePoint;
    
    [SerializeField] protected float bulletDamage = 1f, bulletSpeed = 25f; 
    [SerializeField] protected int bulletsPerShot = 1;
    [SerializeField] private bool automatic;
    [SerializeField] protected float reloadTime = 1;
    [SerializeField] protected int magazineSize = 10;
    [SerializeField] protected int ammoPool = 9999;

    [SerializeField] protected int ammoLoaded;
    public int AmmoPool => ammoPool;
    public int MagazineSize => magazineSize;
    public int AmmoLoaded => ammoLoaded;
    public float ReloadProgress => Mathf.Clamp01((Time.time - timeReloadStarted) / reloadTime);
    
    protected float autoReloadDelay = 0.1f;

    public float rpm = 60;

    [SerializeField] protected Vector3 recoil;
    [SerializeField, Range(0,1)] private float spread;
    [SerializeField] Bullet bullet;

    protected bool isPastFireRate = true, isFiring;
    public bool IsReloading => ReloadProgress < 1;
    public bool IsCooling => ReloadProgress < 1;
    public bool IsLoaded => ammoLoaded > 0;
    protected override bool CanAttack { get { return  !IsReloading && !IsCooling && isPastFireRate && !isFiring && IsLoaded; } }

    private float fireRate => 60 / rpm;
    private float timeReloadStarted;

    private AudioSource audioSource;
    private AudioClip weaponSound;

    protected virtual void Awake()
    {
        ammoLoaded = magazineSize;
        timeReloadStarted = -reloadTime;
        audioSource = gameObject.GetComponent<AudioSource>();
        weaponSound = audioSource.clip;
    }

    public IEnumerator Reload() {
        if (IsReloading) yield return null;
        timeReloadStarted = Time.time;
        ammoPool += ammoLoaded;
        ammoLoaded = 0;
        yield return new WaitForSeconds(reloadTime);
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

    public override IEnumerator Attack()
    {
        //Debug.Log("Entering Attack method");
        audioSource.volume = 0.01f;
        audioSource.PlayOneShot(weaponSound);

        IsAttacking = true;
        Debug.DrawRay(firePoint.position, firePoint.forward, Color.red, 1f);

        for (int i = 0; i < bulletsPerShot; i++) {
            float inaccuracy = Mathf.Lerp(0, 90, spread);
            Quaternion inaccuracyRotation = Quaternion.Euler(Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy));
            ProjectileFactory.CreateBullet(bullet, bulletDamage, bulletSpeed, firePoint.position, inaccuracyRotation * firePoint.forward);
        }

        InvokeRecoil(recoil);
        ammoLoaded--;
        if (ammoLoaded <= 0)
        {
            // TODO: play weapond reload sound here
            StopTryingToFire();
            StartCoroutine(DelayedReload());
        }

        isPastFireRate = false;
        yield return new WaitForSeconds(fireRate);
        isPastFireRate = true;
        IsAttacking = false;

        if (automatic && CanAttack && tryingToAttack) StartCoroutine(Attack());
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
