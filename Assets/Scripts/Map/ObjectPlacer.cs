using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropEntry {
	
	public GameObject prefab;
	public float weight;
	
}

public class ObjectPlacer : MonoBehaviour
{
	
	public int propAmount;
	
	public int seed;
	
	public Vector2 mapSize;
	
	public List<PropEntry> props;
	
	private float waitTime = 2f;
	private float t;
	private bool loaded;
	
	public void SpawnProp(GameObject prop) {
		GameObject newProp = Instantiate(prop);
		newProp.transform.parent = transform;
		
		Vector3 rayPos = new Vector3(Random.Range(0,mapSize.x), 300, Random.Range(0,mapSize.y));
		
		RaycastHit hit;
		
		float alt = 0;
		
		if (Physics.Raycast(rayPos, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
		{
			alt = hit.point.y;
		}
		
		newProp.transform.position = new Vector3(rayPos.x, alt, rayPos.z);
		
	}
	
    // Start is called before the first frame update
    void Start()
    {
		
		
		
    }

    // Update is called once per frame
    void Update()
    {
        if(t >= waitTime && !loaded) {
			
			GetComponent<GrassSpawner>().StartGrass();
			
			loaded = true;
			
			Random.seed = seed;
			float maxVal = 0;
			
			for(int i = 0; i < props.Count; i++) {
				maxVal += props[i].weight;
			}
			
			
			
			for(int i = 0; i < propAmount; i++) {
				
				float rVal = Random.Range(0,maxVal);
				float rIt  = 0;
				
				for(int j = 0; j < props.Count; j++) {
					
					if(rVal >= rIt && rVal < rIt + props[j].weight) {
						SpawnProp(props[j].prefab);
						break;
					}
					rIt += props[j].weight;
					
				}
				
			}
		}
		else {
			t += Time.deltaTime;
		}
    }
}
