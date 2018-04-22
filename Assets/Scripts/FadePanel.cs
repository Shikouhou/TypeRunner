using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour {


    [Range(0, 5)]
    public float speed;
    public bool visible;

    private CanvasGroup group;
    private float alpha = 0;
    private float fadeDir;

    // Use this for initialization
    void Start () {
        group = GetComponent<CanvasGroup>(); ;
        alpha = group.alpha;
	}

    void OnGUI()
    {
        alpha += fadeDir * speed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        
        group.alpha = alpha;
    }

    // Update is called once per frame
    void Update () {
        if (visible)
        {
            fadeDir = 1;
        }
        else
        {
            fadeDir = -1;
        }
    }
}
