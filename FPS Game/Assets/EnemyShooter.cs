using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    public float Distance_;
    public float range = 15f;
    public Transform player;

    public AudioClip enemyShootClip;
    public AudioSource enemyShootSource;

    float fireRate;
    float nextFire;


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
        Distance_ = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if (Distance_ <= range)
        {
            CheckIfTimeToFire();
        }
    }

    void CheckIfTimeToFire()
    {
        if (Time.time > nextFire)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
            
            enemyShootSource.Play();
        }
    }
}
