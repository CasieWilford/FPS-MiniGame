using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackage : MonoBehaviour
{
    public static bool noMoreAmmo;

    public ParticleSystem ammoPickUp;

    // Allows player to reload and destroys itself.
    private void OnCollisionEnter(Collision collision)
    {
        // When ammo is touched and  there is no more ammo left, the player can then pick up the ammo pack.
        if (collision.gameObject.name.Equals("FPSPlayer") && noMoreAmmo)
        {
            noMoreAmmo = false;
            GunScript.beginReloading = true;
            Destroy(gameObject);

            ammoPickUp.Play();
            Debug.Log("Ammo picked up");
            //healthParticle.Play();
            //pickupAudioSource.Play();
        }
    }
}
