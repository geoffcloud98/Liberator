using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject explosion;
    public GameObject player;
    public LayerMask whatIsPlayer;
    //damage
    public int explosionDamage;
    public float explosionRange;
    //bullet lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;
    int collisions;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(collisions > maxCollisions) Explode();

        //count down life
        maxLifetime -= Time.deltaTime;
        if(maxLifetime <= 0) Explode();
    }

    private void Explode()
    {
        if(explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);
        Collider[] players = Physics.OverlapSphere(transform.position, explosionRange, whatIsPlayer);
        for(int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerHealth>().TakeDamage(explosionDamage);
        }
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        DestroyImmediate(gameObject, true);
    }

}
