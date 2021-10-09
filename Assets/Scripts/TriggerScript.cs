using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		if (col.name == transform.name) {
			col.GetComponent<Piece> ().SetReachedTarget (true);
			col.GetComponent<Piece> ().targetPosition = transform.position;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.name == transform.name) 
		{
			col.GetComponent<Piece> ().SetReachedTarget (false);
		}
	}
}
