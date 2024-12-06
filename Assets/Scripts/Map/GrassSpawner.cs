using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
	
	public GameObject grassPrefab;
	public Transform grassParent;
	public Mesh mesh;
	public List<GameObject> grassObjects;
	private bool started;
	public float renderDistance;
	
	public void StartGrass() {
		started = true;
		mesh = GetComponent<MeshCollider>().sharedMesh;
		grassObjects = new List<GameObject>();
		GameObject newGrassParent = new GameObject();
		newGrassParent.transform.parent = transform;
		grassParent = newGrassParent.transform;
	}
	
	public void UpdateGrass() {
		
		Debug.Log("TEEEEEEEEst");
		
		foreach(Vector3 vPos in mesh.vertices) {
			
			bool exists = false;
			GameObject existingGrass = null;
			foreach(GameObject g in grassObjects) {
				if(g.transform.position == vPos) {
					exists = true;
					existingGrass = g;
				}
			}
			
			
			if(Vector3.Distance(vPos, Camera.main.transform.position) <= renderDistance) {
				if(!exists) {
					GameObject newGrass = Instantiate(grassPrefab);
					newGrass.transform.parent = grassParent;
					grassObjects.Add(newGrass);
				}
			}
			if(Vector3.Distance(vPos, Camera.main.transform.position) > renderDistance) {
				if(exists) {
					GameObject newGrass;
					grassObjects.Remove(existingGrass);
				}
			}
		}
		
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(started) {
			UpdateGrass();
		}
    }
}
