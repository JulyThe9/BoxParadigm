using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirCamCtrl : MonoBehaviour {

	// Use this for initialization
	private GameObject mirror;
	private RenderTexture rendTex;
	void Start () {
		
	}

	
	// Update is called once per frame
	void Update () {
		mirror = GameObject.Find (transform.parent.name + "/Mirror");
		rendTex = new RenderTexture (256, 256, 16, RenderTextureFormat.ARGB32);
		gameObject.GetComponent<Camera> ().targetTexture = rendTex;
		Material mat = new Material(Shader.Find("Standard"));		
		mat.SetTexture ("_MainTex",rendTex);
		mirror.GetComponent<Renderer> ().material = mat;
	}
}
