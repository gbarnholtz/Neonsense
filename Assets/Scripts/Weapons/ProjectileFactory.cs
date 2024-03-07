using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public static class ProjectileFactory
{
    public static void CreateBullet(Bullet bullet, DamageInstance damage, Vector3 position, Vector3 direction) {
        Bullet newBullet = PooledObject.Create(bullet, position, Quaternion.LookRotation(direction, Vector3.up));
        InitializeBullet(newBullet, damage);
    }

    public static void CreateBullet(Bullet bullet, DamageInstance damage, Transform firePoint, Vector3 target)
    {
        Bullet newBullet = PooledObject.Create(bullet, firePoint.position, Quaternion.LookRotation(target-firePoint.position, Vector3.up));
        InitializeBullet(newBullet, damage);
    }

    private static void InitializeBullet(Bullet bullet, DamageInstance damage) {
        BulletSystem.Add(bullet);
        bullet.damage = damage;
    }
}
