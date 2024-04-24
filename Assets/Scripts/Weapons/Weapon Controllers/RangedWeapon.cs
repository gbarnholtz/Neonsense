using System;
using System.Collections;
using UnityEngine;


public class RangedWeapon : IWeapon
{
    //[SerializeField] protected WeaponData weaponData;
    [SerializeField] protected Transform firePoint;
    public event Action Fired, Reloaded;
    [SerializeField] protected float bulletDamage = 1f, bulletSpeed = 25f; 
    [SerializeField] protected int bulletsPerShot = 1;
    [SerializeField] private bool automatic;
    [SerializeField] public float reloadTime = 1;
    [SerializeField] protected int magazineSize = 10;
    [SerializeField] protected int ammoPool = 9999;

    [SerializeField] protected int ammoLoaded;
    public int AmmoPool { get { return ammoPool;} set { ammoPool = value; } }
    public int MagazineSize => magazineSize;
    public int AmmoLoaded => ammoLoaded;
    public float ReloadProgress => Mathf.Clamp01((Time.time - timeReloadStarted) / reloadTime);
    
    protected float autoReloadDelay = 0.1f;

    public float rpm = 60;

    public int maxAmmo;

    [SerializeField] protected Vector3 recoil;
    [SerializeField, Range(0,1)] private float spread;
    [SerializeField] Bullet bullet;

    public bool isPastFireRate = true, isFiring;
    public bool IsReloading => ReloadProgress < 1;
    public bool IsCooling => ReloadProgress < 1;
    public bool IsLoaded => ammoLoaded > 0;
    protected override bool CanAttack { get { return  !IsReloading && !IsCooling && isPastFireRate && !isFiring && IsLoaded; } }

    private float fireRate => 60 / rpm;
    public float timeReloadStarted;

    private AudioSource audioSource;
    private AudioClip weaponSound;

    [SerializeField] private float volume;
    [SerializeField] private AudioClip reloadSound;

    private Team team;

    protected virtual void Awake()
    {
        ammoLoaded = magazineSize;
        timeReloadStarted = -reloadTime;
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            weaponSound = audioSource.clip;
        }
        SetTeam();

        maxAmmo = ammoPool;
    }

    private void SetTeam()
    {
        if (transform.parent.gameObject.tag == "normal_enemy"
            || transform.parent.gameObject.tag == "drone")
        {
            team = Team.Enemy;
        } else
        {
            team = Team.Ally;
        }
    }

    public IEnumerator Reload() {
        if (IsReloading) yield return null;
        Reloaded?.Invoke();
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
        audioSource.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(autoReloadDelay);
        StartCoroutine(Reload());
    }

    public override IEnumerator Attack()
    {
        //Debug.Log("Entering Attack method");
        audioSource.volume = volume;
        audioSource.PlayOneShot(weaponSound);
        Fired?.Invoke();
        IsAttacking = true;
        Debug.DrawRay(firePoint.position, firePoint.forward, Color.red, 1f);

        for (int i = 0; i < bulletsPerShot; i++) {
            float inaccuracy = Mathf.Lerp(0, 90, spread);
            Quaternion inaccuracyRotation = Quaternion.Euler(UnityEngine.Random.Range(-inaccuracy, inaccuracy), UnityEngine.Random.Range(-inaccuracy, inaccuracy), UnityEngine.Random.Range(-inaccuracy, inaccuracy));
            ProjectileFactory.CreateBullet(bullet, bulletDamage, bulletSpeed, firePoint.position, inaccuracyRotation * firePoint.forward, team);
        }

        InvokeRecoil(recoil);
        ammoLoaded--;
        if (ammoLoaded <= 0)
        {
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
