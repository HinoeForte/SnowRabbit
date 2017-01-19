using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	LimitLine limitLine = new LimitLine(10f, -2f, -8f, 30f);
	string BGM = "yukinomauhamabe";

	void Awake() {
		
		GameManager.GetInstance ().Limit = limitLine;
		SoundManager.LoadBGM ("bgm", BGM);
		SoundManager.PlayBGM ("bgm");

	}
		
	// Update is called once per frame
	void Update () {
	
	}
}
