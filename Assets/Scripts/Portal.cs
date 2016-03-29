using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public int portalID;
	public PortalManager portals;

	// Use this for initialization
	void Start () {
		//Debug.DrawRay (this.transform.position, this.transform.forward * 10, Color.red, 100);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Vector3 newPos = other.gameObject.transform.position;
		Quaternion newRot = other.gameObject.transform.rotation;

		bool canPortal = portals.portalMove(gameObject.GetComponent<Portal>().portalID, ref newPos, ref newRot);

		if (canPortal) {
			other.gameObject.transform.position = newPos;
			other.gameObject.transform.rotation = newRot;
		}
	}

}