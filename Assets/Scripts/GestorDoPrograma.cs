using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GestorDoPrograma : MonoBehaviour
{
    //Este código permite o acesso ao GestorDoPrograma de qualquer outro script

    //Declaração de membro da class como estática
    public static GestorDoPrograma Instancia;

    //Variável do tipo cor
    public Color corMontacargas;

    //Awake(Despertador) é chamado quando a instancia do script for carregada
    private void Awake()
    {
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

        //Carregar cor, se existir no ficheiro
        CarregarCor();

    }
    //Se não é necessário guardar todos os dados de uma class então podemos criar uma class para lidar apenas com os dados que precisamos:
    [Serializable]
    public class GuardarDados
    {
        public Color corMontacargas;
    }
    public void GuardarCor()
    {
        //Criar uma nova instância da class GuardarDados
        GuardarDados dados = new GuardarDados();
        //Atribuição do valor a armazenar ao membro da class CorMontacargas 
        dados.corMontacargas = corMontacargas;
        //tranformar a instância em JSON
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
