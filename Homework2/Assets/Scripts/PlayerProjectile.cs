using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    public float speed;

    // Use this for initialization
    void Start() {
        var Rb = GetComponent<Rigidbody>();
        Rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.name != "PlayerShip") {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
       }
    }
}