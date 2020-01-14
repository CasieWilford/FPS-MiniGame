using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Projectile for Small Enemy.
public class EnemyProjectile : MonoBehaviour
{ 
    public float speed = 5f;
    public float damage = 10f;
    private Transform player;
    private Vector3 target;

    public ParticleSystem enemyParticle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Players position.
        target = new Vector3(player.position.x, player.position.y, player.position.z);

        gameObject.GetComponent<Rigidbody>().AddRelativeForce((target - gameObject.transform.position) * 32f);

        Destroy(gameObject, 10f);
    }

    void DestroyProjectile()
    {
        Instantiate(enemyParticle, gameObject.transform.position, Quaternion.identity);

        enemyParticle.Play();

        gameObject.SetActive(false);
        Destroy(gameObject, 3f);
    }

    // Reset player when hit by bullet.
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            /*Quaternion resetRotation = new Quaternion();
            resetRotation.x = 0;
            resetRotation.y = 0;
            resetRotation.z = 0;

            Vector3 resetPosition = new Vector3(0, 1, 0);

            other.gameObject.transform.position = resetPosition;
            other.gameObject.transform.rotation = resetRotation;

            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;*/

            PlayerHealth target = transform.GetComponent<PlayerHealth>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            FlashScreen.flashScreen = true;
        }

        // Plays Impact Audio.
        GameObject audio = GameObject.Find("Unkillable Hit Audio");
        audio.GetComponent<AudioSource>().Play();

        DestroyProjectile();
    }
}
