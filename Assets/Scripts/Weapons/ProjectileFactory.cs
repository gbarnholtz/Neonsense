using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public static class ProjectileFactory
{
    public static void CreateBullet(Bullet bullet, float damage, float speed, Vector3 position, Vector3 direction) {
        Bullet newBullet = PooledObject.Create(bullet, position, Quaternion.LookRotation(direction, Vector3.up));
        InitializeBullet(newBullet, damage, speed);
    }

    public static void CreateBullet(Bullet bullet, float damage, float speed, Transform firePoint, Vector3 target)
    {
        Bullet newBullet = PooledObject.Create(bullet, firePoint.position, Quaternion.LookRotation(target-firePoint.position, Vector3.up));
        InitializeBullet(newBullet, damage, speed);
    }

    private static void InitializeBullet(Bullet bullet, float damage, float speed) {
        bullet.Damage = damage;
        bullet.Speed = speed;
        BulletSystem.Add(bullet);
    }
}
