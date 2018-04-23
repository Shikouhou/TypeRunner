using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypeManager : MonoBehaviour {

    public Text input;
    public TypeMode mode;
    public GameObject character;
    public Text countdown;
    public Text wordToTypeComponent;
    public Text wordList;
    public FadePanel winText;

    [Header("Word Lists")]
    public TextAsset oneSyllable;
    public TextAsset twoSyllable;
    public TextAsset threeSyllable;
    public TextAsset fourSyllable;
    public TextAsset fiveSyllable;

    private string command;
    private char currentChar;

    private string currentWord;
    private float countdownTimer;

    private List<string> wordsToType = new List<string>();
    private List<string> wordsRevealed;
    private int wordIterator;

    private List<string> lines;

    void Start()
    {

        int level = character.GetComponent<PointsAdder>().scoreManager.LevelPoints;
        currentChar = '$';
        command = "";
        wordIterator = 0;

        if (level == 1)
        {
            lines = oneSyllable.text.Split('\n').ToList();
        }
        else if (level > 1 && level <= 3)
        {
            List<string> one = oneSyllable.text.Split('\n').ToList();
            List<string> two = twoSyllable.text.Split('\n').ToList();
            one.AddRange(two);
            lines = one;
        }
        else if (level > 3 && level <= 6)
        {
            List<string> two = twoSyllable.text.Split('\n').ToList();
            List<string> three = threeSyllable.text.Split('\n').ToList();
            two.AddRange(three);
            lines = two;
        }
        else if (level > 6 || level <= 8)
        {
            List<string> three = threeSyllable.text.Split('\n').ToList();
            List<string> four = fourSyllable.text.Split('\n').ToList();
            three.AddRange(four);
            lines = three;
        }
        else if (level > 8)
        {
            List<string> two = twoSyllable.text.Split('\n').ToList();
            List<string> three = threeSyllable.text.Split('\n').ToList();
            List<string> four = fourSyllable.text.Split('\n').ToList();
            List<string> five = fiveSyllable.text.Split('\n').ToList();
            three.AddRange(four);
            three.AddRange(five);
            three.AddRange(two);
            lines = three;
        }
    }

    void Update()
    {

        if (character.GetComponent<PlayerLives>().IsDead())
        {
            StopAllCoroutines();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ProcessCommand(command);
            command = "";
        }

        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                command = command.Substring(0, command.Length - 1);
            }
            else if (c == '\r' || c == '\n')
            {
                continue;
            }
            else
            {
                command += char.ToUpperInvariant(c);
                character.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(-0.1f, 0.1f);
                character.GetComponent<AudioSource>().PlayOneShot(character.GetComponent<CharacterController>().keyboard);
                character.GetComponent<AudioSource>().pitch = 1;
            }

            currentChar = c;


        }

        input.text = ">" + command + "_";
    }

    public void QuickfireSingle()
    {
        mode = TypeMode.Quickfire;

        currentWord = lines[UnityEngine.Random.Range(0, lines.Count - 1)];
        currentWord = currentWord.ToUpper();
        currentWord = currentWord.Substring(0, currentWord.Length - 1);

        countdown.gameObject.SetActive(true);
        wordToTypeComponent.gameObject.SetActive(true);
        wordToTypeComponent.text = "Type:" + '\n' + currentWord;

        StartCoroutine(InitiateCountdown());
    }

    public void QuickfireEnd()
    {
        mode = TypeMode.QuickfireEnd;

        wordsToType = new List<string>();
        wordsRevealed = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            wordsToType.Add(lines[UnityEngine.Random.Range(0, lines.Count - 1)]);
            wordsRevealed.Add("?");
        }


        GetCurrentWordEndSequence();

        wordIterator++;

        wordList.gameObject.SetActive(true);
        countdown.gameObject.SetActive(true);
        wordToTypeComponent.gameObject.SetActive(true);
        

        StartCoroutine(EndingSequence());
    }

    private void GetCurrentWordEndSequence()
    {
        wordList.text = "";
        currentWord = wordsToType[wordIterator];
        currentWord = currentWord.ToUpper();
        currentWord = currentWord.Substring(0, currentWord.Length - 1);

        wordsRevealed[wordIterator] = currentWord;

        SetWordList();

        wordToTypeComponent.text = "Type:" + '\n' + currentWord;
    }

    private void SetWordList()
    {
        foreach (string word in wordsRevealed)
        {
            wordList.text += word + '\n';
        }
    }

    private void ProcessCommand(string command)
    {
        if (mode == TypeMode.Run)
        {
            switch (command)
            {
                case "WALK":
                    character.GetComponent<CharacterAnimation>().SetWalk(true);
                    break;
                case "STOP":
                    character.GetComponent<CharacterAnimation>().SetWalk(false);
                    break;
                case "JUMP":
                    character.GetComponent<CharacterAnimation>().SetJump();
                    break;
                case "QUIT":
                    StartCoroutine(Quit());
                    break;
                default:
                    if (Time.timeScale > 0)
                        StartCoroutine(GetComponent<CameraShake>().Shake(.1f, .1f));
                    break;
            }
        }
        else if (mode == TypeMode.Quickfire)
        {

            if (command == currentWord)
            {
                character.GetComponent<PointsAdder>().PointsWord();
            }
            else
            {
                character.GetComponent<AudioSource>().PlayOneShot(character.GetComponent<CharacterController>().wrong);
                character.GetComponent<PlayerLives>().RemoveLife();
            }

            StopAllCoroutines();
            ResetCharacter();
        }
        else if (mode == TypeMode.QuickfireEnd)
        {
            if (command == currentWord)
            {
                character.GetComponent<PointsAdder>().PointsWord();
            }
            else
            {
                character.GetComponent<AudioSource>().PlayOneShot(character.GetComponent<CharacterController>().wrong);
                character.GetComponent<PlayerLives>().RemoveLife();
            }

            if (wordIterator == 10)
            {
                Debug.Log("Done");
                StopAllCoroutines();
                StartCoroutine(WinLevel());
            }

            GetCurrentWordEndSequence();
            wordIterator++;
        }
        else if(mode == TypeMode.GameOver)
        {
            switch (command)
            {
                case "RETRY":
                    Globals.initialLoad = true;
                    StartCoroutine(Retry());
                    break;
                case "QUIT":
                    StartCoroutine(Quit());
                    break;
                default:
                    if (Time.timeScale > 0)
                        StartCoroutine(GetComponent<CameraShake>().Shake(.1f, .1f));
                    break;
            }
        }
        else if (mode == TypeMode.TitleScreen)
        {
            switch (command)
            {
                case "START":
                    Globals.initialLoad = true;
                    StartCoroutine(Retry());
                    break;
                case "QUIT":
                    StartCoroutine(Quit());
                    break;
                default:
                    if (Time.timeScale > 0)
                        StartCoroutine(GetComponent<CameraShake>().Shake(.1f, .1f));
                    break;
            }
        }
    }

    private IEnumerator WinLevel()
    {
        Globals.initialLoad = false;
        character.GetComponent<CharacterController>().darkness.visible = true;
        yield return new WaitForSecondsRealtime(1);
        winText.visible = true;
        yield return new WaitForSecondsRealtime(1.5f);
        winText.visible = false;
        yield return new WaitForSecondsRealtime(1f);

        character.GetComponent<PointsAdder>().SaveGame();

        SceneManager.LoadScene("Level1");
    }

    private IEnumerator Retry()
    {
        character.GetComponent<CharacterController>().darkness.visible = true;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level1");
    }

    private IEnumerator Quit()
    {
        character.GetComponent<CharacterController>().darkness.visible = true;
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }

    private IEnumerator EndingSequence()
    {
        countdownTimer = Globals.quickfireEndTime;
        while (countdownTimer > 0)
        {
            var countdownDisplay = (float)Math.Ceiling((decimal)countdownTimer);
            countdown.text = countdownDisplay.ToString();
            countdownTimer -= Time.unscaledDeltaTime;

            yield return null;
        }

        mode = TypeMode.Done;

        character.GetComponent<AudioSource>().PlayOneShot(character.GetComponent<CharacterController>().wrong);
        StartCoroutine(character.GetComponent<PlayerLives>().InitiateGameOver());
    }

    private IEnumerator InitiateCountdown()
    {
        countdownTimer = Globals.quickfireTime;
        while (countdownTimer > 0)
        {
            var countdownDisplay = (float)Math.Ceiling((decimal)countdownTimer);
            countdown.text = countdownDisplay.ToString();
            countdownTimer -= Time.unscaledDeltaTime;

            yield return null;
        }

        character.GetComponent<AudioSource>().PlayOneShot(character.GetComponent<CharacterController>().wrong);
        character.GetComponent<PlayerLives>().RemoveLife();
        ResetCharacter();
    }

    private void ResetCharacter()
    {
        Time.timeScale = 1;
        countdown.gameObject.SetActive(false);
        wordToTypeComponent.gameObject.SetActive(false);
        mode = TypeMode.Run;
        character.GetComponent<CharacterAnimation>().SetWalk(true);
    }
}

public enum TypeMode
{
    Run,
    Quickfire,
    QuickfireEnd,
    GameOver,
    TitleScreen,
    Done
}
