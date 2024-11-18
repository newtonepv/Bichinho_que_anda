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
    [SerializeField] float tempoDaGeracao;

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


            if (scriptsRedesNeurais[i].GetRedeNeuralNotaSO().GetIncializado()==false)//se n�o estiver inicializada a rede neural (pra come�ar com pesos aleat�rios
            //CRIAR AS REDES NEURAIS
            {
                Inicializar(redeNeuralNota);
            }

            
            //para o prox individuo
        }
        /*List<float> entradas = new List<float>();
        entradas.Add(1);
        entradas.Add(0);
        entradas.Add(2);
        entradas.Add(2);
        List<float> saidas = scriptsRedesNeurais[0].GetRedeNeuralNotaSO().CalculaSaida(entradas);
        for (int j = 0; j < saidas.Count; j++)
        {
            Debug.Log(saidas[j]);
        }
        entradas = new List<float>();
        entradas.Add(1);
        entradas.Add(0);
        entradas.Add(3);
        entradas.Add(4);
        saidas = scriptsRedesNeurais[0].GetRedeNeuralNotaSO().CalculaSaida(entradas);
        for (int j = 0; j < saidas.Count; j++)
        {
            Debug.Log(saidas[j]);
        }*/



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

        //a primeira camada recebe um tratamento diferente, porque no lugar de por uma quantidade de pesos pro n�, igual � quantidade de n�s na camada anterior
        //a gente p�e uma quantidade de pesos igual � da entrada, visto que a entrada � a camada anterior
        List<No> listaPrimeirosNos = new List<No>();
        for (int j = 0; j < qtdNosOrganizadosPorCamada[0]; j++)
        {
            No novoNo = new No();
            List<float> pesos = new List<float>();

            for (int k = 0; k < numEntradas; k++)
            {
                pesos.Add(Mutar(0));//adiciona o peso respectivo � entrada, mutando o 0 obtemos um numero aleatorio dentre mutacao e -mutacao
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
        if(Time.time - comeco>tempoDaGeracao)
        {
            Resetar();
        }
    }

    private void Resetar()
    {
        comeco = Time.time;

        float pontuacaoPrimeiro = scriptsRedesNeurais[0].GetRedeNeuralNotaSO().GetPontuacao();
        int indexDoPrimeiro = 0;
        float pontuacaoSegundo = scriptsRedesNeurais[1].GetRedeNeuralNotaSO().GetPontuacao();
        int indexDoSegundo = 1;
        
        for(int i=0;i< scriptsRedesNeurais.Count; i++)//achar o melhor
        {
            float pontuacaoDoAtual = scriptsRedesNeurais[i].GetRedeNeuralNotaSO().GetPontuacao();
            if (pontuacaoDoAtual < pontuacaoPrimeiro)
            {
                indexDoPrimeiro = i;
                pontuacaoPrimeiro = pontuacaoDoAtual;
            }
        }

        for(int i=0; i< scriptsRedesNeurais.Count; i++)
        {
            if(i == indexDoPrimeiro) { continue; }
            float pontuacaoDoAtual = scriptsRedesNeurais[i].GetRedeNeuralNotaSO().GetPontuacao();
            
            if(pontuacaoDoAtual< pontuacaoSegundo)
            {
                indexDoSegundo = i;
                pontuacaoSegundo = pontuacaoDoAtual;
            }
        }
        for (int i = 0; i < scriptsRedesNeurais.Count; i++)
        {
            scriptsRedesNeurais[i].transform.position = locPadraoDosBichos[i];
            scriptsRedesNeurais[i].GetRedeNeuralNotaSO().SetPontuacao(0);
        }

        CrossoverDosDoisMelhores(indexDoPrimeiro, indexDoSegundo);
    }

    private void CrossoverDosDoisMelhores(int indexDoMelhor, int indexDoSegundoMelhor)
    {

        RedeNeuralNotaSO melhor = scriptsRedesNeurais[indexDoMelhor].GetRedeNeuralNotaSO();
        RedeNeuralNotaSO segundo = scriptsRedesNeurais[indexDoSegundoMelhor].GetRedeNeuralNotaSO();

        for(int i=0; i< scriptsRedesNeurais.Count; i++)
        {
            if (i != indexDoMelhor && i != indexDoSegundoMelhor)
            {
                RedeNeuralNotaSO individuo = scriptsRedesNeurais[i].GetRedeNeuralNotaSO();

                List<Camada> camadasMelhor = melhor.GetCamadas();
                List<Camada> camadasSegundo = segundo.GetCamadas();
                List<Camada> camadasIndividuo = new List<Camada>();

                for (int j=0;j< camadasMelhor.Count; j++) 
                {
                    Camada novaCamada = new Camada();

                    List<No> nosMelhor = camadasMelhor[j].GetNos();
                    List<No> nosSegundo = camadasSegundo[j].GetNos();
                    List<No> nosIndividuo = new List<No>();

                    for(int k=0; k<nosMelhor.Count; k++)
                    {
                        No novoNo = new No();

                        List<float> pesosMelhor = nosMelhor[k].GetPesos();
                        List<float> pesosSegundo = nosSegundo[k].GetPesos();
                        List<float> pesosIndividuo = new List<float>();

                        for (int l=0;l< pesosMelhor.Count; l++) 
                        {
                            pesosIndividuo.Add(Mutar( (pesosMelhor[l]+pesosSegundo[l])/2 )); //mutamos a m�dia dos dois melhores
                        }

                        novoNo.SetPesos(pesosIndividuo);
                        novoNo.SetBias(Mutar( ( nosMelhor[k].GetBias() + nosSegundo[k].GetBias() )/2 ) );

                        nosIndividuo.Add( novoNo );
                    }

                    novaCamada.SetNos(nosIndividuo);

                    camadasIndividuo.Add(novaCamada);
                }

                individuo.SetCamadas( camadasIndividuo );
            }
        }

    }
}

