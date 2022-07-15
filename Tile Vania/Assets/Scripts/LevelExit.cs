using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public float LevelSloMo = 0.2f;
    public float LevelLoadDelay = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextLevel());
    }
    IEnumerator LoadNextLevel()
    {
        Time.timeScale = LevelSloMo;
        yield return new WaitForSecondsRealtime (LevelLoadDelay);
        Time.timeScale = 1f;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }


}
