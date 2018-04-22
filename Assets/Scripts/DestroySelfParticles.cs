using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfParticles : MonoBehaviour {

    private ParticleSystem ps;
    private Vector3 location;

	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = Camera.main.ScreenToWorldPoint(location);
            }
        }
	}

    public void SetPosition(Vector3 position)
    {
        location = position;
    }
}
