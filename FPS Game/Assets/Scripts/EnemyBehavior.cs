using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Enemy health bar.
    public float health = 60f;

    public ParticleSystem enemyDeathParticle;

    public bool isHit = false;
    public float timerHurt = 0;
    public float durationOfHurt = .2f;

    public Transform player;

    public float Distance_;
    public float range = 15f;

    public static bool playerInRange;

    public AudioClip lookAtClip;
    public AudioSource lookAtSource;

    [Header("Body Parts")]
    public GameObject eye1;
    public GameObject eye2;
    public GameObject mouth;
    public GameObject arm1;
    public GameObject arm2;
    public GameObject leg;

    [Header("Material")]
    public Material originalMat;
    public Material originalBodyMat;
    public Material hitMat;
    public Material hitBodyMat;
    public Material lookAtMat;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        lookAtSource.clip = lookAtClip;
    }

    private void Update()
    {
        if (isHit)
        {
            if (timerHurt < durationOfHurt)
                timerHurt += Time.deltaTime;
            else
            {
                isHit = false;
                timerHurt = 0;
                
                // Changes enemy back to original color.
                gameObject.GetComponent<MeshRenderer>().material = originalBodyMat;
                eye1.GetComponentInChildren<MeshRenderer>().material = originalMat;
                eye2.GetComponentInChildren<MeshRenderer>().material = originalMat;
                mouth.GetComponentInChildren<MeshRenderer>().material = originalMat;
                arm1.GetComponentInChildren<MeshRenderer>().material = originalMat;
                arm2.GetComponentInChildren<MeshRenderer>().material = originalMat;
                leg.GetComponentInChildren<MeshRenderer>().material = originalMat;
            }
        }

        Distance_ = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if (Distance_ <= range)
        {
            Vector3 lookVector = player.transform.position - transform.position;
            lookVector.y = transform.position.y;
            Quaternion rot = Quaternion.LookRotation(lookVector);
            rot.x = 0;
            rot.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);

            eye1.GetComponentInChildren<MeshRenderer>().material = lookAtMat;
            eye2.GetComponentInChildren<MeshRenderer>().material = lookAtMat;

            lookAtSource.Play();

            playerInRange = true;
        }
        else
        {
            if (!isHit)
            {
                eye1.GetComponentInChildren<MeshRenderer>().material = originalMat;
                eye2.GetComponentInChildren<MeshRenderer>().material = originalMat;
            }

            playerInRange = false;
        }
    }

    // Enemy health control.
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            WinConditions.enemiesDead++;

            Die();
            enemyDeathParticle.Play();
        }

        HitMaterial();
    }
    
    // Destroy object once health is <= 0.
    void Die()
    {
        Destroy(gameObject);
    }

    // Changes enemys material when hit.
    void HitMaterial()
    {
        gameObject.GetComponent<MeshRenderer>().material = hitBodyMat;
        eye1.GetComponentInChildren<MeshRenderer>().material = hitMat;
        eye2.GetComponentInChildren<MeshRenderer>().material = hitMat;
        mouth.GetComponentInChildren<MeshRenderer>().material = hitMat;
        arm1.GetComponentInChildren<MeshRenderer>().material = hitMat;
        arm2.GetComponentInChildren<MeshRenderer>().material = hitMat;
        leg.GetComponentInChildren<MeshRenderer>().material = hitMat;

        isHit = true;
    }
}
