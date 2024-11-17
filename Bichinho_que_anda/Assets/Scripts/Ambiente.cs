using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class Ambiente : MonoBehaviour
{
    [SerializeField] List<GameObject> individuos = new List<GameObject>();
    [SerializeField] float mutacao;
    [SerializeField] List<int> qtdNosOrganizadosPorCamada;
    [SerializeField] int numEntradas;

    List<RedeNeural> scriptsRedesNeurais = new List<RedeNeural>();
    RandomNumberGenerator rng = RandomNumberGenerator.Create();
    float comeco;
    List<Vector3> locPadraoDosBichos = new List<Vector3>();

    private void Start()
    {
        comeco = Time.time;


        for (int i = 0; i< individuos.Count; i++)
        {

            scriptsRedesNeurais.Add(individuos[i].GetComponentInChildren<RedeNeural>());
            RedeNeuralNotaSO redeNeuralNota = scriptsRedesNeurais[i].GetRedeNeuralNotaSO();

            locPadraoDosBichos.Add(scriptsRedesNeurais[i].transform.position);


            if (scriptsRedesNeurais[i].GetRedeNeuralNotaSO().GetIncializado()==false)//se não estiver inicializada a rede neural (pra começar com pesos aleatórios
            //CRIAR AS REDES NEURAIS
            {
                Inicializar(redeNeuralNota);
            }

            /*List<float> entradas = new List<float>();
            entradas.Add(1);
            entradas.Add(0);
            entradas.Add(2);
            entradas.Add(2);
            List<float> saidas = redeNeuralNota.CalculaSaida(entradas);
            for (int j=0;j< saidas.Count; j++)
            {
                Debug.Log(saidas[j]);
            }*/
            //para o prox individuo
        }
        
        
        
        
    }
    private float Mutar(float valor)
    {

        byte[] randomBytes = new byte[4];  // 4 bytes para cada float (32 bits)

        rng.GetBytes(randomBytes);

        int randomInt = BitConverter.ToInt32(randomBytes, 0);
        float randomFloat = float.Parse((randomInt % 101).ToString()) / 100;
        randomFloat *= mutacao;
        return valor + randomFloat;
    }

    private void Inicializar(RedeNeuralNotaSO redeNeuralNota)
    {
        redeNeuralNota.SetIncializado(true);//inicializar

        List<Camada> camadasDaRede = new List<Camada>();

        //a primeira camada recebe um tratamento diferente, porque no lugar de por uma quantidade de pesos pro nó, igual à quantidade de nós na camada anterior
        //a gente põe uma quantidade de pesos igual à da entrada, visto que a entrada é a camada anterior
        List<No> listaPrimeirosNos = new List<No>();
        for (int j = 0; j < qtdNosOrganizadosPorCamada[0]; j++)
        {
            No novoNo = new No();
            List<float> pesos = new List<float>();

            for (int k = 0; k < numEntradas; k++)
            {
                pesos.Add(Mutar(0));//adiciona o peso respectivo à entrada, mutando o 0 obtemos um numero aleatorio dentre mutacao e -mutacao
            }

            novoNo.SetPesos(pesos);
            novoNo.SetBias(Mutar(0));

            listaPrimeirosNos.Add(novoNo);
        }
        Camada primeiraCamada = new Camada();
        primeiraCamada.SetNos(listaPrimeirosNos);

        camadasDaRede.Add(primeiraCamada);


        for (int j = 1; j < qtdNosOrganizadosPorCamada.Count; j++)
        {//percorre as camadas a partir da segunda

            List<No> nos = new List<No>();

            for (int k = 0; k < qtdNosOrganizadosPorCamada[j]; k++)
            {//pra cada no

                No novoNo = new No();

                List<float> pesos = new List<float>();

                for (int l = 0; l < qtdNosOrganizadosPorCamada[j - 1]; l++)//preenche pela quantidade de nos na camada anterior
                {
                    pesos.Add(Mutar(0));
                }
                novoNo.SetPesos(pesos);
                novoNo.SetBias(Mutar(0));

                nos.Add(novoNo);//completando a lista dos nos
            }
            Camada camadaNova = new Camada();
            camadaNova.SetNos(nos);//transformando de lista de nos para camada

            //camada pronta
            camadasDaRede.Add(camadaNova);
        }
        //agora que todas as camadas estao prontas vamos adicionar a lista de camadas na rede

        redeNeuralNota.SetCamadas(camadasDaRede);

    }

    private void Update()
    {
        if(Time.time - comeco>10)
        {
            Resetar();
        }
    }

    private void Resetar()
    {
        comeco = Time.time;

        RedeNeuralNotaSO melhorIndv = scriptsRedesNeurais[0].GetRedeNeuralNotaSO();
        int indexDoMelhor = 0;
        RedeNeuralNotaSO segundoMelhorIndv = scriptsRedesNeurais[1].GetRedeNeuralNotaSO();
        int indexDoSegundoMelhor = 1;

        for(int i =0; i < scriptsRedesNeurais.Count; i++)
        {
            scriptsRedesNeurais[i].transform.position = locPadraoDosBichos[i];

            if (melhorIndv.GetPontuacao() >= scriptsRedesNeurais[i].GetRedeNeuralNotaSO().GetPontuacao())
            {
                //atualiza o menor
                segundoMelhorIndv = melhorIndv;
                indexDoSegundoMelhor = indexDoMelhor;

                melhorIndv = scriptsRedesNeurais[i].GetRedeNeuralNotaSO();
                indexDoMelhor = i;
            }else if (segundoMelhorIndv.GetPontuacao() > scriptsRedesNeurais[i].GetRedeNeuralNotaSO().GetPontuacao())
            {
                segundoMelhorIndv = scriptsRedesNeurais[i].GetRedeNeuralNotaSO();
                indexDoSegundoMelhor = i;
            }

            Debug.Log(scriptsRedesNeurais[i].GetRedeNeuralNotaSO().GetPontuacao());
            //scriptsRedesNeurais[i].GetRedeNeuralNotaSO().SetPontuacao(0);

        }
        Debug.Log(indexDoMelhor);
        Debug.Log(indexDoSegundoMelhor);
        Debug.Log(scriptsRedesNeurais[indexDoMelhor].GetRedeNeuralNotaSO().GetPontuacao());
        Debug.Log(scriptsRedesNeurais[indexDoSegundoMelhor].GetRedeNeuralNotaSO().GetPontuacao());
    }

    private void CrossoverDosDoisMelhores(int indexDoMelhor, int indexDoSegundoMelhor)
    {

    }
}

