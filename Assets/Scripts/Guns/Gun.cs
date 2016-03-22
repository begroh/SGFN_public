using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public Bullet bullet;
    public float spreadForce;
    public float numBullets;
    public float reloadTime;
    public float launchForce;
    public float bulletRange;
    public bool holdToFire = false;

    private float lastFireTime;
    private float distFromBarrelEnd = 1.4f;

    void Start()
    {
        lastFireTime = Time.time;
    }

    public void Fire()
    {
        if (lastFireTime + reloadTime < Time.time)
        {
            for (int i = 0; i < numBullets; i++)
            {
                SpawnBullet();
            }
            lastFireTime = Time.time;
        }
    }

    public void Copy(Gun otherGun)
    {
        spreadForce = otherGun.spreadForce;
        numBullets = otherGun.numBullets;
        reloadTime = otherGun.reloadTime;
        launchForce = otherGun.launchForce;
        bulletRange = otherGun.bulletRange;
        holdToFire = otherGun.holdToFire;
    }

    // Turns the gun back into a pistol. Sorry that the code is so bad.
    // When we need different types of guns i'll abstract this out so the code
    // isn't so horrible.
    //                      - Nick
    public void Reset()
    {
        spreadForce = 900f;
        numBullets = 1;
        reloadTime = 0.1f;
        launchForce = 5000f;
        bulletRange = 30f;
        holdToFire = false;
    }

    private void SpawnBullet()
    {
        Vector3 spawnLoc = transform.position + transform.forward * distFromBarrelEnd;
        Bullet spawnedBullet = (Bullet) Instantiate(bullet, spawnLoc, Quaternion.identity);
        spawnedBullet.transform.rotation = transform.rotation;
        spawnedBullet.range = bulletRange;

        Rigidbody2D body = spawnedBullet.GetComponent<Rigidbody2D>();
        body.AddForce(transform.right * launchForce);
        body.AddForce(transform.up * Random.Range(-spreadForce, spreadForce));
    }
}
