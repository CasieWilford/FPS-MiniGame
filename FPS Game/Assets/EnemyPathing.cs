using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    // Enemy health bar.
    public float health = 110f;

    float speed = 2.5f;
    float stoppingDistance = 0f;
    float attackRadius = 12f;

    public GameObject deathParticleObject;
    public ParticleSystem enemyDeathParticle;

    public AudioSource chaseSource;
    public AudioClip chaseClip;


    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        chaseSource.clip = chaseClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) <= attackRadius)
        {

            if(!chaseSource.isPlaying)
                chaseSource.Play();

            // If the enemy is far away, it will move towards the player.
            if (Vector3.Distance(transform.position, player.transform.position) > stoppingDistance)
            {
                this.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }

            // If enemy is close enough but no to close, the enemy will stop.
            else if (Vector3.Distance(transform.position, player.transform.position) < stoppingDistance)// && Vector2.Distance(transform.position, player.transform.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }
        }
        else
        {
            chaseSource.Stop();
        }

        deathParticleObject.transform.position = this.transform.position;
    }

    // Enemy health control.
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            WinConditions.enemiesDead++;

            enemyDeathParticle.Play();
            Die();
        }

        //HitMaterial();
    }

    // Destroy object once health is <= 0.
    void Die()
    {
        Destroy(gameObject);
        Destroy(deathParticleObject, 3f);
    }
}
