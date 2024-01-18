using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.ShaderData;
public class GerarPassword : MonoBehaviour
{
    //Gerar Salt
    public static string Salt()
    {
        var random = new RNGCryptoServiceProvider();

        // Maximum length of salt
        int max_length = 32;

        // Empty salt array
        byte[] salt = new byte[max_length];

        // Build the random bytes
        random.GetNonZeroBytes(salt);

        // Return the string encoded salt
        return Convert.ToBase64String(salt);
    }
    public static string GerarPass(string pass, string salt)
    {
        string passUser;
        passUser = ComputeHash(pass, salt);
        Debug.Log("Pass gerada: " + passUser + "Salt Gerado: " + salt);
        return passUser;

    }
    public static string ComputeHash(string passwordPlainText, string saltString)
    {

        // Convert plain text into a byte array.
        byte[] saltBytes = Encoding.UTF8.GetBytes(saltString);

        // Convert plain text into a byte array.
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(passwordPlainText);

        // Allocate array, which will hold plain text and salt.
        byte[] plainTextWithSaltBytes =
                new byte[plainTextBytes.Length + saltBytes.Length];

        // Copy plain text bytes into resulting array.
        for (int i = 0; i < plainTextBytes.Length; i++)
            plainTextWithSaltBytes[i] = plainTextBytes[i];

        // Append salt bytes to the resulting array.
        for (int i = 0; i < saltBytes.Length; i++)
            plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

        // Because we support multiple hashing algorithms, we must define
        // hash object as a common (abstract) base class. We will specify the
        // actual hashing algorithm class later during object creation.
        HashAlgorithm hash;

        hash = new SHA256Managed();

        // Compute hash value of our plain text with appended salt.
        byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

        // Create array which will hold hash and original salt bytes.
        byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                            saltBytes.Length];

        // Copy hash bytes into resulting array.
        for (int i = 0; i < hashBytes.Length; i++)
            hashWithSaltBytes[i] = hashBytes[i];

        // Append salt bytes to the result.
        for (int i = 0; i < saltBytes.Length; i++)
            hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

        // Convert result into a base64-encoded string.
        string hashValue = Convert.ToBase64String(hashWithSaltBytes);

        // Return the result.
        return hashValue;
    }
}
