using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{

    public float rotateSpeed = 1f;

    public ParticleSystem pickupParticle;

    public AudioSource pickupSource;
    public AudioClip pickupClip;

    // Start is called before the first frame update
    void Start()
    {
        pickupSource.clip = pickupClip;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateSpeed, rotateSpeed, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Make pickups invisible.
        if (collision.gameObject.name.Equals("FPSPlayer"))
        {
            WinConditions.pickups++;

            pickupSource.Play();

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;

            pickupParticle.Play();
        }
    }
}
