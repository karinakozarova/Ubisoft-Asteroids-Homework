using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float projectileVelocity = 15f;
    public float timeToLive = 3f;
    public string tagToDamage = "Enemy";
    public 
	
	void Start () {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward * projectileVelocity;
        Destroy(gameObject, timeToLive);
	}
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}
