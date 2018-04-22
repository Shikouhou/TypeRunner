using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    [Range(0,10)]
    public float xOffset;
    public float damp;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
	
	void Update () {

		if (target != null)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;

            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(destination.x + xOffset, destination.y, destination.z), ref velocity, damp);
        }

	}
}
