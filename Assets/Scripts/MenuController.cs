using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	private int[] cellSolved = { 0, 3, 5, 7 };

	RawImage CameraImage;

	[SerializeField] private bool pictureTaken;
	private Text cameraButtonText;

	void Start()
	{
		CameraImage = GameObject.Find ("CameraImage").GetComponent<RawImage> ();
		cameraButtonText = GameObject.Find ("CameraButtonText").GetComponent<Text> ();
		CameraImage.gameObject.SetActive (false);
	}

	public void PlayPressed()
	{
		Data.Instance.useCamerapic = false;
		SceneManager.LoadScene ("Level1");
	}
	public void ExitPressed()
	{
		Application.Quit ();
	}

	public void DropdownValueChanged(int indx)
	{
		Data.Instance.data = cellSolved[indx];
	}

	public void CameraPicturePressed()
	{
		CameraImage.gameObject.SetActive (true);

		pictureTaken = false;
		cameraButtonText.text = "Capture";
		Data.Instance.useCamerapic = true;

		WebCamPhotoCamera.Instance.OnCamera ();
	}

	public void TakeCamera()
	{
		if ( !pictureTaken )
		{
			
			cameraButtonText.text = "Next";
			pictureTaken = true;

			StartCoroutine (WebCamPhotoCamera.Instance.TakePhoto ());
		}
		else
		{
			WebCamPhotoCamera.Instance.OffCamera ();
			SceneManager.LoadScene ("level1");
		}

	}
}
