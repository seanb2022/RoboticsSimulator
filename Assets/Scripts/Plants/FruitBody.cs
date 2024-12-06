using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBody : MonoBehaviour
{
	
	public float weight = 0.1f; //The weight of the fruit/vegetable in kilograms
	public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void Detach() {
		rb.isKinematic = false;
	}
	
	void OnCollisionEnter(Collision collision)
    {
        Detach();
    }
	
	void OnTriggerEnter(Collider collider)
    {
		if(collider.gameObject.tag == "basketZone") {
			collider.gameObject.transform.parent.gameObject.GetComponent<Basket>().AddFruit(this);
			Destroy(this);
		}
    }
}
