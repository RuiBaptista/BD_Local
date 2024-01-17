using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using static GestorPrograma;
using UnityEngine.SceneManagement;

public class Registar : MonoBehaviour
{
    //variáveis necessárias para guardar o nome e pass do utilizador
    public InputField nomeUtilizador;
    public InputField passwordUtilizador;
    //btnSubmeter
    public Button btnSubmeter;

    public void GuardarDados()
    {
        GestorPrograma.Instancia.Utilizador = nomeUtilizador.text;
        GestorPrograma.Instancia.Pontuacao = 0;
        GestorPrograma.Instancia.Password = GerarPassword.Hash(passwordUtilizador.text);
        //inicia uma Corotina chamada Regista 
        GestorPrograma.Instancia.GuardarDadosUtilizador();


        //Carregar Menu Principal 
        MenuPrincipal();
    }
  
    
    public void VerificarInputs()
    {
        //verificar se campos nome e password foram prenchidos com 8 ou mais caracteres
        //Permitir clicar no botão btnSubmeter apenas se a seguinte condição se verificar:
        btnSubmeter.interactable = (nomeUtilizador.text.Length >= 3 && passwordUtilizador.text.Length >= 8);

    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }

}
