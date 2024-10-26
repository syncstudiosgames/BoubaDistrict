using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Jugar()
    {  
        SceneManager.LoadScene("Character Selection");
    }

    public void Logo()
    {
        // funcion especifica para la primera pantalla
        // una vez pulsas el logo se te redirige al menu de inicio
        SceneManager.LoadScene("MenuInicio");
    }

    public void Credits()
    {
        // acceso a creditos 
        SceneManager.LoadScene("Credits");
    }

    public void Salir()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    public void MainMenu()
    {
        Debug.Log("Inicio");
        SceneManager.LoadScene("Inicio");
    }

}
