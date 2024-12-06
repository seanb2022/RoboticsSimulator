using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoverControls : MonoBehaviour
{
	
	public Toggle selfDriving;
	public Toggle scienceQA;
	public Button plantButton;
	public Button recordButton;
	public Button toggleViewButton;
	public Button toggleViewButton2;
	public GameObject plantMenu;
	public GameObject recordingNotification;
	
	private float timeSinceRecordingStarted = 0f;
	
    // Start is called before the first frame update
    void Start()
    {
        plantButton.onClick.AddListener(TogglePlanting);
		recordButton.onClick.AddListener(ToggleRecording);
		toggleViewButton.onClick.AddListener(ToggleView);
		toggleViewButton2.onClick.AddListener(ToggleView);
    }

    // Update is called once per frame
    void Update()
    {
        if(PathMaker.Instance.rover != null) {
			PathMaker.Instance.rover.selfDriving = selfDriving.isOn;
			PathMaker.Instance.rover.scienceQA = scienceQA.isOn;
		}
		
		if(timeSinceRecordingStarted >= 5) {
			recordingNotification.SetActive(false);
		}
		//timeSinceRecordingStarted += Time.deltaTime;
		
    }
	
	private void TogglePlanting() {
		if(plantMenu.activeSelf) {
			plantMenu.SetActive(false);
			PathMaker.Instance.manip.placingPlot = true;
		}
		else {
			plantMenu.SetActive(true);
			PathMaker.Instance.manip.placingPlot = true;
			PathMaker.Instance.mainCam.mode = 2;
			PathMaker.Instance.placeCamOrg.transform.position = PathMaker.Instance.mainCam.focusedRobot.transform.position + new Vector3(0,15,0);
		}
	}
	
	private void ToggleRecording() {
		if(PathMaker.Instance.rover.recording) {
			PathMaker.Instance.rover.StopRecording();
			recordButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Start Recording";
			recordingNotification.SetActive(false);
		}
		else {
			
			
			PathMaker.Instance.rover.StartRecording();
			timeSinceRecordingStarted = 0f;
			recordingNotification.SetActive(true);
			string s = "Recording to: " + Application.dataPath + "/Recordings/" + PathMaker.Instance.rover.curRecordDir;
			Debug.Log(s);
			recordingNotification.GetComponent<TMP_Text>().text = s;
			recordButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Stop Recording";
		}
	}
	
	private void ToggleView() {
		PathMaker.Instance.manip.ToggleView();
	}
	
}
