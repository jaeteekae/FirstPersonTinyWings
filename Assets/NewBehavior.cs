using UnityEngine;
using System.Collections;

public class NewBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        /*var exp = GetComponent<ParticleSystem>();
        exp.Play();
        Destroy(gameObject, exp.duration);*/

    }
    public GameObject explosionPrefab;
    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(transform.position), out hit)) //
        {
			var exp = GetComponent<ParticleSystem>();
			exp.Play();
			Destroy(gameObject, exp.duration);
            Instantiate(explosionPrefab, hit.point, Quaternion.identity);
        }
    }
    
   
}
