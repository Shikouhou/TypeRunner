using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreLoader : MonoBehaviour
{

    public Text highscore;

    private void Start()
    {
        int score;
        score = PlayerPrefs.GetInt("tr_highscore");

        highscore.text = score.ToString();

    }

}
