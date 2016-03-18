using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public Bullet bullet;
    public float spread;
    public float numBullets;
    public float reloadTime;
    public float launchForce;

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

    private void SpawnBullet()
    {
        Vector3 spawnLoc = transform.position + transform.forward * distFromBarrelEnd;
        Bullet spawnedBullet = (Bullet) Instantiate(bullet, spawnLoc, Quaternion.identity);
	spawnedBullet.transform.rotation = transform.rotation;

        Rigidbody2D body = spawnedBullet.GetComponent<Rigidbody2D>();
        body.AddForce(transform.right * launchForce);
    }
}
