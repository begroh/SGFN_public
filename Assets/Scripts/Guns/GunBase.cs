using UnityEngine;
using System.Collections;

public abstract class GunBase : MonoBehaviour {

    public Bullet bullet;
    public float spread;
    public float numBullets;
    public float reloadTime;
    public float bulletSpeed;

    private float lastFireTime;
    private Vector2 launchVec;

    void Start()
    {
        lastFireTime = Time.time;
        launchVec = new Vector2(bulletSpeed, 0);
    }

    public void Fire()
    {
        if (lastFireTime + reloadTime < Time.time)
        {
            for (int i = 0; i < numBullets; i++)
            {
                SpawnBullet();
            }
        }
    }

    private void SpawnBullet()
    {
        Bullet spawnedBullet = (Bullet) Instantiate(bullet);
        Rigidbody2D body = spawnedBullet.GetComponent<Rigidbody2D>();
        body.velocity = launchVec;
        spawnedBullet.transform.Rotate(Random.Range(spread, spread * -1), 0, 0);
    }

}
