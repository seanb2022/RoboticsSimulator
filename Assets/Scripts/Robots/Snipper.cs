using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipper : MonoBehaviour
{
	
	public float val;
	private bool snipped = false;
	private float cutoff = 0.9f;
	public List<GameObject> affectedObjects;
	
	public void Snip() {
		snipped = true;
		foreach(GameObject g in affectedObjects) {
			if(g.layer == 9) {
				FruitBody f = g.transform.parent.GetComponent<FruitBody>();
				if(f != null) {
					f.Detach();
					Destroy(g);
				}
			}
		}
	}
	
    // Start is called before the first frame update
    void Start()
    {
        affectedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!snipped) {
			if(val >= cutoff) {
				Snip();
			}
		}
		if(val < cutoff) {
			snipped = false;
		}
    }
	
	void OnTriggerEnter(Collider other) {
		affectedObjects.Add(other.gameObject);
	}
	
	void OnTriggerExit(Collider other) {
		affectedObjects.Remove(other.gameObject);
	}
	
}
