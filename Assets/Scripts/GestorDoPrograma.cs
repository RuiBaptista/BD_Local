using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GestorDoPrograma : MonoBehaviour
{
    //Este c�digo permite o acesso ao GestorDoPrograma de qualquer outro script

    //Declara��o de membro da class como est�tica
    public static GestorDoPrograma Instancia;

    //Vari�vel do tipo cor
    public Color corMontacargas;

    //Awake(Despertador) � chamado quando a instancia do script for carregada
    private void Awake()
    {
        //Se inst�ncia n�o for nula ent�o 
        if (Instancia != null)
        {
            Destroy(gameObject);
            return;
        }

        //Informa o programa que � desta inst�ncia que estamos a tratar
        Instancia = this;
        //M�todo que informa a aplica��o para n�o destruir o objeto marcado quando � carregada uma cena.
        DontDestroyOnLoad(gameObject);

        //Carregar cor, se existir no ficheiro
        CarregarCor();

    }
    //Se n�o � necess�rio guardar todos os dados de uma class ent�o podemos criar uma class para lidar apenas com os dados que precisamos:
    [Serializable]
    public class GuardarDados
    {
        public Color corMontacargas;
    }
    public void GuardarCor()
    {
        //Criar uma nova inst�ncia da class GuardarDados
        GuardarDados dados = new GuardarDados();
        //Atribui��o do valor a armazenar ao membro da class CorMontacargas 
        dados.corMontacargas = corMontacargas;
        //tranformar a inst�ncia em JSON
        string json = JsonUtility.ToJson(dados);
        //escrever dados para um ficheiro
        File.WriteAllText(Application.persistentDataPath + "/guardardados.json", json);
        //Debug.Log(Application.persistentDataPath);
    }

    public void CarregarCor()
    {
        string path = Application.persistentDataPath + "/guardardados.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GuardarDados data = JsonUtility.FromJson<GuardarDados>(json);

            corMontacargas = data.corMontacargas;
        }
    }
}
