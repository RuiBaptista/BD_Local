using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class Registar : MonoBehaviour
{
    //variáveis necessárias para guardar o nome e pass do utilizador
    [SerializeField]
    private InputField nome, nomeUtil, email, pass, confpass;
    //btnSubmeter
    public Button btnSubmeter;
    //Label erros
    [SerializeField]
    private Text erro;



    BD bd = new BD();

    public void GuardarDadosJson()
    {
        //Guardar dados da instância
        GestorPrograma.Instancia.Utilizador = nome.text;
        GestorPrograma.Instancia.Pontuacao = 0;
        GestorPrograma.Instancia.Password = GerarPassword.GerarPass(pass.text, GerarPassword.Salt());

       //continuar aqui a guardar dados se necessário...
    
        //inicia uma Corotina chamada Regista 
        GestorPrograma.Instancia.GuardarDadosUtilizador();


        //Carregar Menu Principal 
        //MenuPrincipal();
    }

    //Criar utilizador
    public void Submeter()
    {
        int pontuacao = 0;
        DateTime datatime = DateTime.Now;
        string formatarData = datatime.ToString("yyyy-MM-dd HH:mm:ss");

        if (pass.text == confpass.text)
        {
            //Veridicar se BD Existe e se não criar BD
            bd.Criar();

            if (bd.VerificarSeExiste(nomeUtil.text) == true)
            {
                erro.text = "Nome de Utilizador já existe";
            }
            else
            {
                if (bd.CriarUtilizador(nome.text, nomeUtil.text, email.text, pass.text, pontuacao, formatarData))
                {
                    //Guardar dos jason
                    //GuardarDadosJson();
                    MenuPrincipal();
                }
                else
                {
                    erro.text = "Não foi possível criar o utilizador";
                }
            }
        }
        else
        {
            erro.text = "Pass e confirmação não são iguas";
        }
    }

    public void VerificarInputs()
    {
        //verificar se campos nome e password foram prenchidos com 8 ou mais caracteres
        //Permitir clicar no botão btnSubmeter apenas se a seguinte condição se verificar:
        btnSubmeter.interactable = (nome.text.Length >= 3 && pass.text.Length >= 8 && confpass.text.Length >= 8);

    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }

}
