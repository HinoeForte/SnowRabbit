using UnityEngine;
using System.Collections;

/// <summary>
/// カメラコントローラークラス
/// 主にプレイシーンのプレイヤーを追尾する
/// </summary>
public class CameraController : MonoBehaviour {

	Vector3 offset = new Vector3 (0f, 1f, -5f);

	GameObject player;
	LimitLine limitLine;
	float sideBase = 10f;

	// Use this for initialization
	void Awake () {
		
		player = GameObject.Find ("Player");
		limitLine = GameManager.GetInstance ().Limit;
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + offset.y, offset.z);
	
	}

	// Update is called once per frame
	void Update () {

		PlayerWatch ();

	}

	/// <summary>
	/// プレイヤーに注目する
	/// </summary>
	void PlayerWatch() {

		var pos = new Vector3 ();
		pos = new Vector3 (player.transform.position.x, player.transform.position.y + offset.y, offset.z);

		// x軸はみ出し補正
		if (player.transform.position.x < (limitLine.Left + sideBase)) {
			pos.x = limitLine.Left + sideBase;
		}else if (player.transform.position.x > (limitLine.Right - sideBase)) {
			pos.x = limitLine.Right - sideBase;
		} 
		/*
		// y軸はみ出し補正
		if (player.transform.position.y < limitLine.Bottom) {
			pos.y = limitLine.Bottom;
		}else if (player.transform.position.y > limitLine.Top) {
			pos.y = limitLine.Top;
		} 
		*/
		transform.position = pos;

	}
}
