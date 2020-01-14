using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    bool isGrabbable = true;

    float timer = 0;
    public float duration = 10f;
    public float rotateSpeed = 1f;

    public GameObject cap;
    public GameObject body;

    public ParticleSystem healthParticle;

    public AudioClip pickupAudioClip;
    public AudioSource pickupAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        pickupAudioSource.clip = pickupAudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        // Makes health powerup rotate.
        transform.Rotate(rotateSpeed, 0f, 0f);

        if (!isGrabbable)
        {
            if (timer < duration)
                timer += Time.deltaTime;
            else
            {
                // Make visible
                cap.GetComponent<MeshRenderer>().enabled = true;
                body.GetComponent<MeshRenderer>().enabled = true;
                gameObject.GetComponent<Collider>().enabled = true;
                //particles.SetActive(true);
                //particleBurst.SetActive(false);
                timer = 0;

                isGrabbable = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("FPSPlayer"))
        {
            if (isGrabbable)
            {
                // Make invisible.
                cap.GetComponent<MeshRenderer>().enabled = false;
                body.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<Collider>().enabled = false;
                //particles.SetActive(false);
                isGrabbable = false;
                //particleBurst.SetActive(true);

                healthParticle.Play();
                pickupAudioSource.Play();
            }

        }
    }
}
