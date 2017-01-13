using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// サウンド管理クラス
/// </summary>
public class SoundManager : MonoBehaviour {

	// サウンド種別
	enum soundType{
		BGM,
		SE,
	}

	// SEチャンネル数
	const int SE_CHANNEL = 4;

	// サウンド再生のためのゲームオブジェクト
	GameObject playObj = null;

	// サウンドリソース
	AudioSource sourceBGM = null;
	AudioSource sourceSE  = null;

	// SEチャンネル
	AudioSource[] sourceSEArray;

	// BGMにアクセスするためのテーブル
	Dictionary<string, Data> poolBGM =  new Dictionary<string, Data>();
	// SEにアクセスするためのテーブル
	Dictionary<string, Data> poolSE =  new Dictionary<string, Data>();

	// サウンド情報を保持するデータ
	class Data{

		// アクセス用のキー
		public string key;
		// リソース名
		public string resName;
		// AudioClip
		public AudioClip clip;
		// 音量
		public float volume;
		// ピッチ
		public float pitch;
		// ステレオ
		public float panStereo;
		// 音響
		public float spatialBlend;

		public Data(string key, string res, float volume, float pitch){

			this.key = key;
			resName = "Sounds/" + res;
			// AudioClipの取得
			clip = Resources.Load(resName) as AudioClip;
			// ステレオ・音響はデフォルト値
			panStereo = 0.0f;
			spatialBlend = 0.0f;

			// 音量が0.0f～1.0fの範囲を超えていたら、0.0fまたは1.0fに設定する
			if      (1.0f < volume) this.volume = 1.0f;
			else if (0.0f > volume) this.volume = 0.0f;
			else                    this.volume = volume;
			
			// ピッチが.0f～1.0fの範囲を超えていたら、0.0fまたは1.0fに設定する
			if      (-3.0f > pitch)  this.pitch = -3.0f;
			else if (3.0f < pitch)  this.pitch = 3.0f;
			else                    this.pitch = pitch;
		}
	}

	static SoundManager instance = null;

	public static SoundManager GetInstance() {

		return instance ?? (instance = new SoundManager ());
	}

	public SoundManager() {
		sourceSEArray = new AudioSource[SE_CHANNEL];
	}

	/// <summary>
	/// AudioSourceの取得
	/// </summary>
	/// <returns>AudioSourceコンポーネント</returns>
	/// <param name="type">サウンドの種別</param>
	/// <param name="channel">チャンネル</param>
	AudioSource GetAudioSource(soundType type, int channel = -1){

		if(playObj == null){
			// GameObjectがなければ作る
			playObj = new GameObject("SoundManager");
			// 破棄しないようにする
			GameObject.DontDestroyOnLoad(playObj);
			// AudioSourceを作成
			sourceBGM = playObj.AddComponent<AudioSource> ();
			sourceSE  = playObj.AddComponent<AudioSource> ();
			for(int i = 0; i < SE_CHANNEL; i++){
				sourceSEArray [i] = playObj.AddComponent<AudioSource> ();
			}
		}

		if(type == soundType.BGM){
			return sourceBGM;
		} else {
			if(0 == channel && channel < SE_CHANNEL){
				// チャンネル指定
				return sourceSEArray[channel];
			} else {
				// デフォルト
				return sourceSE;
			}
		}
	}

	/// <summary>
	/// BGMの読込
	/// </summary>
	/// <param name="key">アクセス用のキー</param>
	/// <param name="resName">リソース名</param>
	/// <param name="volume">音量</param>
	/// <param name="pitch">ピッチ</param>
	public static void LoadBGM(string key, string resName, float volume = 1.0f, float pitch = 1.0f){
		GetInstance()._LoadBGM (key, resName, volume, pitch);
	}

	void _LoadBGM(string key, string resName, float volume, float pitch){
		if (poolBGM.ContainsKey (key)) {
			// 既に登録済みなのでいったん消す
			poolBGM.Remove (key);
		}
		poolBGM.Add (key, new Data (key, resName, volume, pitch));
	}

	/// <summary>
	/// SEの読込
	/// </summary>
	/// <param name="key">アクセス用のキー</param>
	/// <param name="resName">リソース名</param>
	/// <param name="volume">音量</param>
	/// <param name="pitch">ピッチ</param>
	public static void LoadSE(string key, string resName, float volume = 1.0f, float pitch = 1.0f){
		GetInstance()._LoadSE (key, resName, volume, pitch);
	}
		
	void _LoadSE(string key ,string resName, float volume, float pitch){
		if(poolSE.ContainsKey(key)){
			// 既に登録済みなのでいったん消す
			poolSE.Remove (key);
		}
		poolSE.Add (key, new Data (key, resName, volume, pitch));
	}

	/// <summary>
	/// BGMの再生
	/// </summary>
	/// <returns><c>true</c>, 有効, <c>false</c> 無効, </returns>
	/// <param name="key">アクセス用のキー</param>
	public static bool PlayBGM(string key){
		return GetInstance()._PlayBGM (key);
	}

	bool _PlayBGM(string key){
		if (poolBGM.ContainsKey (key) == false) {
			// 対応するキーがない
			return false;
		}

		// いったん止める
		StopBGM ();

		// リソースの取得
		var data = poolBGM [key];

		// 再生
		var source    = GetAudioSource (soundType.BGM);
		source.loop   = true;
		source.clip   = data.clip;
		source.volume = data.volume;
		source.pitch  = data.pitch;
		source.Play ();

		return true;

	}
	/// <summary>
	/// BGMの停止
	/// </summary>
	/// <returns><c>true</c>, 有効, <c>false</c> 無効, </returns>
	public static bool StopBGM(){
		return GetInstance()._StopBGM ();
	}

	bool _StopBGM(){
		GetAudioSource (soundType.BGM).Stop ();
		return true;
	}

	/// <summary>
	/// BGMの一時停止
	/// </summary>
	/// <returns><c>true</c>, 有効, <c>false</c> 無効, </returns>
	public static bool PauseBGM(){
		return GetInstance()._PauseBGM ();
	}

	bool _PauseBGM(){
		GetAudioSource (soundType.BGM).Pause ();
		return true;
	}

	/// <summary>
	/// BGMの一時停止の解除
	/// </summary>
	/// <returns><c>true</c>, 有効, <c>false</c> 無効, </returns>
	public static bool UnPauseBGM(){
		return GetInstance()._UnPauseBGM ();
	}

	bool _UnPauseBGM(){
		GetAudioSource (soundType.BGM).UnPause ();
		return true;
	}

	/// <summary>
	/// SEの再生
	/// </summary>
	/// <returns><c>true</c>, 有効, <c>false</c> 無効, </returns>
	/// <param name="key">アクセス用のキー</param>
	public static bool PlaySE(string key, int channel = -1){
		return GetInstance()._PlaySE (key,channel);
	}

	bool _PlaySE(string key, int channel = -1){
		if (poolSE.ContainsKey (key) == false) {
			// 対応するキーがない
			return false;
		}

		// リソースの取得
		var data = poolSE [key];

		if (0 <= channel && channel < SE_CHANNEL) {
			// 再生
			var source    = GetAudioSource (soundType.SE);
			source.clip   = data.clip;
			source.volume = data.volume;
			source.pitch  = data.pitch;
			source.Play ();
		} else {
			// デフォルトで再生
			var source = GetAudioSource (soundType.SE);
			source.volume = data.volume;
			source.pitch  = data.pitch;
			source.PlayOneShot (data.clip);
		}
		return true;
	}

	/// <summary>
	/// 音量の設定
	/// BGM、SE両対応
	/// 0.0f（小）～1.0f（大）の範囲で有効
	/// </summary>
	/// <returns><c>true</c>, 有効, <c>false</c> 無効, </returns>
	/// <param name="key">アクセス用のキー</param>
	/// <param name="volume">設定する音量</param>
	public static bool SetVolume(string key, float volume) {
		return GetInstance()._SetVolume (key, volume);
	}

	bool _SetVolume(string key, float volume) {

		if (poolBGM.ContainsKey (key) == false && poolSE.ContainsKey (key) == false) {
			// 対応するキーがない
			return false;
		}
			
		// 0.0f～1.0fの範囲外なら設定しない
		if (0.0f > volume || 1.0f < volume) return false;

		Data data = new Data("non", "non", 1.0f, 1.0f);	// エラー回避のため仮で値を入れる
		AudioSource source = new AudioSource();

		if (poolBGM.ContainsKey (key)) {

			data = poolBGM [key];
			source = GetAudioSource (soundType.BGM);
		} else if (poolSE.ContainsKey (key)) {

			data = poolSE [key];
			source = GetAudioSource (soundType.SE);
		}

		// 音量設定
		source.clip   = data.clip;
		source.volume = volume;
		return true;
	}

	/// <summary>
	/// ピッチの設定
	/// BGM、SE両対応
	/// -3.0f（遅い?）～3.0f（早い?）の範囲で有効(1.0fが標準)
	/// </summary>
	/// <returns><c>true</c>, 有効, <c>false</c> 無効, </returns>
	/// <param name="key">アクセス用のキー</param>
	/// <param name="volume">設定するピッチ</param>
	public static bool SetPitch(string key, float pitch) {
		return GetInstance()._SetPitch (key, pitch);
	}

	bool _SetPitch(string key, float pitch) {

		if (poolBGM.ContainsKey (key) == false && poolSE.ContainsKey (key) == false) {
			// 対応するキーがない
			return false;
		}

		// 0.0f～1.0fの範囲外なら設定しない
		if (-3.0f > pitch || 3.0f < pitch) return false;

		Data data = new Data("non", "non", 1.0f, 1.0f);	// エラー回避のため仮で値を入れる
		AudioSource source = new AudioSource();

		if (poolBGM.ContainsKey (key)) {

			data = poolBGM [key];
			source = GetAudioSource (soundType.BGM);
		} else if (poolSE.ContainsKey (key)) {

			data = poolSE [key];
			source = GetAudioSource (soundType.SE);
		} else
			Debug.Log ("Error :: Pitch");

		// ピッチ設定
		source.clip = data.clip;
		source.pitch = pitch;
		return true;
	}

	/// <summary>
	/// ステレオの設定
	/// BGM、SE両対応
	/// -1.0f（左）～1.0f（右）の範囲で有効
	/// </summary>
	/// <returns><c>true</c>, 有効, <c>false</c> 無効, </returns>
	/// <param name="key">アクセス用のキー</param>
	/// <param name="volume">設定するステレオ</param>
	public static bool SetStereoPan(string key, float stereo) {
		return GetInstance()._SetStereoPan (key, stereo);
	}

	bool _SetStereoPan(string key, float stereo) {
		
		if (poolBGM.ContainsKey (key) == false && poolSE.ContainsKey (key) == false) {
			// 対応するキーがない
			return false;
		}

		// 0.0f～1.0fの範囲外なら設定しない
		if (-1.0f > stereo || 1.0f < stereo) return false;

		Data data = new Data("non", "non", 1.0f, 1.0f);	// エラー回避のため仮で値を入れる
		AudioSource source = new AudioSource();

		if (poolBGM.ContainsKey (key)) {

			data = poolBGM [key];
			source = GetAudioSource (soundType.BGM);
		} else if (poolSE.ContainsKey (key)) {

			data = poolSE [key];
			source = GetAudioSource (soundType.SE);
		}

		// ステレオ設定
		source.clip = data.clip;
		source.panStereo = stereo;
		return true;
	}

	/// <summary>
	/// 音響の設定
	/// BGM、SE両対応
	/// 0.0f（2D）～1.0f（3D）の範囲で有効
	/// </summary>
	/// <returns><c>true</c>, 有効, <c>false</c> 無効, </returns>
	/// <param name="key">アクセス用のキー</param>
	/// <param name="volume">設定するステレオ</param>
	public static bool SetSpatialBlend(string key, float sb) {
		return GetInstance()._SetSpatialBlend (key, sb);
	}

	bool _SetSpatialBlend(string key, float sb) {

		if (poolBGM.ContainsKey (key) == false && poolSE.ContainsKey (key) == false) {
			// 対応するキーがない
			return false;
		}

		// 0.0f～1.0fの範囲外なら設定しない
		if (0.0f > sb || 1.0f < sb) return false;

		Data data = new Data("non", "non", 1.0f, 1.0f);	// エラー回避のため仮で値を入れる
		AudioSource source = new AudioSource();

		if (poolBGM.ContainsKey (key)) {

			data = poolBGM [key];
			source = GetAudioSource (soundType.BGM);
		} else if (poolSE.ContainsKey (key)) {

			data = poolSE [key];
			source = GetAudioSource (soundType.SE);
		}

		// 音響設定
		source.clip = data.clip;
		source.spatialBlend = sb;
		return true;
	}
}
