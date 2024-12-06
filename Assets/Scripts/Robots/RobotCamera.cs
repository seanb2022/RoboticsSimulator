using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RobotCamera : MonoBehaviour
{

	Camera cam;	
	public int frame;
	
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CaptureFrame(string folderName) {
    	
    	RenderTexture curRT = RenderTexture.active;
    	RenderTexture.active = cam.targetTexture;
    	
    	cam.Render();
    	
    	Texture2D img = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
    	img.ReadPixels(new Rect(0,0,cam.targetTexture.width, cam.targetTexture.height), 0,0);
    	img.Apply();
    	RenderTexture.active = curRT;
    	
    	var bytes = img.EncodeToPNG();
    	Destroy(img);
    	
    	File.WriteAllBytes(Application.dataPath + "/Recordings/" + folderName + "/" + frame + ".png", 		bytes);
    	frame++;
    }
    
}
