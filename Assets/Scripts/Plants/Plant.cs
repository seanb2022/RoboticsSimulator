using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

	public GameObject visibleMesh;
	private bool wayPointAdded;
	
	public GameObject healthyModel;
	public GameObject deadModel;
	
	public float health;
	public float mold;
	public float water;
	public bool healthy;
	private bool removedWaypoint;
	public bool killingMold;
	public bool moldVulnerable;
	public DebugRover rover;
	
	public GameObject[] growthStages;
	public int growthStage;
	
	public bool desired;

    // Start is called before the first frame update
    void Start()
    {
		if(growthStages.Length > 0) {
			growthStage = Random.Range(0,4);
			growthStages[growthStage].SetActive(true);
		}
		
		health = 40;
		water = Random.Range(0,100);
        //mold = Random.Range(0,100);
		mold = 20;
		if(!desired) {
			healthy = true;
			mold = 0;
		}
    }
	
    // Update is called once per frame
    void Update()
    {
		
		if(rover != null) {
			if(!rover.lightOn) {
				killingMold = false;
			}
		}
		
		if(killingMold) {
			float dist = Vector3.Distance(transform.position, rover.transform.position);
			float killAmount = (10f * Time.deltaTime)/(dist*dist);
			if(rover != null) {
				mold -= killAmount;
			}
			if(mold <= 0) {
				mold = 0;
				health -= killAmount;
				healthy = true;
			}
		}
		
		if(health <= 0) {
			
			if(healthy) {
				deadModel.SetActive(true);
				healthyModel.SetActive(false);
				healthy = false;
			}
			
		}
		
		if(healthy) {
			if(desired) {
				if(!removedWaypoint) {
					removedWaypoint = true;
					//rover.currentWaypoint = PathMaker.Instance.GetNextWaypoint(rover.transform.position);
				}
			}
		}
		else {
			if(!desired) {
				if(!removedWaypoint) {
					removedWaypoint = true;
					//rover.currentWaypoint = PathMaker.Instance.GetNextWaypoint(rover.transform.position);
				}
			}
		}
		
		/*
        if(Vector3.Distance(transform.position, Camera.main.transform.position) > 50f) {
        	visibleMesh.SetActive(false);
        }
        else {
        	visibleMesh.SetActive(true);
        }
		*/
		
		if(!wayPointAdded) {
			if(PathMaker.Instance != null) {
				//wayPointAdded = true;
				//PathMaker.Instance.waypoints.Add(new Vector2(transform.position.x, transform.position.z));
				//PathMaker.Instance.loaded = true;
			}
		}
    }
}
