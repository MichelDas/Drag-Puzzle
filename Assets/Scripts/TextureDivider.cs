using UnityEngine;
using System.Collections;

public class TextureDivider: MonoBehaviour {

	public Texture2D source;
	private char[] Namepreview = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'}	;

	public int maxWidth, maxHeight, NumberOfDivision;
	// Use this for initialization
	void Awake () {

		GameObject spritesRoot = GameObject.Find("pieces");

		if ( !Data.Instance.useCamerapic )
			source = Data.Instance.images [Data.Instance.imageIndex];
		else
		{
			source = Data.Instance.myPic;
		}

	//	GameManager.Instance.fullSprite =  Sprite.Create(source, new Rect(0.0f, 0.0f, 960, 640), new Vector2(0.5f, 0.5f));
		maxWidth = source.width;
		maxHeight = source.height;

		int newWidth = maxWidth / NumberOfDivision;
		int newHeight = maxHeight / NumberOfDivision;

		for(int i = 0; i < NumberOfDivision; i++)
		{
			for(int j = 0; j < NumberOfDivision; j++)
			{
				Sprite newSprite = Sprite.Create(source, new Rect(i*newWidth, j*newHeight, newWidth, newHeight), new Vector2(0.5f, 0.5f));
				GameObject n = new GameObject();
				SpriteRenderer sr = n.AddComponent<SpriteRenderer>();
				sr.sprite = newSprite;
				sr.gameObject.name = Namepreview [j] + i.ToString ();
			//	n.transform.position = new Vector3(i*2, j*2 , 0);
				n.gameObject.AddComponent<BoxCollider>();
				n.gameObject.AddComponent<Piece> ();
				n.gameObject.AddComponent<Rigidbody> ();
				n.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
				n.gameObject.tag = "piece";
				n.transform.parent = spritesRoot.transform;
			}
		}
	}
}

