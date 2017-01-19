using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalOfSnowController : MonoBehaviour {

	float speed = 0.06f;
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y - speed, transform.position.z);
		transform.Rotate (0,0,1f);
		if (transform.position.y < -10f) {
			Destroy (gameObject);
		}
	}
}
