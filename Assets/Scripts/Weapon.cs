using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] float range =100f;
    [SerializeField] float damage = 30f;
    [SerializeField] ParticleSystem bang;
    [SerializeField] GameObject hitEffect;
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            shoot();
        }

        void shoot()
        {
            playBang();
            processRaycast();
        }

        void playBang()
        {
            bang.Play();
        }
        void processRaycast()
        {
            RaycastHit hit;
            if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
            {
                CreateHitimapct(hit);
                EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
                if (target == null) return;
                target.TakeDamage(damage);
            }
            else return;
        }

        void CreateHitimapct(RaycastHit hit)
        {
           GameObject impact= Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 0.1f);
        }
    }
}
