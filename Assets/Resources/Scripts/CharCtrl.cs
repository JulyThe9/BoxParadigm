using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCtrl : MonoBehaviour {
	
	public float speed = 7f;  // use getcomponent instead of static! 
	public float yLowBound = 2f;
	//private string surfName;
    private EditorUI editorUI;

    private Vector3 initialPosition;

    void Start () {
        // what is it needed for?
		editorUI = GameObject.Find ("Canvas").GetComponent<EditorUI> ();
        initialPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

	void Update () {

        if (!editorUI.GetGridGenerated()) return;
	 
		float move = Input.GetAxis ("Vertical") * speed;
		float turn = Input.GetAxis ("Horizontal") * speed;
		float zoom = Input.GetAxis ("Mouse ScrollWheel") * speed * 15f;
/*		if (zoom != 0f && move == 0f) {
			Debug.Log ("HERE");
			move = zoom;
		} */
		move *= Time.deltaTime;
		turn *= Time.deltaTime;
		zoom *= Time.deltaTime;

		transform.Translate (turn, -1f*zoom, move);
        //Debug.Log(GameObject.Find(surfName));
        if (transform.position.y < (GameObject.Find(editorUI.surfName).transform.position.y + yLowBound))
        {
			transform.position = new Vector3 (transform.position.x, 
				GameObject.Find (editorUI.surfName).transform.position.y + yLowBound,
				transform.position.z);
		}

        if (Input.GetKey(KeyCode.R))
        {
            transform.position = initialPosition;
        }

        // source tree test
        // source tree test again

		//Debug.Log (speed);
	}
}
