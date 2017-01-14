using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	static GameManager instance = null;

	// スクロールの限界値
	LimitLine limitLine;

	public static GameManager GetInstance() {
		return instance ?? (instance = new GameManager ());
	}
	void Awake() {
		// 現在のシーン名を取得して空のオブジェクトを生成し、その名前のスクリプトを実行させる
		string stage = SceneManager.GetActiveScene ().name;
		var scene = new GameObject ();
		scene.name = stage;
		scene.AddComponent(Type.GetType(stage));

		// プレイヤーの生成
		var player = (GameObject)Resources.Load ("Prefabs/Stage/Player");
		var obj = Instantiate (player, new Vector3(0f,0f,0f), Quaternion.identity);
		obj.name = player.name;

		// カメラの生成
		var camera = (GameObject)Resources.Load ("Prefabs/Stage/Camera");
		obj = Instantiate (camera, new Vector3(0f,0f,0f), Quaternion.identity);
		obj.name = camera.name;
	}

	// Update is called once per frame
	void Update () {
	
	}
		
	/// <summary>
	/// limitLineのgetterとsetter
	/// </summary>
	/// <value>The limit.</value>
	public LimitLine Limit {
		get{ return limitLine; }
		set{ limitLine = value; }
	}
}
