using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RedeNeural : MonoBehaviour
{
    [SerializeField] private RedeNeuralNotaSO redeNeuralNotaSO;
    [SerializeField] private GameObject objetivo;
    Rigidbody rb;
    int DebugCounter = 0;


    public RedeNeuralNotaSO GetRedeNeuralNotaSO()
    {
        return redeNeuralNotaSO;
    }
    public void SetRedeNeuralNotaSO(RedeNeuralNotaSO redeNeuralNotaSO)
    {
        this.redeNeuralNotaSO = redeNeuralNotaSO;
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
        Vector2 locDoObjetivo = new Vector2(objetivo.transform.localPosition.x, objetivo.transform.localPosition.z);
        Vector2 locDoBicho = new Vector2(transform.localPosition.x, transform.localPosition.z);

        List<float> entrada = new List<float>();
        entrada.Add(locDoObjetivo.x);
        entrada.Add(locDoObjetivo.y);
        entrada.Add(locDoBicho.x);
        entrada.Add(locDoBicho.y);

        List<float> saida = redeNeuralNotaSO.CalculaSaida(entrada);
        rb.velocity = new Vector3(saida[0], rb.velocity.y, saida[1]) ;

        /*if (DebugCounter%50==0&&gameObject.tag=="DEBUG")
        {
            Debug.Log(locDoBicho.x+" "+ locDoBicho.y);
            Debug.Log(rb.velocity.x+ " "+ rb.velocity.z);
        }*/

        redeNeuralNotaSO.SetPontuacao(CalculoPontuacao(locDoBicho, locDoObjetivo));

        DebugCounter++;
    }
    private float CalculoPontuacao(Vector2 bicho, Vector2 obj)
    {
        return redeNeuralNotaSO.GetPontuacao() + (Time.deltaTime * (bicho-obj).magnitude);
    }
}
