using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoScript1 : MonoBehaviour
{
    // Auto's uit de testgroep bevatten een multi-layer hidden netwerk. 
    // De input zijn de vijf sensoren en de snelheid.
    // Er is een hidden layer met 5 lagen, en een hidden layer met 4 lagen.
    // Er zijn drie outputs. 

    // Netwerk informatie
    private int inputCount = NeuralNetwork.inputCount;
    private int hiddenCount = NeuralNetwork.hiddenCount;
    private int secondHiddenCount = NeuralNetwork.secondHiddenCount;
    private int outputCount = NeuralNetwork.outputCount;
    // Snelheid informatie
    public float moveSpeed = NeuralNetwork.moveSpeed;
    private static float rotateSpeed = NeuralNetwork.rotateSpeed;
    public bool gebotst = false;
    // Gewichten informatie
    public float[] inputValues = new float[6];
    public float[] inputLayers;
    public float[,] weightsInput;
    public float[,] weightsHidden;
    public float[,] weightsSecondHidden;
    private double[] hoogste;
    // Bias informatie
    public float biasInput;
    public float biasHidden;
    public float biasSecondHidden;
    // Start informatie
    private Vector3 defPos;
    private Quaternion defRot;
    private Vector3 defScale;
    private AfstandRadar d;

    public int aantalRonden = 0;
    
    private Vector3 lastPosition;
    public float distanceTraveled = 0;
    
    // Start is called before the first frame update
    private void Start()
    {
        moveSpeed = 0;
        AfstandRadar Radar = GetComponentInChildren<AfstandRadar>();

        // De beginwaardes worden opgeslagen.
        (defRot, defPos, defScale) = (transform.rotation, transform.position, transform.localScale);

        lastPosition = transform.position;

        if (NeuralNetwork.multiHidden == false)
            (biasInput, biasHidden, weightsInput, weightsHidden) = NeuralNetwork.NieuwNetwerk(inputCount, hiddenCount, outputCount);
        else
            (biasInput, biasHidden, biasSecondHidden, weightsInput, weightsHidden, weightsSecondHidden) = NeuralNetwork.NieuwDiepNetwerk(inputCount, hiddenCount, secondHiddenCount, outputCount);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Afstand
        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        
        // Versnelling van de auto
        if (moveSpeed < NeuralNetwork.moveSpeed && gebotst == false)
        {
            moveSpeed += NeuralNetwork.accelerationSpeed;
        }
        // Sturen van de auto
        rotateSpeed = 15f * moveSpeed;

        // Vraagt de gegevens op bij de radar.
        AfstandRadar Radar = GetComponentInChildren<AfstandRadar>();

        // Input
        inputValues[0] = Radar.TargetDistanceLeft;
        inputValues[1] = Radar.TargetDistanceLeft45;
        inputValues[2] = Radar.TargetDistanceFWD;
        inputValues[3] = Radar.TargetDistanceRight45;
        inputValues[4] = Radar.TargetDistanceRight;
        // moveSpeed wordt meegegeven als parameter. Wordt gedeeld door maximale moveSpeed om te normaliseren.
        //inputValues[5] = moveSpeed;


        // Er wordt een nieuwe array aangemaakt waar de output in komt te staan.
        float[] outputValues = new float[NeuralNetwork.outputCount];

        if (NeuralNetwork.multiHidden == false)
        {
            float[]hiddenValues = NeuralNetwork.calculateOutcome(weightsInput, biasInput, inputValues);
            outputValues = NeuralNetwork.calculateOutcome(weightsHidden, biasHidden, hiddenValues);
        }
        else
        {
            float[] hiddenValues = NeuralNetwork.calculateOutcome(weightsInput, biasInput, inputValues);
            float[] secondHiddenValues = NeuralNetwork.calculateOutcome(weightsHidden, biasHidden, hiddenValues);
            outputValues = NeuralNetwork.calculateOutcome(weightsSecondHidden, biasSecondHidden, secondHiddenValues);
        }


        // Verwerking van de output.
        if (NeuralNetwork.enkeleOutput == true)
        {
            //if (moveSpeed < maxsnelheid && moveSpeed > 1.0 && gebotst == false)
            //{
            //    moveSpeed += (float)outputValues[1];
            //}

            // Snelheid
            transform.Translate(moveSpeed * Time.deltaTime, 0f, 0f);

            // Draaisnelheid
            rotateSpeed = moveSpeed * 15f;

            // Draaien
            transform.Rotate(Vector3.up * rotateSpeed * (float)outputValues[0] * Time.deltaTime);
        }
        else
        {
            if (outputValues[0] == outputValues.Max())
                transform.Rotate(Vector3.up * rotateSpeed * 0 * Time.deltaTime);
            else if (outputValues[1] == outputValues.Max())
                transform.Rotate(Vector3.up * rotateSpeed * 1 * Time.deltaTime);
            else
                transform.Rotate(Vector3.up * rotateSpeed * -1 * Time.deltaTime);

            transform.Translate(moveSpeed * 1f * Time.deltaTime, 0f, 0f);
        }



    }
    /// <summary>
    /// Methode om de auto te resetten als hij botst.
    /// </summary>
    public void Reset()
    {
        //moveSpeed = 0;
        aantalRonden = 0;
        lastPosition = defPos;
        gebotst = false;
        distanceTraveled = 0;

        if(NeuralNetwork.reset)
            (biasInput, biasHidden, biasSecondHidden, weightsInput, weightsHidden, weightsSecondHidden) = NeuralNetwork.NieuwDiepNetwerk(inputCount, hiddenCount, secondHiddenCount, outputCount);
     

        (transform.rotation, transform.position, transform.localScale) = (defRot, defPos, defScale);
    }
}
