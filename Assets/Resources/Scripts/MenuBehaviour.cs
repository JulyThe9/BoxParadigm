using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuBehaviour : MonoBehaviour {

	public void newGame(){
	}

	public void mapEditor(){
		SceneManager.LoadScene(1);
	}

	public void settings(){
	}

	public void quit(){
		Application.Quit();
	}

	public void mark(string objName){
		Text value = GameObject.Find(objName).GetComponent<Text>();
		value.alignment = TextAnchor.UpperCenter;
	}

	public void unmark(string objName){
		Text value = GameObject.Find(objName).GetComponent<Text>();
		value.alignment = TextAnchor.MiddleCenter;
	}
		

}
