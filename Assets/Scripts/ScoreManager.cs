using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text level;
    public Text jumps;
    public Text words;
    public Text score;

    public GameObject canvas;
    public GameObject pointsPopup;
    public GameObject player;

    public int LevelPoints { get; private set; }
    public int JumpsPoints { get; private set; }
    public int WordsPoints { get; private set; }
    public int ScorePoints { get; private set; }

    private void Awake()
    {
        if (level)
        {
            if (!Globals.initialLoad)
            {
                LevelPoints = PlayerPrefs.GetInt("tr_level");

                JumpsPoints = PlayerPrefs.GetInt("tr_jumps");

                WordsPoints = PlayerPrefs.GetInt("tr_words");

                ScorePoints = PlayerPrefs.GetInt("tr_points");

            }
            else
            {
                LevelPoints = 1;
            }

            level.text = LevelPoints.ToString();
            jumps.text = JumpsPoints.ToString();
            words.text = WordsPoints.ToString();
            score.text = ScorePoints.ToString();

        }

    }

    public void AddJump()
    {
        JumpsPoints++;
        jumps.text = JumpsPoints.ToString();
        AddPoints(10);
    }

    public void AddWord()
    {
        WordsPoints++;
        words.text = WordsPoints.ToString();
        AddPoints(15);
    }

    private void AddPoints(int points)
    {
        ScorePoints += points;
        score.text = ScorePoints.ToString();

        PointsPopup(points);
    }

    public void SaveScore()
    {
        int previousScore = PlayerPrefs.GetInt("tr_highscore");
        if (ScorePoints > previousScore)
        {
            PlayerPrefs.SetInt("tr_highscore", ScorePoints);
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("tr_jumps", JumpsPoints);
        PlayerPrefs.SetInt("tr_words", WordsPoints);
        PlayerPrefs.SetInt("tr_points", ScorePoints);
        PlayerPrefs.SetInt("tr_level", LevelPoints + 1);
    }

    private void PointsPopup(int points)
    {
        GameObject g = Instantiate(pointsPopup, canvas.transform);
        g.GetComponentInChildren<Text>().text = "+" + points.ToString();
        g.GetComponent<SetPopupPosition>().Set(player.transform.position);
        player.GetComponent<AudioSource>().PlayOneShot(player.GetComponent<CharacterController>().points);
    }
}
