using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Drawing;

struct BulletTransformJob : IJobParallelForTransform
{
    [ReadOnly]
    public NativeArray<Vector3> positions;

    public void Execute(int index, TransformAccess transform)
    {
        transform.localPosition = positions[index];
    }
};

class BulletSystem : MonoBehaviour
{
    private static BulletSystem instance;

    const int CAPACITY = 4096;

    Bullet[] bullets;
    int bulletCount = 0;

    TransformAccessArray transforms;
    NativeArray<Vector3> positionsToWrite;
    JobHandle txJob;

    NativeArray<RaycastCommand> commands;
    NativeArray<RaycastHit> results;
    JobHandle physJob;
    private List<Bullet> bulletsToAdd;

    void Awake()
    {
        instance = this;
        bullets = new Bullet[CAPACITY];
        transforms = new TransformAccessArray(CAPACITY);
        results = new NativeArray<RaycastHit>(CAPACITY, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        commands = new NativeArray<RaycastCommand>(CAPACITY, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        positionsToWrite = new NativeArray<Vector3>(CAPACITY, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        /*
        Debug.Log("Transforms (cap/length): " + transforms.capacity + ", " + transforms.length);
        Debug.Log("results (cap/length): " + results.IsCreated + ", " + results.Length);
        Debug.Log("commands (cap/length): " + commands.IsCreated + ", " + commands.Length);
        Debug.Log("positionsToWrite (cap/length): " + positionsToWrite.IsCreated + ", " + positionsToWrite.Length);
        */
        bulletsToAdd = new List<Bullet>();
    }

    [System.Obsolete]
    void Update()
    {
        foreach (Bullet bullet in bulletsToAdd) {
            bullets[bulletCount] = bullet;
            transforms.Add(transform);
            bulletCount++;
        }
        bulletsToAdd.Clear();

        if (bulletCount == 0) return;

        float dt = Time.deltaTime;

        /*
		Build job input buffers.
		*/
        for (int it = 0; it < bulletCount; ++it)
        {
            Bullet bullet = bullets[it];
            positionsToWrite[it] = bullet.transform.position + (dt * bullet.Speed) * bullet.direction;
            commands[it] = new RaycastCommand(bullet.transform.position, bullet.direction, bullet.Speed * dt);
        }

        /*
		Schedule a batch transform update.
		*/
        BulletTransformJob job;
        job.positions = positionsToWrite;
        txJob = job.Schedule(transforms);

        /*
		Schedule a batch of physics queries.
		*/
        physJob = RaycastCommand.ScheduleBatch(commands.GetSubArray(0, bulletCount), results.GetSubArray(0, bulletCount), 1);
    }

    void LateUpdate()
    {
        if (bulletCount == 0) return;

        float dt = Time.deltaTime;

        /*
		Wait for both jobs to finish, if they're still going.
		*/
        txJob.Complete();
        physJob.Complete();

        /*
		Handle bullet impacts, swapping with the end of the array to remove
		*/
        for (int it = 0; it < bulletCount;)
        {
            var bullet = bullets[it];
            var hit = results[it];
            if (hit.collider == null)
            {
                bullet.transform.position += (dt * bullet.Speed) * bullet.direction;
                ++it;
                continue;
            }
            bullet.transform.position = hit.point;
            HandleHit(bullet, hit);
            StartCoroutine(ReleaseBulletDelayed(bullet, 1f));
            bulletCount--;
            transforms.RemoveAtSwapBack(it);
            if (it < bulletCount)
            {
                bullets[it] = bullets[bulletCount];
                results[it] = results[bulletCount];
            }
            bullets[bulletCount] = null;
        }
    }

    public void HandleHit(Bullet bullet, RaycastHit hit)
    {
        if(bullet.hitEffect!= null) PooledObject.Create(bullet.hitEffect, hit.point, Quaternion.LookRotation(Vector3.Lerp(hit.normal, -bullet.direction, 0.2f)));
        using (Draw.WithColor(Color.red))
        {
            using (Draw.WithDuration(0.1f)) {
                Draw.WireSphere(hit.point, 0.25f);
                Draw.Ray(hit.point, hit.normal);
            } 
        }

        // Have damageable takeDamage
        if (hit.transform.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(bullet.Damage);
        }
    }

    void OnDestroy()
    {
        instance = null;
        transforms.Dispose();
        results.Dispose();
        commands.Dispose();
        positionsToWrite.Dispose();
    }

    private IEnumerator ReleaseBulletDelayed(Bullet bullet, float delay) {
        bullet.body.SetActive(false);
        yield return new WaitForSeconds(delay);
        bullet.body.SetActive(true);
        bullet.Release();
    }

    public static void Add(Bullet bullet) {
        CheckSingleton();
        instance.bulletsToAdd.Add(bullet);
    }

    public static BulletSystem CheckSingleton()
    {
        if (instance == null) instance = new GameObject("[Bullet System]").AddComponent<BulletSystem>();
        return instance;
    }

}