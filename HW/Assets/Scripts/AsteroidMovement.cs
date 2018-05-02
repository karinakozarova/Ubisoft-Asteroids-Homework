using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour {
    public float maxPositionX = 9.8f;
    public float minPositionX = -9.8f;
    public float maxPositionY = 5.2f;
    public float minPositionY = -5.2f;
    public float speed = 60.0f;
    public float rotationSpeed = 500f;
    public Vector3 halfExtents = new Vector3(9.8f, 0, 5.2f);
    float hor, ver;
    private void Start()
    {
        hor = Random.Range(minPositionX, maxPositionX);
        ver = Random.Range(minPositionY, maxPositionY);
    }

    void Update () {
        
        Quaternion offset = Quaternion.Euler(0f, 0f, rotationSpeed * Time.deltaTime);
        transform.rotation = transform.rotation * offset;
        Vector3 displacement = new Vector3(hor, 0f, ver).normalized * speed * Time.deltaTime;
        Vector3 newPosition = transform.position + displacement;
        if (newPosition.x >= maxPositionX || newPosition.x <= minPositionX || newPosition.y >= maxPositionY || newPosition.y <= minPositionY)
        {
            hor = Random.Range(minPositionX, maxPositionX);
            ver = Random.Range(minPositionY,maxPositionY);
            transform.position = new Vector3(Random.Range(-9.7f, 9.7f), 0f, Random.Range(-5.1f, 5.1f));
        }
        else transform.position = newPosition;
    }
}
