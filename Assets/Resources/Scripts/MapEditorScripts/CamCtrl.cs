using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour {

	private Vector2 mouseLook;
	private Vector2 smooth;
	public float sens = 5.0f;
	public float sming = 2.0f;

	private GameObject character;

	void Start () {
		character = this.transform.parent.gameObject;
	}

	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			Vector2 mouseData = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));
			mouseData = Vector2.Scale (mouseData, 
				new Vector2 (sens * sming, sens * sming));

				smooth.x = Mathf.Lerp (smooth.x, mouseData.x, 1f / sming);
			    smooth.y = Mathf.Lerp (smooth.y, mouseData.y, 1f / sming);
				mouseLook += smooth;
			transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right); 
			character.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, character.transform.up);

		}
	}
}
