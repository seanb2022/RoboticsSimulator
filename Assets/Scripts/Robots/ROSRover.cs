using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
//using RosMessageTypes.UnityRoboticsDemo;

public class ROSRover : MonoBehaviour
{
	
	ROSConnection ros;
	
	private string topicName = "drrobot_motor_cmd";
	
	public float messageFreq = 0.5f;
	private float timeElapsed;
	private List<string> instructionList;
	private int frame;
	
    // Start is called before the first frame update
    void Start()
    {
		instructionList = new List<string>();
		instructionList.Add("MMW !MG");
		instructionList.Add("MMW !M 200 200");
		instructionList.Add("MMW !M 0 0");
        ros = ROSConnection.GetOrCreateInstance();
		ros.RegisterPublisher<RosMessageTypes.Robots.DrRobotMoveMsg>(topicName);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > messageFreq) {
			
			RosMessageTypes.Robots.DrRobotMoveMsg rMsg = new RosMessageTypes.Robots.DrRobotMoveMsg(
			instructionList[frame%instructionList.Count]
			);
			
			ros.Publish(topicName, rMsg);
			//ros.Publish(topicName, rMsg);
			timeElapsed = 0;
			frame++;
			
		}
    }
}
