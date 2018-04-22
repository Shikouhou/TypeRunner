using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHit : MonoBehaviour {

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            GetComponentInParent<PointsGap>().SetTriggered();
            other.GetComponent<CharacterController>().Jump();
            other.GetComponent<AudioSource>().PlayOneShot(other.GetComponent<CharacterController>().wrong);
            other.GetComponent<PlayerLives>().RemoveLife();
        }
    }

    private IEnumerator FixForRespawn()
    {
        yield return new WaitForSecondsRealtime(2);
        hasBeenTriggered = false;
    }

}
