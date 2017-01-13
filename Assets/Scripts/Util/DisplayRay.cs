using UnityEngine;
using System.Collections;

/// <summary>
/// 画面端からレイを飛ばして背景に当たらなかったらカメラをそれ以上動かさないという作戦
/// </summary>
public class DisplayRay : MonoBehaviour {
	GameObject camera;
	int displayHelfWidth = 0;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Camera");
		displayHelfWidth = Screen.width / 2;
	}
	
	// Update is called once per frame
	void Update () {
		//Check ();

	}

	void Check () {
		Camera c = camera.GetComponent<Camera> ();
		// 端っこをとる
		Debug.Log(Screen.width);
		Vector3 tmp = new Vector3 (Screen.width, Screen.height / 2, camera.transform.position.z);
		// スクリーン座標からワールド座標へ変換
		Vector3 p = c.ScreenToWorldPoint (tmp);
		Debug.Log (p);
		Ray ray = new Ray (p, camera.transform.forward);
		Debug.DrawRay (p, camera.transform.forward * 20f,Color.red);

		RaycastHit[] hits = Physics.RaycastAll (ray, Mathf.Infinity);
		foreach (var obj in hits) {
			if (obj.collider.tag == "Background") {
				Debug.Log (obj.collider.name);
			}
		}
	}

	public Vector3 RangeCheck() {
		
		Camera c = camera.GetComponent<Camera> ();

		// 端っこをとる(現在は左だけ)
		var tmp = new Vector3 (0f, Screen.height / 2, (-c.transform.position.z));
		// スクリーン座標からワールド座標へ変換
		Vector3 p = c.ScreenToWorldPoint (tmp);
		// カメラから画面端の方向ベクトルを取得
		var dir = p - c.transform.position;

		Ray ray = new Ray (c.transform.position, dir);
		Debug.DrawRay (c.transform.position, dir * 20f,Color.green);

		RaycastHit[] hits = Physics.RaycastAll (ray, Mathf.Infinity);
		foreach (var obj in hits) {
			if (obj.collider.tag == "Background") {
				return obj.point;
			}
		}
		return new Vector3(-1000f,-1000f,-1000f);

		/*
		Ray ray = new Ray (p, camera.transform.forward);
		Debug.DrawRay (p, camera.transform.forward * 20f,Color.red);

		RaycastHit[] hits = Physics.RaycastAll (ray, Mathf.Infinity);
		foreach (var obj in hits) {
			if (obj.collider.tag == "Background") {
				return true;
			}
		}
		return false;
		*/
	}
}
