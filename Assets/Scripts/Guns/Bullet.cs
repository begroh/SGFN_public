using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float range;

    private Vector3 startLoc;

    void Start()
    {
        startLoc = transform.position;
    }

    void Update()
    {
        if (Mathf.Abs(Vector3.Distance(startLoc, transform.position)) > range)
	    Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }

}
