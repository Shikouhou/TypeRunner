using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour {

    public List<GameObject> hearts;
    public GameObject particles;

    private int lives = 3;
    private FadePanel darkness;
    private bool dead;

    public bool IsDead()
    {
        return dead;
    }

    private void Start()
    {
        darkness = GetComponent<CharacterController>().darkness;
    }

    void Update () {
		if (lives <= 0)
        {
            dead = true;
            StartCoroutine(InitiateGameOver());
        }
	}

    public IEnumerator InitiateGameOver()
    {
        darkness.visible = true;
        GetComponent<PointsAdder>().SaveScore();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameOver");
    }

    public void RemoveLife()
    {
        if (dead)
        {
            return;
        }

        GameObject g = Instantiate(particles);
        g.GetComponent<DestroySelfParticles>().SetPosition(hearts[hearts.Count - lives].transform.position);
        hearts[hearts.Count - lives].SetActive(false);
        lives--;
    }
}
