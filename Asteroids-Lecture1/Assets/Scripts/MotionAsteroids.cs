using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionAsteroids : MonoBehaviour {
    public Vector3 halfExtents = new Vector3(6, 0, 4);
    public Vector3 startPos;
    public Vector3 target;
    public float speed = 0.2f;
    public float rotationSpeed = 180f;
    public float count = 0.5f;
    float getRandomValue()
    {
        return Random.Range(-6, 0);
    }

    Vector3 getRandomPositionCoordinates()
    {
       int which = Random.Range( 0,4);
       if(which == 1) return new Vector3(9.0f, 0.0f, 0.0f);
       if (which == 2) return new Vector3(0.0f, 0.0f, 6.0f);
       if (which == 3) return new Vector3(6.0f, 0.0f, 4.0f);

    return new Vector3(0.0f, 0.0f, -6.0f);
    }
    void Start()
    {
        //target = new Vector3(12.0f, 12.0f, 12.0f); 
        target = getRandomPositionCoordinates();
        startPos = this.transform.position;
    }
    void Update() {
        float step = speed * Time.deltaTime*2;
        transform.position = Vector3.MoveTowards(transform.position,target, 0.5f);
        Vector3 newPosition = transform.position;
        for (int i = 0; i < 3; ++i)
        {
            float prev = newPosition[i];
            newPosition[i] = Mathf.Clamp(newPosition[i], -halfExtents[i], halfExtents[i]);
            if (prev != newPosition[i])
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, step);
                Debug.Log("Start pos: " + startPos);
            }

        }
        transform.position = newPosition;

        Update();
    }

    
}
