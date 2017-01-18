using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody rigid;
	float walkForce = 3f;
	float jumpPower = 300.0f;
	LimitLine limitLine;
	float sideBase = 6.15f;

	void Awake () {
		
		rigid = GetComponent<Rigidbody> ();
		limitLine = GameManager.GetInstance ().Limit;

	}

	// Update is called once per frame
	void Update () {
		
		Move ();

	}

	/// <summary>
	/// 移動
	/// </summary>
	void Move() {
		
		float moveHorizontal = Input.GetAxis ("Horizontal");

		if (moveHorizontal != 0) {

			rigid.velocity = new Vector3 (moveHorizontal * walkForce, rigid.velocity.y, rigid.velocity.z);

			ChangeAppearanceDirection (moveHorizontal);


		} else {
			// 待機アニメーションに変更
		}

		OverflowCorrection ();
		Jump ();
	}

	/// <summary>
	/// ジャンプ
	/// </summary>
	void Jump() {
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			
			float sizeY = gameObject.GetComponent<SphereCollider> ().radius;

			if (Physics.Raycast (transform.position, Vector3.down, sizeY + 0.2f)) {
				rigid.AddForce (Vector3.up * jumpPower);
			}

		}
	}

	/// <summary>
	/// プレイヤーの左右方向による見た目変更
	/// </summary>
	void ChangeAppearanceDirection(float horizontal) {

		if (horizontal > 0f) {
			transform.localScale = new Vector3 (0.5f, transform.localScale.y, transform.localScale.z);
		} else if (horizontal < 0f) {
			transform.localScale = new Vector3 (-0.5f, transform.localScale.y, transform.localScale.z);
		}

	}

	/// <summary>
	/// プレイヤーの移動範囲はみ出し補正
	/// </summary>
	void OverflowCorrection() {

		// x軸
		if (transform.position.x < limitLine.Left + sideBase) {
			transform.position = new Vector3 (limitLine.Left + sideBase, transform.position.y, transform.position.z);
		} else if (transform.position.x > limitLine.Right - sideBase) {
			transform.position = new Vector3 (limitLine.Right - sideBase, transform.position.y, transform.position.z);
		}

		// y軸
		if (transform.position.y < limitLine.Bottom) {
			transform.position = new Vector3 (transform.position.x, limitLine.Bottom, transform.position.z);
		} else if (transform.position.y > limitLine.Top) {
			transform.position = new Vector3 (transform.position.x, limitLine.Top, transform.position.z);
		}

	}
}
