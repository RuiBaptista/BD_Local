using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GestorPrograma : MonoBehaviour
{
    //Acesso BD
    BD bd = new BD();
    public static GestorPrograma Instancia { get; private set; }

    private string utilizador;
    public string Utilizador {
        get { return utilizador; }
        set { utilizador = value; }
    }

    private string password;
    public string Password
    {
        get { return password; }
        set { password = value; }
    }

    private int pontuacao;
    public int Pontuacao
    {
        get { return pontuacao; }
        set { pontuacao = value; }
    }

    //Verificar se sessão do utilizador já está iniciada
    public bool SessaoIniciada() {

        if(utilizador != null){
            return true;
        }
        else
        {
            return false;
        }
    }
    //Fazer reset ao utilizador
    public void LoggOut()
    {
        utilizador = null;
    }

    //Awake(Despertador) é chamado quando a instancia do script for carregada
    private void Awake()
    {
        //2.	Garanta a Existência de apenas um GestorDoPrograma durante a sua execução.
        //Se instância não for nula então 
        if (Instancia != null)
        {
            Destroy(gameObject);
            return;
        }
        //Informa o programa que é desta instância que estamos a tratar
        Instancia = this;
        //Método que informa a aplicação para não destruir o objeto marcado quando é carregada uma cena.
        DontDestroyOnLoad(gameObject);

    }

    //Se não é necessário guardar todos os dados de uma class então podemos criar uma class para lidar apenas com os dados que precisamos:
    [Serializable]
    public class GuardarDados
    {
        public string utilizador;
        public string password;
        public int pontuacao;
    }

    public void GuardarDadosUtilizador()
    {
        //Criar uma nova instância da class GuardarDados
        GuardarDados dados = new GuardarDados();
        //Atribuição do valor a armazenar ao membro da class CorMontacargas 
        dados.utilizador = utilizador;
        dados.password = password;
        dados.pontuacao= pontuacao; 
        //tranformar a instância em JSON
        string json = JsonUtility.ToJson(dados);
        //escrever dados para um ficheiro
        File.WriteAllText(Application.persistentDataPath + "/guardardados.json", json);
        Debug.Log(Application.persistentDataPath);
    }

    public void CarregarDadosUtilizador()
    {
        string path = Application.persistentDataPath + "/guardardados.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GuardarDados data = JsonUtility.FromJson<GuardarDados>(json);

            utilizador = data.utilizador;
            pontuacao = data.pontuacao;
        }
    }

    public void CarregarDadosInstancia()
    {
        bd.DadosJogo(utilizador);
    }

}
