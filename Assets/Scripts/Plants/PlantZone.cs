using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantZone : MonoBehaviour
{

	public GameObject plant;
	public GameObject weed;
	
	public int actionType = 0; //0 = Measure Moisture, 1 = Kill weeds, 2 = Snip fruit

	public float xDensity;
	public float yDensity;
	public float weedDensity;
	
	public bool makeWaypoints = true;
	
	public Transform prefabOutput;
	
	private Vector2 rOffset;
	
	private int iter;

    // Start is called before the first frame update
    void Start()
    {
		
		xDensity = PathMaker.Instance.xDensity;
		yDensity = PathMaker.Instance.yDensity;
		
		rOffset = new Vector2(0,(1f/yDensity)/2);
		
    	prefabOutput.position = Vector3.zero;
        float x = transform.localScale.x;
        float y = transform.localScale.y;
        float z = transform.localScale.z;
        
        float max_x = xDensity*x;
        float max_y = yDensity*z;
		
		int weedAmount = (int)(x*z*weedDensity);
		
		for(int w = 0; w < weedAmount; w++) {
			
			//newWeed.transform.position = 
			
			RaycastHit hit;
			Vector3 startPos = new Vector3(
			Random.Range(transform.position.x-x,transform.position.x+(x)),
			y/2 + transform.position.y,
			Random.Range(transform.position.z-z,transform.position.z+(z))
			);
			if (Physics.Raycast(startPos, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
			{
			    GameObject newWeed = Instantiate(weed);
				newWeed.transform.parent = prefabOutput;
			    newWeed.transform.position = hit.point;
			    
			}
			
		}
        
		
		
        for(int _y = (int)-max_y; _y < max_y; _y++) {
			
			PathMaker.Waypoint wp;
			
			PathMaker.Instance.loaded = true;
			
			if(plant.GetComponent<Plant>().moldVulnerable) {
				
        	}
			//else {
			if(actionType == 1) {
				
				for(int _x = (int)-max_x; _x < max_x; _x += 1) {
					RaycastHit hit;
					Vector3 startPos = new Vector3(_x/xDensity, y/2,(_y/yDensity));
					
					//Place Plant
					if (Physics.Raycast(startPos+transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
					{
						GameObject newPlant = Instantiate(plant);
						newPlant.transform.parent = prefabOutput;
						newPlant.transform.position = hit.point;
					}
				}
			
				Vector2 wayPointPos;
				Vector2 wayPointPos2;
				
				wayPointPos = new Vector2(-max_x/xDensity, _y/yDensity);
				wayPointPos += new Vector2(transform.position.x, transform.position.z);
				//PathMaker.Instance.waypoints.Add(wayPointPos);
				wayPointPos2 = new Vector2(max_x/xDensity, _y/yDensity);
				wayPointPos2 += new Vector2(transform.position.x, transform.position.z);
				//PathMaker.Instance.waypoints.Add(wayPointPos);
				
				
				wayPointPos += rOffset;
				wayPointPos2 += rOffset;
				
				Vector2 midPoint = wayPointPos + wayPointPos2;
				midPoint /= 2;
				
				if(iter % 2 == 0) {
					wp = new PathMaker.Waypoint();
					wp.pos = wayPointPos;
					wp.light = false;
					wp.checkWater = false;
					PathMaker.Instance.waypoints.Add(wp);
					wp = new PathMaker.Waypoint();
					wp.pos = midPoint;
					wp.light = true;
					wp.checkWater = false;
					PathMaker.Instance.waypoints.Add(wp);
					wp = new PathMaker.Waypoint();
					wp.pos = wayPointPos2;
					wp.light = true;
					wp.checkWater = false;
					PathMaker.Instance.waypoints.Add(wp);
					wp = new PathMaker.Waypoint();
					wp.pos = wayPointPos2 + new Vector2((3f/xDensity),(1f/yDensity)*0.8f);
					wp.light = true;
					wp.checkWater = false;
					PathMaker.Instance.waypoints.Add(wp);
					wp = new PathMaker.Waypoint();
					wp.pos = wayPointPos2 + new Vector2((2.6f/xDensity),(1f/yDensity)*1f);
					wp.light = false;
					wp.checkWater = false;
				}
				else {
					wp = new PathMaker.Waypoint();
					wp.pos = wayPointPos2;
					wp.light = false;
					wp.checkWater = false;
					PathMaker.Instance.waypoints.Add(wp);
					wp = new PathMaker.Waypoint();
					wp.pos = midPoint;
					wp.light = true;
					wp.checkWater = false;
					PathMaker.Instance.waypoints.Add(wp);
					wp = new PathMaker.Waypoint();
					wp.pos = wayPointPos;
					wp.light = true;
					wp.checkWater = false;
					PathMaker.Instance.waypoints.Add(wp);
					wp = new PathMaker.Waypoint();
					wp.pos = wayPointPos + new Vector2(-(3f/xDensity),(1f/yDensity)*0.8f);
					wp.light = false;
					wp.checkWater = false;
					PathMaker.Instance.waypoints.Add(wp);
					wp = new PathMaker.Waypoint();
					wp.pos = wayPointPos + new Vector2(-(2.6f/xDensity),(1f/yDensity)*1f);
					wp.light = false;
					wp.checkWater = false;
					PathMaker.Instance.waypoints.Add(wp);
				}
			}
			
			
			int startX = (int)-max_x;
			int endX = (int)max_x;
			int _inc = 1;
			
			if(iter % 2 == 0) {
				//startX *= -1;
				//endX *= -1;
				_inc = -1;
			}
			
			//Moisture Robot Path
			
			//if(plant.GetComponent<Plant>().moldVulnerable) {
			
			if(actionType == 0) {
			
				for(int _x = startX; _x < endX; _x += 1) {
					RaycastHit hit;
					Vector3 startPos = new Vector3(_inc*_x/xDensity, y/2,(_y/yDensity));
					
					//MoveWaypoint
					if (Physics.Raycast(startPos+transform.position + new Vector3(0,0,1), transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
					{
						Vector2 plantPoint = new Vector2(hit.point.x, hit.point.z);
						wp = new PathMaker.Waypoint();
						wp.pos = plantPoint;
						wp.light = false;
						wp.checkWater = false;
						PathMaker.Instance.waypoints.Add(wp);
					}
					
					//AimWaypoint
					if (Physics.Raycast(startPos+transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
					{
						GameObject newPlant = Instantiate(plant);
						newPlant.transform.parent = prefabOutput;
						newPlant.transform.position = hit.point;
						Vector2 plantPoint = new Vector2(hit.point.x, hit.point.z);
						wp = new PathMaker.Waypoint();
						wp.pos = plantPoint;
						wp.light = false;
						wp.checkWater = true;
						wp.aimOnly = true;
						PathMaker.Instance.waypoints.Add(wp);
					}
				}
			}
			
			//Pick fruit
			if(actionType == 2) {
				
				for(int _x = startX; _x < endX; _x += 1) {
					RaycastHit hit;
					Vector3 startPos = new Vector3(_inc*_x/xDensity, y/2,(_y/yDensity));
					
					//MoveWaypoint
					if (Physics.Raycast(startPos+transform.position + new Vector3(0,0,1), transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
					{
						Vector2 plantPoint = new Vector2(hit.point.x, hit.point.z);
						wp = new PathMaker.Waypoint();
						wp.pos = plantPoint;
						wp.light = false;
						wp.checkWater = false;
						PathMaker.Instance.waypoints.Add(wp);
					}
					
					//AimWaypoint
					if (Physics.Raycast(startPos+transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
					{
						GameObject newPlant = Instantiate(plant);
						newPlant.transform.parent = prefabOutput;
						newPlant.transform.position = hit.point;
						Vector2 plantPoint = new Vector2(hit.point.x, hit.point.z);
						wp = new PathMaker.Waypoint();
						wp.pos = plantPoint;
						wp.light = false;
						wp.checkWater = false;
						wp.pickFruit = true;
						wp.plant = newPlant;
						PathMaker.Instance.waypoints.Add(wp);
					}
				}
				
			}
			
			iter++;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
