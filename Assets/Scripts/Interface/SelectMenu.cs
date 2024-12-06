using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenu : MonoBehaviour
{

	public List<GameObject> robotPrefabs;
	public List<GameObject> robots;
	public float radius;
	
	
	
	public void HideSelections() {
		foreach(GameObject r in robots) {
			r.SetActive(false);
		}
	}
	
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < robotPrefabs.Count; i++) {
        	float m = Mathf.PI*2;
        	float theta = m/(robotPrefabs.Count);
        	theta *= i+1;
        	Vector3 newPos = new Vector3(Mathf.Sin(theta)*radius,0,Mathf.Cos(theta)*radius);
        	GameObject newRobot = Instantiate(robotPrefabs[i]);
        	newRobot.transform.position = newPos;
        	robots.Add(newRobot);
        }
		Debug.Log(robotPrefabs[0]);
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
