using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Ambiente : MonoBehaviour
{
    [SerializeField] List<GameObject> individuos = new List<GameObject>();
    [SerializeField] float mutacao;
    [SerializeField] int entradasDasRedesNeurais;
    [SerializeField] List<int> Ncamadas;
    List<RedeNeural> scriptsRedesNeurais = new List<RedeNeural>();
    private void Start()
    {
        for(int i = 0; i< individuos.Count; i++)
        {
            scriptsRedesNeurais.Add(individuos[i].GetComponentInChildren<RedeNeural>());
            if (scriptsRedesNeurais[i].GetRedeNeuralNotaSO().GetIncializado()==false)
            //CRIAR AS REDES NEURAIS
            {
                RedeNeuralNotaSO redeNeutalNota = scriptsRedesNeurais[i].GetRedeNeuralNotaSO();
                redeNeutalNota.SetIncializado(true);
                List<Camada> camadasMesmo = new List<Camada>();

                for(int j=0;j< Ncamadas.Count; j++)
                {//pra cada camada
                    for(int k=0; k < Ncamadas[j]; k++)
                    {//pra cada no
                        No novoNo = new No();

                    }
                }

                
                
            }
        }
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] randomBytes = new byte[4];  // 4 bytes para cada float (32 bits)

        rng.GetBytes(randomBytes);

        int randomInt = BitConverter.ToInt32(randomBytes, 0);
        float randomFloat = float.Parse((randomInt % 101).ToString())/ 100;
        Debug.Log(randomFloat);
        
    }

    
}