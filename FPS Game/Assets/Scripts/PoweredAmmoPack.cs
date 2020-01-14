using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredAmmoPack : MonoBehaviour
{
    public static bool noMorePoweredAmmo;

    public ParticleSystem ammoPickUp;

    // Allows player to reload and destroys itself.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("FPSPlayer") && noMorePoweredAmmo)
        {
            noMorePoweredAmmo = false;
            GunScript.pBeginReloading = true;
            Destroy(gameObject);

            ammoPickUp.Play();

            //healthParticle.Play();
            //pickupAudioSource.Play();
        }
    }
}
