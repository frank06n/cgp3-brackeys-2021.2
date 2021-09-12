using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsMenuScript : MonoBehaviour
{
    private bool escPressed = false;
    void Update()
    {
        if (!escPressed && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager2.instance.LoadScene(0);
        }
    }
}
