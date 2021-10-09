using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class WebCamPhotoCamera : MonoBehaviour {
    
	WebCamTexture webCamTexture;

    public RawImage renderer;

	WebCamDevice[] devices;

    string your_path = "ScreenShot/";

    private static WebCamPhotoCamera instance;

    public static WebCamPhotoCamera Instance {
        get {
            return instance;
        }
    }

    void Start () {
        instance = this;

    }

    public void OnCamera () {
     //   webCamTexture = new WebCamTexture ();
      

		devices = WebCamTexture.devices;

		foreach (WebCamDevice cam in devices)
		{
			if ( cam.isFrontFacing )
			{
				webCamTexture = new WebCamTexture (cam.name);
			}
		}

		

		renderer.material.mainTexture = webCamTexture;

        webCamTexture.Play ();
    }

    public void OffCamera () {
        webCamTexture.Stop ();
    }

    void Update () {
        if (Input.GetKeyDown (KeyCode.P)) {
            StartCoroutine (TakePhoto ());
        }
    }

    public static string ScreenShotName (int width, int height) {
        return string.Format ("{0}/ScreenShot/MyPic.png",
            Application.dataPath,
            width, height,
            System.DateTime.Now.ToString ("yyyy-MM-dd_HH-mm-ss"));
    }

    public IEnumerator TakePhoto () {

        yield return new WaitForEndOfFrame ();

        Texture2D photo = new Texture2D (webCamTexture.width, webCamTexture.height);
        photo.SetPixels (webCamTexture.GetPixels ());
        photo.Apply ();
        Data.Instance.myPic = photo;

        Data.Instance.myPic = Resize (Data.Instance.myPic, 960, 640);

        //	//Encode to a PNG
        byte[] bytes = photo.EncodeToPNG ();
        string filename = ScreenShotName (photo.width, photo.height);
        System.IO.File.WriteAllBytes (filename, bytes);

    }
    public static Texture2D Resize (Texture2D source, int newWidth, int newHeight) {
        source.filterMode = FilterMode.Point;
        RenderTexture rt = RenderTexture.GetTemporary (newWidth, newHeight);
        rt.filterMode = FilterMode.Point;
        RenderTexture.active = rt;
        Graphics.Blit (source, rt);
        Texture2D nTex = new Texture2D (newWidth, newHeight);
        nTex.ReadPixels (new Rect (0, 0, newWidth, newWidth), 0, 0);
        nTex.Apply ();
        RenderTexture.active = null;
        return nTex;

    }
}