using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

	private static Data instance;

	public static Data Instance
	{
		get
		{
			return instance;
		}
	}

	public bool useCamerapic;

	public int data;
	public Texture2D myPic;


	public int imageIndex;
	public Texture2D[] images;

	// Use this for initialization
	void Start () {
		
		instance = this;
		data = 3;
		DontDestroyOnLoad (this);
	}
}
