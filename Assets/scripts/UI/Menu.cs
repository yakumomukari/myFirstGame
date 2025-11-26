using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject newGameBotton;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(newGameBotton);
    }
    public void ExitGame()
    {
        Application.Quit();
        // Debug.Log("?");
    }
}
