using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {
    public GameObject EnemyProjectile;
    public int rotation = 100;
    public float appliedForce = 20f;
    private float nextActionTime = 0;
    public float Period = 10.0f;
    void Start()
    {
        //The Asteroid Manager will handle all movement of the asteroids
        AsteroidsManager.Instance.RegisterAsteroid(gameObject);

        RadomizeDirection(transform.position);
        
    }

    private void OnDestroy()
    {
        AsteroidsManager.Instance.UnregisterAsteroid(gameObject);
    }

    public void RadomizeDirection(Vector3 newPosition)
    {
        //newPosition.y = 0;
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));

        Vector3 newDirection = new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f));
        newDirection.Normalize();

        //Caching the Rigid Body to avoid the slowness of getting it every time
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(newDirection * appliedForce);
    }

    private void Update()
    {
        transform.Rotate(0, rotation * Time.deltaTime, 0);
        if(Time.time > nextActionTime)
        {
            nextActionTime = Time.time + Period;
            Fire();
        }
    }

    void Fire()
    {
        var Projectile = Instantiate(
                EnemyProjectile,
                transform.position + transform.forward,
                transform.rotation);

        Destroy(Projectile, 5f);
    }
}