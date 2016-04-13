using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortalManager : MonoBehaviour {

	public static PortalManager P;
	public List<Portal> portalList = new List<Portal>();
	Dictionary<int, int> portalMap = new Dictionary<int, int>();

	public enum PortalMovement {
		pairs,
		cyclic,
		random
	}
	public PortalMovement portalMovement = PortalMovement.pairs;

	bool canPortal = true;

	void Awake () {
		P = this;
	}

	void Start () {
		if (portalMovement == PortalMovement.pairs) {
			for (int i=0; i< portalList.Count; i += 2) {
				if (i < portalList.Count - 1) {
					portalMap [i] = i + 1;
					portalMap [i + 1] = i;
				} else {
					portalMap [i] = i;
				}
			}
		} else if (portalMovement == PortalMovement.cyclic) {
			for (int i=0; i< portalList.Count; i++) {
				if (i == portalList.Count - 1) {
					portalMap [i] = 0;
				} else {
					portalMap [i] = i + 1;
				}
			}

		}
	}

	public bool portalMove(int portalNum, ref Vector3 pos, ref Quaternion rot) {
		if (!canPortal) {
			return false;
		}

		int nextPortal = portalNum;

		if (portalMovement == PortalMovement.random) {
			while (nextPortal == portalNum) {
				nextPortal = (Random.Range (0, portalList.Count));
			}
		} else {
			nextPortal = portalMap [portalNum];
		}

		// Otherwise you'll just bounce back and forth between the portals
		canPortal = true;
		//Invoke ("turnOnPortal", .5f);

		rot = portalList [nextPortal].transform.rotation; // - portalList [portalNum].transform.rotation;

		//Vector3 offset = portalList [nextPortal].transform.forward * (portalList [nextPortal].transform.localScale.x/2.1f);
		Vector3 offset = portalList[nextPortal].transform.right * 5.8f;

		pos = portalList[nextPortal].transform.position + offset;

		return true;
	}

	void turnOnPortal() {
		canPortal = true;
	}
}