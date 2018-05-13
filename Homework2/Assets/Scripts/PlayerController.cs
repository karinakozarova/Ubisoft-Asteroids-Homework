using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject PlayerProjectile;
    public float speed = 0.2f;
    public float rotationSpeed = 180f;
    public Vector3 halfExtents = new Vector3(6, 0, 4);

    private void Start()
    {
        AsteroidsManager.Instance.RegisterPlayer(gameObject);
    }
    

    void Update () {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Quaternion offset = Quaternion.Euler(0f, horizontalInput * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = transform.rotation * offset;
        Vector3 displacement = new Vector3(0f, 0f, verticalInput) * speed * Time.deltaTime;
        displacement = transform.rotation * displacement;
        Vector3 newPosition = transform.position + displacement;
        for (int i =0; i < 3; ++i)
        {
            newPosition[i] = Mathf.Clamp(newPosition[i], -halfExtents[i], halfExtents[i]);
        }
        transform.position = newPosition;


        if (Input.GetKeyDown(KeyCode.Space)) {
            var Projectile = Instantiate(
                PlayerProjectile,
                transform.position + transform.forward,
                transform.rotation);

            Destroy(Projectile, 5f);
        }
    }
    private void OnDestroy()
    {
        AsteroidsManager.Instance.UnregisterPlayer(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "PlayerProjectile 1 1(Clone)")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
