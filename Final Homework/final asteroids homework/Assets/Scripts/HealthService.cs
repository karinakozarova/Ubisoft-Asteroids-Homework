using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HealthService : MonoBehaviour {

    public int health = 100;
    protected int currentHealth = 100;

    public uint splitChunksCount = 0;
    public GameObject chunkPrefab;
    public Vector3 spawnOffset;
    
    protected virtual void Start () {
        currentHealth = health;
	}

    public void DealDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    protected void OnDeath()
    {
        if (chunkPrefab != null && splitChunksCount > 0)
        {
            float step = 360f / splitChunksCount;
            for (int i = 0; i < splitChunksCount; i++)
            {
                float angle = step * i;
                Vector3 spawnPosition = gameObject.transform.position;
                spawnPosition += Quaternion.Euler(0, angle, 0) * spawnOffset;
                Instantiate(chunkPrefab, spawnPosition, gameObject.transform.rotation);
            }
        }

        Destroy(gameObject);
        SceneManager.LoadScene("End");
    }
    
}
