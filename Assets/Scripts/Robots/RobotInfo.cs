using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RobotInfo : MonoBehaviour
{

	public GameObject robot;
	public GameObject floatingText;
	public bool hovering;
	private Vector3 sizeGoal;
	public bool chaseCam;
	public bool terrain;

    // Start is called before the first frame update
    void Start()
    {
        sizeGoal = new Vector3(1f,1f,1f);
		floatingText.GetComponent<TMP_Text>().text = robot.GetComponent<DebugRover>().robotName;
    }

    // Update is called once per frame
    void Update()
    {
        if(hovering) {
        	sizeGoal = new Vector3(1.1f,1.1f,1.1f);
        }
        else {
        	sizeGoal = new Vector3(1f,1f,1f);
        }
		
		floatingText.transform.LookAt(-PathMaker.Instance.mainCam.transform.position);
        
        
        transform.localScale = Vector3.Lerp(transform.localScale,
        	sizeGoal, 3.5f * Time.deltaTime);
        	
		hovering = false;
    }
}
