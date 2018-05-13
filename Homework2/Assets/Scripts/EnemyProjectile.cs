using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
    public float speed = 6f;

    // Use this for initialization
    void Start()
    {
        var Rb = GetComponent<Rigidbody>();
        Rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerShip")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
