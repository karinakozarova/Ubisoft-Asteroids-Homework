using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public GameObject projectileToSpawn;
    public Transform shotSpawnTransform;
    public float fireRate = 0.5f;
    public float projectileTimeToLive = 3f;
    public float firstShotDelay = 0.0f;
    private float nextShotTime = 0.0f;

    public void Start()
    {
        nextShotTime = Time.time + firstShotDelay;
    }
    public void Fire()
    {
        if (Time.time > nextShotTime)
        {
            GameObject newProjectile = Instantiate(projectileToSpawn, shotSpawnTransform.position, shotSpawnTransform.rotation);
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), newProjectile.GetComponent<Collider>());
            nextShotTime = Time.time + fireRate;
        }
    }
}
