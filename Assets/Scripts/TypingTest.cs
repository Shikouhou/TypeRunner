using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingTest : MonoBehaviour {

    public Text input;

    private string message;
    private char currentChar;

	// Use this for initialization
	void Start () {
        currentChar = '$';
        message = "";
	}
	
	// Update is called once per frame
	void Update () {

        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                message = message.Substring(0, message.Length - 1);
            }
            else if (c == '\n' || c == '\r')
            {
                message = "";
                StartCoroutine(GetComponent<CameraShake>().Shake(.025f, .1f));
            }
            else
            {
                message += char.ToUpperInvariant(c);
                StartCoroutine(GetComponent<CameraShake>().Shake(.01f, .025f));                
            }
            currentChar = c;


        }

        input.text = message + "_";
    }
}
