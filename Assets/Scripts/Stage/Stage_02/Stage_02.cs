using UnityEngine;
using System.Collections;

public class Stage_02 : MonoBehaviour {

	LimitLine limitLine = new LimitLine(10f,- 5f, -13f, 18f);

	void Awake() {
		GameManager.GetInstance ().Limit = limitLine;
		SoundManager.LoadBGM ("bgm", "yukinomauhamabe");
		SoundManager.PlayBGM ("bgm");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
