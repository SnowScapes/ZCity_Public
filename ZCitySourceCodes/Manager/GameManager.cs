using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public ObjectPool pool;
    public GameObject player;

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    public IEnumerator GameOverCoroutine()
    {
        UIManager.Instance.GameOverText.SetActive(true);
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}