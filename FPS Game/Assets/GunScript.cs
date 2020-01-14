using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float explosiveDamage = 50f;

    float shots = 0;
    float poweredShots = 0;
    //float poweredFireAmount = 0;

    float fireRate;
    float nextFire;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject explosiveImpactEffect;

    // If player is dead, player cannot shoot.
    public static bool playerDied;

    [Header("Powered Reloading")]
    public static bool pBeginReloading;
    public int pMaxAmmo = 3;
    private int pCurrentAmmo;
    public float pReloadTime = 1f;
    private bool pIsReloading = false;

    [Header("Reloading")]
    public static bool beginReloading;
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Animator animator;

    [Header("Bullet UI")]
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;
    public GameObject bullet5;
    public GameObject bullet6;
    public GameObject bullet7;
    public GameObject bullet8;
    public GameObject bullet9;
    public GameObject bullet10;

    public GameObject chargedBullet1;
    public GameObject chargedBullet2;
    public GameObject chargedBullet3;


    [Header("Audio")]
    // Gun shot audio.
    public AudioClip gunShotClip;
    public AudioSource gunShotSource;

    // Bullet impact audio.
    public AudioClip bulletImpactClip;
    public AudioSource bulletImpactSource;

    // Explosive bullet impact audio.
    public AudioClip explosiveImpactClip;
    public AudioSource explosiveImpactSource;

    // Reloading audio.
    public AudioClip reloadingClip;
    public AudioSource reloadingSource;

    /*[Header("Recoil")]
    public GameObject weapon;
    public Transform recoilMod;

    public float recoil = 0f;
    public float recoil = 0f;
    float maxRecoil_x = -20f;
    float recoilSpeed = 10;
    float minRecoil = 2;*/


    void Start()
    {
        gunShotSource.clip = gunShotClip;
        bulletImpactSource.clip = bulletImpactClip;
        explosiveImpactSource.clip = explosiveImpactClip;
        reloadingSource.clip = reloadingClip;

        currentAmmo = maxAmmo;
        pCurrentAmmo = pMaxAmmo;

        fireRate = 3f;
        nextFire = Time.time;
    }

    // Update for Second Ability reloading.
    void FixedUpdate()
    {
        if (pIsReloading)
            return;

        if (pCurrentAmmo <= 0)
            PoweredAmmoPack.noMorePoweredAmmo = true;

        // If the player has no ammo and collides with an ammo pack, then it will reload.
        if (pCurrentAmmo <= 0 && pBeginReloading)
        {
            StartCoroutine(PoweredReload());

            reloadingSource.Play();

            DisplayPoweredAmmo();

            return;
        }

        // When there is no more ammo, the player can no long shoot.
        if (pCurrentAmmo <= 0)
            return;

        if (Input.GetButtonDown("Fire2") && Time.time > nextFire && !playerDied)
        {
            ExplosiveShoot();
            gunShotSource.Play();

            // 5 second timer between powered up shots.
            nextFire = Time.time + fireRate;

            StopDisplayPoweredAmmo();

            PlayerHealth.poweredShotActivated = true;
        }
    }

    // Second Ability reloading.
    IEnumerator PoweredReload()
    {
        pIsReloading = true;
        Debug.Log("Reload p ");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(pReloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);


        pCurrentAmmo = pMaxAmmo;
        pIsReloading = false;
        pBeginReloading = false;

        PoweredAmmoPack.noMorePoweredAmmo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;

        // If there is no more ammo, player can pick up ammo pack.
        if (currentAmmo <= 0)
            AmmoPackage.noMoreAmmo = true;

        if (currentAmmo <= 0 && beginReloading)
        {
            Debug.Log("stop");
            StartCoroutine(Reload());

            reloadingSource.Play();

            DisplayAmmoUI();
            
            return;
        }

        if (currentAmmo <= 0)
            return;

        if (Input.GetButtonDown("Fire1") && !playerDied)
        {
            Shoot();
            gunShotSource.Play();

            StopDisplayAmmoUI();

            //recoil += .1f;
            //Recoiling();
        }
    }
    
    // Gun Reload Function.
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);

        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
        beginReloading = false;

        AmmoPackage.noMoreAmmo = false;
    }


    void Shoot()
    {
        // Plays gun particle effect when shot.
        muzzleFlash.Play();

        currentAmmo--;

        // Detects object infront of gun and damages if target.
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            EnemyBehavior target = hit.transform.GetComponent<EnemyBehavior>();

            EnemyPathing target2 = hit.transform.GetComponent<EnemyPathing>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            
            if(target2 != null)
            {
                target2.TakeDamage(damage);
            }
           

            // Instantiate impact effect and deletes after 1 second.
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);

            if (impactGO != null)
            {
                bulletImpactSource.Play();
            }
        }
    }

    void ExplosiveShoot()
    {
        muzzleFlash.Play();

        pCurrentAmmo--;

        // Detects object infront of gun and damages if target.
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            EnemyBehavior target = hit.transform.GetComponent<EnemyBehavior>();

            EnemyPathing target2 = hit.transform.GetComponent<EnemyPathing>();

            if (target != null)
            {
                target.TakeDamage(explosiveDamage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            if (target2 != null)
            {
                target2.TakeDamage(explosiveDamage);
            }

            // Instantiate explosive impact effect and deletes after 1 second.
            GameObject impactGO2 = Instantiate(explosiveImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO2, 2f);

            if (impactGO2 != null)
            {
                explosiveImpactSource.Play();
            }

        }
    }

    // Gets rid of one bullet for every bullet shot.
    void StopDisplayAmmoUI()
    {
        shots++;
        Debug.Log(shots);

        if (shots == 1)
            bullet1.gameObject.SetActive(false);
        else if (shots == 2)
            bullet2.gameObject.SetActive(false);
        else if (shots == 3)
            bullet3.gameObject.SetActive(false);
        else if (shots == 4)
            bullet4.gameObject.SetActive(false);
        else if (shots == 5)
            bullet5.gameObject.SetActive(false);
        else if (shots == 6)
            bullet6.gameObject.SetActive(false);
        else if (shots == 7)
            bullet7.gameObject.SetActive(false);
        else if (shots == 8)
            bullet8.gameObject.SetActive(false);
        else if (shots == 9)
            bullet9.gameObject.SetActive(false);
        else if (shots == 10)
        {
            bullet10.gameObject.SetActive(false);

            shots = 0;
        }
    }

    // Resets the bullets once gun reloads.
    void DisplayAmmoUI()
    {
        bullet1.gameObject.SetActive(true);
        bullet2.gameObject.SetActive(true);
        bullet3.gameObject.SetActive(true);
        bullet4.gameObject.SetActive(true);
        bullet5.gameObject.SetActive(true);
        bullet6.gameObject.SetActive(true);
        bullet7.gameObject.SetActive(true);
        bullet8.gameObject.SetActive(true);
        bullet9.gameObject.SetActive(true);
        bullet10.gameObject.SetActive(true);
    }


    // Display for Power Ammo.
    void StopDisplayPoweredAmmo()
    {
        poweredShots++;
        
        if (poweredShots == 1)
            chargedBullet1.gameObject.SetActive(false);
        else if (poweredShots == 2)
            chargedBullet2.gameObject.SetActive(false);
        else if (poweredShots == 3)
        {
            chargedBullet3.gameObject.SetActive(false);

            poweredShots = 0;
        }
    }

    void DisplayPoweredAmmo()
    {
        chargedBullet1.gameObject.SetActive(true);
        chargedBullet2.gameObject.SetActive(true);
        chargedBullet3.gameObject.SetActive(true);
    }

    



    /*void Recoiling()
    {
        if (recoil > 0)
        { 
            var maxRecoil = Quaternion.Euler (maxRecoil_x, 0, 0);

            recoilMod.rotation = Quaternion.Slerp(recoilMod.rotation, maxRecoil, Time.deltaTime * recoilSpeed);
            //weapon.transform.localEulerAngles.x = recoilMod.localEulerAngles.x;
            recoil -= Time.deltaTime;

        }
        else
        {
            recoil = 0;
            var minRecoil = Quaternion.Euler(0, 0, 0);

            recoilMod.rotation = Quaternion.Slerp(recoilMod.rotation, minRecoil, Time.deltaTime * recoilSpeed / 2);
            //gameObject.transform.localEulerAngles.x = recoilMod.localEulerAngles.x;
        }
    }*/
}

