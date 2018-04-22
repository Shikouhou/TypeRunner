using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickfireStop : MonoBehaviour {

    private bool stopHasBeenTriggered;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && !stopHasBeenTriggered)
        {
            if (other.GetComponent<CharacterAnimation>().Anim.GetBool("walk") == false)
            {
                stopHasBeenTriggered = true;
                Camera.main.gameObject.GetComponent<TypeManager>().QuickfireSingle();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && !stopHasBeenTriggered)
        {
            Debug.Log("lose a life");
            other.GetComponent<AudioSource>().PlayOneShot(other.GetComponent<CharacterController>().wrong);
            other.GetComponent<PlayerLives>().RemoveLife();
        }
        else if (other.tag == "Player" && stopHasBeenTriggered)
        {
            stopHasBeenTriggered = false;
        }
    }
}
