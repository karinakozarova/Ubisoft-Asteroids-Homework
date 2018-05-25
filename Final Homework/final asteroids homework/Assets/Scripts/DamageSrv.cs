using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSrv : MonoBehaviour {

    public int damage = 10;
    public string tagToDamage = "Enemy";
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(tagToDamage))
        {
            HealthService healthSrv = collision.gameObject.GetComponent<HealthService>();
            if (healthSrv)
                healthSrv.DealDamage(damage);
        }
    }
}
