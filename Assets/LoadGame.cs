using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public float time = 3;
    void Start()
    {
        StartCoroutine(LoadLevel(time));
    }

    IEnumerator LoadLevel(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Game");
    }
}
