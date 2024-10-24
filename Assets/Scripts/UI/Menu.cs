using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Jugar()
    {  
        SceneManager.LoadScene("GameScene");
    }

    public void Logo()
    {
        // funcion especifica para la primera pantalla
        // una vez pulsas el logo se te redirige al menu de inicio
        SceneManager.LoadScene("MenuInicio");
    }

    public void Salir()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

}
