using UnityEngine;
using System.Collections;

public class Stage_02 : MonoBehaviour {

	LimitLine limitLine = new LimitLine(10f, -2f, -8f, 30f);
	string BGM = "yukinomauhamabe";

	void Awake() {

		GameManager.GetInstance ().Limit = limitLine;
		SoundManager.LoadBGM ("bgm", BGM);
		SoundManager.PlayBGM ("bgm");

		var snow = (GameObject)Resources.Load ("Prefabs/Stage/Stage_02/Generator/CrystalOfSnowGenerator");
		var obj = Instantiate (snow);
		obj.name = snow.name;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
