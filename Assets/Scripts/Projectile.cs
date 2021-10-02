using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy e = collision.GetComponent<Enemy>();
        if (e != null) {
            e.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
