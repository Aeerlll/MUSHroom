using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour {

    public Transform muzzle;
    public Shooting projectile;
    public float msBeetweenShots = 100;
    public float muzzleVelocity = 35;

    float nextShotTime;

    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBeetweenShots / 1000;
            Shooting newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Shooting;
            newProjectile.SetSpeed(muzzleVelocity);
        }
    }
}