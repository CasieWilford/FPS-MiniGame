using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float startHealth = 100;
    private float health;

    public static bool poweredShotActivated;

    bool addHealth = false;

    public GameObject deathPanel;
    public GameObject powerup1;
    public GameObject pathingEnemy;

    public AudioSource deathJingleSource;
    public AudioClip deathJingleClip;

    //public GameObject powerup2;
    
    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;

        deathJingleSource.clip = deathJingleClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 100 && addHealth)
        {
            AddHealth(1);
        }
        else
            addHealth = false;

        // Takes damage when player uses second ability.
        if (Input.GetButtonDown("Fire2") && poweredShotActivated)
        {
            TakeDamage(20);
            poweredShotActivated = false;
        }
    }

   
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            TakeDamage(25f);
        }
        
        if(other.gameObject.Equals(powerup1))
        {
            // Power up 1's power
            addHealth = true;
            Debug.Log("Adding health");
        }
    }

    public void AddHealth(float amount)
    {
        health += amount;

        healthBar.fillAmount = health / startHealth;

        if (health > 100f)
        {
            health = 100f;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0f)
        {
            Debug.Log("you die");
            deathPanel.SetActive(true);

            deathJingleSource.Play();

            // Do something in here that shows the user they fuckin died
            gameObject.GetComponent<Rigidbody>().AddExplosionForce(1500f, gameObject.transform.position, 5);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.Equals(powerup1) && health < 100)
        {
            // Power up 1's power
            addHealth = true;
            Debug.Log("Adding health");
        }

        if (collision.gameObject.Equals(pathingEnemy))
        {
            TakeDamage(10);
        }
    }


}
