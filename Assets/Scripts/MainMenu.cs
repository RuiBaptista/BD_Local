using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private Text InfoUtilizador;
    private void Start()
    {
       VerificarUtilizador();
    }

    public void VerificarUtilizador()
    {
        GestorPrograma.Instancia.CarregarDadosUtilizador();
    }

    public void MenuRegisto()
    {
        SceneManager.LoadScene(1);
    }
    public void MenuLogin()
    {
        SceneManager.LoadScene(2);
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }
    public void Jogo()
    {
        SceneManager.LoadScene(3);
    }


}
