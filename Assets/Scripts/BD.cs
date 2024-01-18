using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
//Utilização do Namespace acesso BD
using System.Data;
using Mono.Data.Sqlite;
using System.Security.Cryptography.X509Certificates;
using static UnityEditor.ShaderData;

public class BD
{
    //Criar ligação a base de Dados
    private string bd = "URI=file:./jogoDB.db";
    IDbConnection ligacaoBD;
    public void Ligacao()
    {
        //Abrir ligação  
        ligacaoBD = new SqliteConnection(bd);
        ligacaoBD.Open();
    }
    public void Criar()
    {
        try
        {
            //Abrir ligação  
            Ligacao();

            // Criar BD
            IDbCommand dbcmd;
            dbcmd = ligacaoBD.CreateCommand();

            //Criar tabelas
            string q_CriarTabelas = " CREATE TABLE IF NOT EXISTS jogador (" +
                                        " id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                                        " nome VARCHAR(30)," +
                                        " nomeUtil VARCHAR(10) NOT NULL," +
                                        " email VARCHAR(50) NOT NULL," +
                                        " pass VARCHAR(256) NOT NULL," +
                                        " salt VARCHAR(100) NOT NULL);" +
                                    " CREATE TABLE IF NOT EXISTS dados_jogo (" +
                                        " id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                                        " nomeUtil VARCHAR(10) NOT NULL," +
                                        " pontuacao INT NOT NULL," +
                                        " datahora VARCHAR(20) NOT NULL );";

            //Criar tabelas
            dbcmd.CommandText = q_CriarTabelas;
            dbcmd.ExecuteReader();

            //Fechar ligação
            FecharLigacao();

            //Debug
            Debug.Log("BD e tabelas criadas com sucesso!");

        }
        catch (Exception erroLigacao)
        {
            Debug.Log(erroLigacao);
            
        }
    }

    public void FecharLigacao()
    {
        ligacaoBD.Close();
    }

    public bool CriarUtilizador(string nome, string nomeUtil, string email, string pass, int pontuacao, string formatarData)
    {
        bool valida = false;
        string salt = GerarPassword.Salt();
       

        try
        {
            Ligacao();

            IDbCommand cmnd = ligacaoBD.CreateCommand();
            cmnd.CommandText = "INSERT INTO jogador (nome, nomeutil, email, pass, salt) " +
                                    "VALUES (@nome, @nomeUtil, @email, @pass, @salt);" +
                                "INSERT INTO dados_jogo (nomeutil, pontuacao, datahora) " +
                                    "VALUES (@nomeUtil, @pontuacao, @datatime);";


            //Passar parametros
            cmnd.Parameters.Add(new SqliteParameter("@nome", nome));
            cmnd.Parameters.Add(new SqliteParameter("@nomeUtil", nomeUtil));
            cmnd.Parameters.Add(new SqliteParameter("@email", email));
            cmnd.Parameters.Add(new SqliteParameter("@pass", GerarPassword.GerarPass(pass, salt)));
            cmnd.Parameters.Add(new SqliteParameter("@salt", salt));
            cmnd.Parameters.Add(new SqliteParameter("@pontuacao", pontuacao));
            cmnd.Parameters.Add(new SqliteParameter("@datatime", formatarData));



            cmnd.ExecuteNonQuery();

            valida= true;

            if (valida == true)
            {
                // Ler e imprimir todos os dados dos utilizadores
                IDataReader reader;
                string query = "SELECT * FROM jogador";
                cmnd.CommandText = query;
                reader = cmnd.ExecuteReader();

                while (reader.Read())
                {
                    Debug.Log("id: " + reader[0].ToString());
                    Debug.Log("nome: " + reader[1].ToString());
                    Debug.Log("nomeUtil: " + reader[2].ToString());
                    Debug.Log("email: " + reader[3].ToString());
                    Debug.Log("pass: " + reader[4].ToString());
                    Debug.Log("salt: " + reader[5].ToString());
                }
            }
            //Fechar ligação
            FecharLigacao();

            return valida;


        }
        catch (Exception erroSelect)
        {
            Debug.Log(erroSelect);
            return valida;
        }
    }
    //Guardar Progresso do jogador
    public bool GuardarProgresso(string nomeUtil, string localizacaoUnit, string corMontacargas)
    {
        bool valida = false;
        try
        {
            Ligacao();

            IDbCommand cmnd = ligacaoBD.CreateCommand();
            cmnd.CommandText = " UPDATE dados_jogo SET unidlocalizacao = @unidlocalizacao, unidCor = @unidCor WHERE nomeUtil LIKE @nomeUtil ";
            //Passar parametros
            cmnd.Parameters.Add(new SqliteParameter("@nomeUtil", nomeUtil));
            cmnd.Parameters.Add(new SqliteParameter("@unidlocalizacao", localizacaoUnit));
            cmnd.Parameters.Add(new SqliteParameter("@unidCor", corMontacargas));

            cmnd.ExecuteScalar();
            
            //Fechar ligação
            FecharLigacao();
            valida = true;

            return valida;


        }
        catch (Exception erroSelect)
        {
            Debug.Log(erroSelect);
            return valida;
        }
    }

    public string GetSalt(string nomeUtil)
    {
        string salt = "";

        try
        {
            Ligacao();
            IDbCommand cmnd = ligacaoBD.CreateCommand();

            IDataReader reader;
            string query = " SELECT salt FROM jogador WHERE nomeutil LIKE @nomeUtil";
            //Passar parametros
            cmnd.Parameters.Add(new SqliteParameter("@nomeUtil", nomeUtil));
            cmnd.CommandText = query;

            reader = cmnd.ExecuteReader();

            while (reader.Read())
            {
                salt = reader[0].ToString();
            }

            return salt;
        }
        catch (Exception erroSelect)
        {
            Debug.Log(erroSelect);
            return "";
        }
    }

    public bool IniciarSessao(string nomeUtil, string pass )
    {
        try
        {
            Ligacao();
            IDbCommand cmnd = ligacaoBD.CreateCommand();

            IDataReader reader;
            string query = " SELECT nomeutil, pass FROM jogador WHERE nomeutil LIKE @nomeUtil AND pass LIKE @pass";
            //Passar parametros
            cmnd.Parameters.Add(new SqliteParameter("@nomeUtil", nomeUtil));
            cmnd.Parameters.Add(new SqliteParameter("@pass", pass));
            cmnd.CommandText = query;

            reader = cmnd.ExecuteReader();

            while (reader.Read())
            {
                string userNome = reader[0].ToString(); 
                string userPass = reader[1].ToString();
                if (nomeUtil == userNome && pass == userPass)
                {
                    Debug.Log("Pode iniciar sessão");
                    return true;
                }
                else
                {
                    return false;
                }
            }

            FecharLigacao();
            return false;
        }
        catch (Exception erroSelect)
        {
            Debug.Log(erroSelect);
            return false;
        }
    }

    public bool VerificarSeExiste(string nomeUtil)
    {
        try
        {
            int registos = 0;
            Ligacao();
            IDbCommand cmnd = ligacaoBD.CreateCommand();

            IDataReader reader;
            string query = " SELECT nomeutil FROM jogador WHERE nomeutil LIKE @nomeutil ";
            //Passar parametros
            cmnd.Parameters.Add(new SqliteParameter("@nomeutil", nomeUtil));
            cmnd.CommandText = query;

            reader = cmnd.ExecuteReader();

            while (reader.Read())
            {
                registos++;
            }

            FecharLigacao();

            if (registos == 1)
            {
                return true;

            }
            else
            {
                return false;
            }

        }
        catch (Exception erroSelect)
        {
            Debug.Log(erroSelect);
            return false;
        }
    }

    public bool DadosJogo(string nomeUtil)
    {
        try
        {
            Ligacao();
            IDbCommand cmnd = ligacaoBD.CreateCommand();

            IDataReader reader;
            string query = " SELECT unidlocalizacao, unidCor FROM dados_jogo WHERE nomeutil LIKE @nomeutil ";
            //Passar parametros
            cmnd.Parameters.Add(new SqliteParameter("@nomeutil", nomeUtil));
            cmnd.CommandText = query;

            reader = cmnd.ExecuteReader();

            FecharLigacao();
            return true;
        }
        catch (Exception erroSelect)
        {
            Debug.Log(erroSelect);
            return false;
        }
    }
    //Método que devolve a posicao guardada na BD
    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
        float.Parse(sArray[0], CultureInfo.InvariantCulture),
        float.Parse(sArray[1], CultureInfo.InvariantCulture),
        float.Parse(sArray[2], CultureInfo.InvariantCulture));

        return result;
    }
}
