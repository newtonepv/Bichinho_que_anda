using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "PesosENota", menuName = "PesosENota")]
public class RedeNeuralNotaSO : ScriptableObject
{
    [SerializeField] private List<Camada> camadas = new List<Camada>();
    [SerializeField] private float pontuacao;
    [SerializeField] private bool incializado;
    public List<Camada> GetCamadas()
    {
        return camadas;
    }
    public void SetCamadas(List<Camada> camadas)
    {
        this.camadas = camadas;
    }
    public float GetPontuacao()
    {
        return pontuacao;
    }
    public void SetPontuacao(float pontuacao)
    {
        this.pontuacao = pontuacao;
    }
    public bool GetIncializado()
    {
        return incializado;
    }
    public void SetIncializado(bool incializado)
    {
        this.incializado = incializado;
    }
    public List<float> CalculaSaida(List<float> entrada)
    {

        for (int i = 0; i < camadas.Count; i++)
        {
            entrada = camadas[i].CalcularSaida(entrada);//meio que uma recursividade, a entrada da próxima é a saída dessa
        }
        return entrada;
    }
}

[System.Serializable]
public class Camada
{

    [SerializeField] private List<No> nos = new List<No>();


    public List<No> GetNos()
    {
        return nos;
    }
    public void SetNos(List<No> Nos)
    {
        this.nos = Nos;
    }


    public List<float> CalcularSaida(List<float> entradasEmOrdem)
    {
        List<float> saidas = new List<float>();
        for(int i = 0; i<nos.Count; i++)
        {
            saidas.Add(nos[i].CalcularSaida(entradasEmOrdem));
        }
        return saidas;
    }
}

[System.Serializable]
public class No
{
    private List<float> pesos;
    private float vies;

    public No()
    {
        
    }

    public List<float> GetPesos()
    {
        return pesos;
    }
    public void SetPesos(List <float> pesos)
    {
        this.pesos = pesos;
    }
    public float GetBias()
    {
        return vies;
    }
    public void SetBias(float vies)
    {
        this.vies = vies;
    }


    public float CalcularSaida(List<float> entradasEmOrdem)
    {
        float somaPonderada = 0;
        for(int i = 0; i< pesos.Count; i++)
        {
            somaPonderada += pesos[i] * entradasEmOrdem[i];
        }
        return (float)Math.Tanh(somaPonderada+vies);
    }

   
}
