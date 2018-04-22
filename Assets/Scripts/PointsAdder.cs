using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsAdder : MonoBehaviour {

    public ScoreManager scoreManager;

    public void PointsGap()
    {
        scoreManager.AddJump();
    }

    public void PointsWord()
    {
        scoreManager.AddWord();
    }

    public void SaveScore()
    {
        scoreManager.SaveScore();
    }

    public void SaveGame()
    {
        scoreManager.SaveGame();
    }
}
