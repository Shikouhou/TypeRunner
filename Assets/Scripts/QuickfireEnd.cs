using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickfireEnd : MonoBehaviour {

    private bool stopHasBeenTriggered;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && !stopHasBeenTriggered)
        {
            if (other.GetComponent<CharacterAnimation>().Anim.GetBool("walk") == false)
            {
                stopHasBeenTriggered = true;
                Camera.main.gameObject.GetComponent<TypeManager>().QuickfireEnd();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && !stopHasBeenTriggered)
        {
            other.GetComponent<AudioSource>().PlayOneShot(other.GetComponent<CharacterController>().wrong);
            other.GetComponent<PlayerLives>().RemoveLife();
        }
    }
}
