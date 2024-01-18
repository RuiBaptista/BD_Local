using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{

    //Variáveis necessárias
    [SerializeField]
    private InputField nomeUser, pass;
    [SerializeField]
    private Text erroLogin;

    [SerializeField]
    private Button btnSubmeter;

    BD bd = new BD();

    public void Submeter()
    {

        if (bd.IniciarSessao(nomeUser.text, GerarPassword.GerarPass(pass.text, bd.GetSalt(nomeUser.text))) == true && nomeUser.text != "" && pass.text != "")
        {
            GestorPrograma.Instancia.Utilizador = nomeUser.text;
            //Carregar dados instancia
            GestorPrograma.Instancia.CarregarDadosInstancia();
            //Carregar dados Jason
            //GestorPrograma.Instancia.CarregarDadosUtilizador();
            SceneManager.LoadScene(0);
        }
        else
        {
            erroLogin.text = "Erro: Utilizador ou pass inválidos";
        }
    }


    public void Voltar()
    {
        SceneManager.LoadScene(0);
    }


    public void VerificarInputs()
    {
        //verificar se campos nome e password foram prenchidos com 8 ou mais caracteres
        //Permitir clicar no botão btnSubmeter apenas se a seguinte condição se verificar:
        btnSubmeter.interactable = (nomeUser.text.Length >= 3 && pass.text.Length >= 8);
    }
}
