using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {


	[SerializeField] private bool reachedTarget;

	public Vector3 targetPosition;
	public bool locked;

	public bool GetReachedTarget()
	{
		return reachedTarget;
	}

	public void SetReachedTarget(bool flag)
	{
		reachedTarget = flag;
	}
}
