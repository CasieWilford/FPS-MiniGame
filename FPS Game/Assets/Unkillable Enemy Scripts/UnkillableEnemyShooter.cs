using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnkillableEnemyShooter : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    public Transform player;
    public ParticleSystem enemyShoot;

    public AudioClip enemyShootClip;
    public AudioSource enemyShootSource;

    float fireRate;
    float nextFire;

    public static bool startFiring;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        fireRate = 3f;
        nextFire = Time.time;

        enemyShootSource.clip = enemyShootClip;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfTimeToFire();
    }

    void CheckIfTimeToFire()
    {
        if (Time.time > nextFire && startFiring)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;

            enemyShoot.Play();
            enemyShootSource.Play();
        }
    }
}
