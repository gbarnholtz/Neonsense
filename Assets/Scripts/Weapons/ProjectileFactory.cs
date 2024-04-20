using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public static class ProjectileFactory
{
    public static void CreateBullet(Bullet bullet, float damage, float speed, Vector3 position, Vector3 direction, Team team) {
        Bullet newBullet = PooledObject.Create(bullet, position, Quaternion.LookRotation(direction, Vector3.up));
        InitializeBullet(newBullet, damage, speed, team);
    }

    public static void CreateBullet(Bullet bullet, float damage, float speed, Transform firePoint, Vector3 target, Team team)
    {
        Bullet newBullet = PooledObject.Create(bullet, firePoint.position, Quaternion.LookRotation(target-firePoint.position, Vector3.up));
        InitializeBullet(newBullet, damage, speed, team);
    }

    private static void InitializeBullet(Bullet bullet, float damage, float speed, Team team) {
        bullet.Damage = damage;
        bullet.Speed = speed;
        bullet.Team = team;
        BulletSystem.Add(bullet);
    }
}
