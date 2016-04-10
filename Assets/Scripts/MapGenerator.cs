using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class MapGenerator : MonoBehaviour {

	public TextAsset map;
	[Serializable]
	public struct tile {
		public char character;
		public GameObject go;
		public float rotation;
	}
	public tile[] tileList;
	private Dictionary<char, tile> tileDict = new Dictionary<char, tile>();


	void Start () {
		foreach (tile t in tileList) {
			tileDict [t.character] = t;
		}

		loadMap (map);
	}

	// Read in the file
	void loadMap(TextAsset file) {
		try {
			string[] lines = file.text.Trim().Split('\n');
			int rows = lines.Length;
			int maxLineLength = 0;
			string line;

			for (int y = rows-1; y >= 0; y--) {
				line = lines[y];
				if (line.Length > maxLineLength) {
					maxLineLength = line.Length;
				}

				for (int x = 0; x < line.Length; x++) {
					Vector3 position = new Vector3(x, rows-y-1, 0f);
					placeObject(line[x], position);
				}
			}

			positionCamera (maxLineLength, lines.Length);

		} catch (Exception e) {
			print ("Error loading map");
			print (e);
		}
	}

	// Instantiate and place the prefab specified
	void placeObject(char c, Vector3 pos) {
		if (c == '*') {
			return;
		}

		if (!tileDict.ContainsKey (c)) {
			print ("Error: unrecognized character " + c);
		}

		GameObject prefab = tileDict [c].go;

		if (prefab != null) {
			GameObject go = Instantiate (prefab);
			go.transform.position = pos;
			go.transform.rotation = Quaternion.Euler (0, 0, tileDict[c].rotation);
			go.transform.parent = this.gameObject.transform;
		}
	}

	void positionCamera(int mapWidth, int mapHeight) {
		float tempZ = Camera.main.transform.position.z;
		Camera.main.transform.position = new Vector3((mapWidth - 1) / 2.0f, (mapHeight - 1) / 2.0f, tempZ);
		Camera.main.GetComponent<Camera> ().orthographicSize = (mapHeight + 1) / 2;
	}
}