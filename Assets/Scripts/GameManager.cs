using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	[SerializeField] private GameObject piecesHolder, piecePositionHolder, colliderHolder;

	[SerializeField] private List<Transform> pieces, colliders, positions;
	private Transform[] pc, piecePositions, colliderPositions;

	private Text moveText, scoreText, pieceText, gameCompleteMoveText, gameCompleteScoreText;
	private int move, score, remainingPiece, totalPiece;

	private Image gameCompletefullImage, hintfullImage;

	[SerializeField] public Sprite fullSprite;

	public Transform selectedPiece;
	private Vector3 pieceLastPosition;
	public bool startDrag;

	public GameObject gameCompletePanel;

	[SerializeField] private GameObject glowEffect;
	[SerializeField] private int solvedPieces;

	private static GameManager instance;

	public static GameManager Instance
	{
		get{
			return instance;
		}
	}

	// Use this for initialization
	void Start () 
	{
		instance = this;
		moveText = GameObject.Find ("MoveText").GetComponent<Text> ();
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();
		pieceText = GameObject.Find ("PieceText").GetComponent<Text> ();
		gameCompleteMoveText = GameObject.Find ("GameCompleteMoveText").GetComponent<Text> ();
		gameCompleteScoreText = GameObject.Find ("GameCompleteScoreText").GetComponent<Text> ();
		gameCompletefullImage = GameObject.Find ("GameCompleteFullImage").GetComponent<Image> ();
		gameCompletePanel = GameObject.Find ("GameCompletePanel");
		gameCompletePanel.SetActive (false);
		CreateBoard ();

		remainingPiece = pieces.Count;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		MovePiece ();
		HandleUI ();
		HandleWin ();
	}

	private void CreateBoard()
	{

		if ( Data.Instance )
		{
			solvedPieces = Data.Instance.data;
		}
		else
		{
			solvedPieces = 4;
		}

		pc = piecesHolder.GetComponentsInChildren<Transform> ();
		for (int i=1 ; i<pc.Length ; i++ ) 
		{
			pieces.Add (pc[i]);
		}
		piecePositions = piecePositionHolder.GetComponentsInChildren<Transform> ();
		colliderPositions = colliderHolder.GetComponentsInChildren<Transform> ();

		for (int i=1 ; i<piecePositions.Length ; i++ ) 
		{
			positions.Add (piecePositions[i]);
		}

		for (int i=1 ; i<colliderPositions.Length ; i++ ) 
		{
			colliders.Add (colliderPositions[i]);
		}

		for (int i = 0; i < solvedPieces ; i++)
		{
			int indx =	Random.Range (0, pieces.Count);
			Piece p = pieces [indx].GetComponent<Piece> ();
			while (p.locked)
			{
				indx =	Random.Range (0, pieces.Count);
				p = pieces [indx].GetComponent<Piece> ();
			}
			for (int j = 0; j < colliders.Count; j++)
			{
				if ( p.name == colliders [j].name )
				{
					p.transform.position = colliders [j].transform.position;
					p.locked = true;
					pieces.RemoveAt (indx);
				}
			}

//

		}


		for (int j = 0; j < pieces.Count; j++)
		{
			pieces [j].transform.position = positions [j].transform.position;
		}


	}

	Ray ray;
	float xPos, yPos;
	public int selectedPieceIndex;
	public float offset;

	private void MovePiece()
	{

//		if (Input.GetMouseButton (0)) {
//			Vector2 mousePosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
//			Vector2 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
//
//
//
//		}

		RaycastHit hit;

		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.GetMouseButtonDown(0)) {
			if (Physics.Raycast (ray, out hit) && hit.transform.tag == "piece"){
				if( hit.transform.GetComponent<Piece>().locked == false)
				{
					startDrag = true;
					selectedPiece = hit.transform;
					pieceLastPosition = hit.transform.position;
					move++;
					selectedPiece.GetComponent<SpriteRenderer> ().sortingOrder++;
					for (int i = 0; i < pieces.Count; i++)
					{
						if ( pieces [i].name == selectedPiece.name )
						{
							selectedPieceIndex = i;

						}

					}
				}
			}
		}

		if (Input.GetMouseButtonUp (0) && startDrag) {
		//	sele Vector3.Lerp (selectedPiece.transform.position, pieceLastPosition, 10f);

			Piece piece = selectedPiece.GetComponent<Piece> ();

			if (piece.GetReachedTarget ()) 
			{
				selectedPiece.transform.position = piece.targetPosition;
				glowEffect.SetActive (true);
				glowEffect.transform.position = new Vector3( piece.targetPosition.x,piece.targetPosition.y,piece.targetPosition.z+1) ;
				StartCoroutine (GlowOff ());
				selectedPiece.GetComponent<SpriteRenderer> ().sortingOrder--;
				selectedPiece = null;
				startDrag = false;
				piece.locked = true;
				pieces.RemoveAt (selectedPieceIndex);
				score += 100;
				remainingPiece--;

				for (int j = 0; j < pieces.Count; j++)
				{
					pieces [j].transform.position = positions [j].transform.position;
				}

//				for (int i = selectedPieceIndex + 1; i < pieces.Count; i++) 
//				{
//					if (pieces [i].gameObject.GetComponent<Piece> ().locked == false) 
//					{
//						pieces [i].transform.position = new Vector3 (pieces [i].transform.position.x, pieces [i].transform.position.y + offset, pieces [i].transform.position.z);
//					}
//				}
			}
			else 
			{
				selectedPiece.transform.position = pieceLastPosition;
				selectedPiece.GetComponent<SpriteRenderer> ().sortingOrder--;
				selectedPiece = null;
				startDrag = false;
				score -= 50;
								
			}


		}

		if (startDrag) {
			Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			xPos = pos.x;
			yPos = pos.y;
			selectedPiece.position = new Vector3 (xPos, yPos, selectedPiece.position.z);
		}


	}

	IEnumerator GlowOff()
	{
		yield return new WaitForSeconds (1);
		glowEffect.SetActive (false);
	}

	private void HandleUI()
	{
		moveText.text = "Move: " + move;
		scoreText.text = "Score: " + score;
		pieceText.text = "Piece remaining: " + remainingPiece;
	}

	private void HandleWin()
	{
		if ( remainingPiece <= 0 )
		{
			gameCompletePanel.SetActive (true);
			gameCompleteMoveText.text = "Move: " + move;
			gameCompleteScoreText.text = "Score: " + score;
			gameCompletefullImage.sprite = fullSprite;
		}
	}

	public void MainMenuPressed()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	public void RestartPressed ()
	{
		SceneManager.LoadScene ("Level1");
	}
}
