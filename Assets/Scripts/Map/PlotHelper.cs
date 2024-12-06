using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotHelper : MonoBehaviour
{
	
	public GameObject[] stakes;
	public GameObject plantZone;
	public float weedDensity;
	
	public GameObject plant;
	public Vector2 size;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stakes[0].transform.position = transform.position + new Vector3(size.x,0,size.y);
		stakes[1].transform.position = transform.position + new Vector3(-size.x,0,size.y);
		stakes[2].transform.position = transform.position + new Vector3(-size.x,0,-size.y);
		stakes[3].transform.position = transform.position + new Vector3(size.x,0,-size.y);
    }
	
	public void PlacePlot() {
		
		GameObject newPlot = Instantiate(plantZone);
		newPlot.GetComponent<PlantZone>().plant = plant;
		if(plant.GetComponent<Plant>().moldVulnerable) {
			newPlot.GetComponent<PlantZone>().actionType = 0;
		}
		else {
			newPlot.GetComponent<PlantZone>().actionType = PathMaker.Instance.rover.robotType;
		}
		newPlot.GetComponent<PlantZone>().weedDensity = weedDensity;
		newPlot.transform.position = transform.position;
		newPlot.transform.localScale = new Vector3(size.x,20,size.y);
		PathMaker.Instance.roverControls.plantMenu.SetActive(false);
		PathMaker.Instance.rover.wantsToTeleport = true;
		//Destroy(gameObject);
	}
}
