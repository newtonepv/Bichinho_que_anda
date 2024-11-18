using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RedeNeural : MonoBehaviour
{
    [SerializeField] private RedeNeuralNotaSO redeNeuralNotaSO;
    [SerializeField] private GameObject objetivo;
    Rigidbody rb;


    public RedeNeuralNotaSO GetRedeNeuralNotaSO()
    {
        return redeNeuralNotaSO;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        redeNeuralNotaSO.SetPontuacao(0);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 locDoObjetivo = new Vector2(objetivo.transform.position.x, objetivo.transform.position.z);
        Vector2 locDoBicho = new Vector2(transform.position.x, transform.position.z);

        List<float> entrada = new List<float>();
        entrada.Add(locDoObjetivo.x);
        entrada.Add(locDoObjetivo.y);
        entrada.Add(locDoBicho.x);
        entrada.Add(locDoBicho.y);

        List<float> saida = redeNeuralNotaSO.CalculaSaida(entrada);
        rb.velocity = new Vector3(saida[0], 0, saida[1]);

        redeNeuralNotaSO.SetPontuacao(CalculoPontuacao(locDoBicho, locDoObjetivo));

    }
    private float CalculoPontuacao(Vector2 bicho, Vector2 obj)
    {
        return redeNeuralNotaSO.GetPontuacao() + (Time.deltaTime * (bicho-obj).magnitude);
    }
}
