﻿
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float impactForce;

    private float nextTimeToFire = 0f;
    public int maxAmmo = 10;
    public float currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Animator animator;

    public Camera fpsCam;

    void Start()
    {
        currentAmmo = maxAmmo;
        animator.GetComponent<Animator>();
    }

    void OnEnable()
        {
            isReloading = false;
            animator.SetBool("Reloading", false);
        }
    

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;


        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            
            shoot();
        }
    }

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
    }

    void shoot()
    {
        muzzleFlash.Play();

        currentAmmo--;

        RaycastHit hit;
      if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
      {
          Debug.Log(hit.transform.name);

          Target target = hit.transform.GetComponent<Target>();
          if (target != null)
          {
              target.takeDamage(damage);
          }

          if (hit.rigidbody != null)
          {
              hit.rigidbody.AddForce(hit.normal * impactForce);
          }

          GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
          Destroy(impactGo, 2f);
      }

    }
}
