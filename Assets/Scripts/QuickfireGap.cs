using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickfireGap : MonoBehaviour {

    [Range(0,0.1f)]
    public float dampTime;

    private float velocity = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(SlowDownBrother());
        }
    }

    private IEnumerator SlowDownBrother()
    {
        while (Time.timeScale != 0)
        {
            if (Time.timeScale - dampTime < 0)
            {
                Time.timeScale = 0;
                break;
            }

            Time.timeScale -= dampTime;
            yield return null;
        }

        Camera.main.gameObject.GetComponent<TypeManager>().QuickfireSingle();
    }

}
