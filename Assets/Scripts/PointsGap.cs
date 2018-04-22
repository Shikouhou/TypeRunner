using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsGap : MonoBehaviour {

    private bool hasBeenTriggered = false;

    public void SetTriggered()
    {
        hasBeenTriggered = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            other.GetComponent<PointsAdder>().PointsGap();
        }
    }

}
