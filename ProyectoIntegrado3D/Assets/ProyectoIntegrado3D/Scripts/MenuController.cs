using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Botones de MainMenu")]
    public Button button_MainMenu_Play;
    public Button button_MainMenu_Exit;

    void Awake()
    {
        button_MainMenu_Play.onClick.AddListener(LoadLevel1);
        button_MainMenu_Exit.onClick.AddListener(CloseGame);
    }

    void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    void CloseGame()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}
