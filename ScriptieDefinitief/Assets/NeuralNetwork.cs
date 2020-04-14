using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class NeuralNetwork : MonoBehaviour
{
    // 5 - 5 - 4 - 3 werkt goed
    public static int inputCount = 5;
    public static int hiddenCount = 4;
    public static int secondHiddenCount = 3;
    public static int outputCount = 1;

    public static int normalizeFactor = 1;
    public static float maxDistance = 0.5f;
    public static bool debugRadar = false;

    public static int botsCount = 0;

    // De rotateSpeed moet ongeveer 15 keer de moveSpeed zijn. Tenzij enkeleOutput true is.

    public static float moveSpeed = 10;
    public static float accelerationSpeed = 1f;
    public static float rotateSpeed = 15f * moveSpeed;

    public static bool enkeleOutput = true;
    public static bool multiHidden = true;

    public static List<AutoScript1> autos = new List<AutoScript1>();
    public static int aantalAutos = 0;
    public static float[] rewardList;
    public static float[] rewardListCopy;
    public static bool[] gebotst;
    public static int winnaar;
    public static int gen;

    public static int frame = 0;
    public static int poging = 1;

    public static int nummer = 0;
    public static float grens;

    public static int ronden = 0;
    public static bool reset = false;

    static string path = @"C:\Users\Ysbra\Scriptie\Neuroevolution-extra_data.txt";
    static string path2 = @"C:\Users\Ysbra\Scriptie\Neuroevolution-eindresultaat.txt";

    public void Start()
    {
        voegToePublic();

        Time.timeScale = 5f;

        NeuralNetwork.gen = 1;
        
        foreach (Transform child in transform)
        {
            autos.Add(child.gameObject.GetComponent<AutoScript1>());
            aantalAutos += 1;
        }

        rewardList = new float[aantalAutos];
        rewardList = new float[aantalAutos];
        rewardListCopy = new float[aantalAutos];
        gebotst = new bool[aantalAutos];
    }


    private void FixedUpdate()
    {
        // Terminate
        //if (NeuralNetwork.gen == 1000)
        //{
        //    UnityEditor.EditorApplication.isPlaying = false;
        //}

        for (int i = 1; i < rewardList.Length + 1; i++)
        {
            rewardList[i - 1] = autos[i - 1].distanceTraveled;
        }
        for(int i = 1; i < rewardList.Length + 1; i++)
        {
            if(rewardList[i - 1] == rewardList.Max())
            {
                winnaar = i - 1;
            }
        }
        rewardListCopy = rewardList;

        System.Array.Sort(rewardListCopy);
        System.Array.Reverse(rewardListCopy);
        
        grens = rewardListCopy[aantalAutos / 10];
        
        //if(ronden == 10)
        //{
        //    print(frame + ", " + poging);
        //    voegToe(frame + ", " + poging, path);
        //    Resette();
        //}

        frame++;

    }

    public static float[,] AssignWeights(int currentLayer, int nextLayer)
    {
        float[,] weights = new float[currentLayer, nextLayer];

        // Voor iedere laag wordt ruimte gereserveerd voor de weights.
        for (int i = 0; i < currentLayer; i++)
        {
            // Voor ieder gewicht wordt een willekeurig nummer bepaald.
            for (int j = 0; j < nextLayer; j++)
            {
                weights[i, j] = Randomizer.RandomWeight();
            }
        }
        return weights;
    }
    public static float[] calculateOutcome(float[,] inputArray, float bias, float[] inputValues)
    {
        float[] newValues = new float[inputArray.GetLength(1)];

        for (int j = 0; j < inputArray.GetLength(1); j++)
        {
            float som = 0;
            for (int i = 0; i < inputArray.GetLength(0); i++)
            {
                som += inputArray[i, j] * inputValues[i];
            }
            som += bias;
            newValues[j] = (float)System.Math.Tanh(som);
        }
        return newValues;
    }
    public static (float, float, float[,], float[,]) NieuwNetwerk(int inputCount, int HiddenCount, int outputCount)
    {
        // De biases krijgen een willekeurige bias.
        float biasInput = UnityEngine.Random.Range(-2.0f, 2.0f);
        float biasHidden = UnityEngine.Random.Range(-2.0f, 2.0f);

        // De weights worden willekeurig toegewezen.
        float[,] weightsInput = NeuralNetwork.AssignWeights(inputCount, HiddenCount);
        float[,] weightsHidden = NeuralNetwork.AssignWeights(HiddenCount, outputCount);

        return (biasInput, biasHidden, weightsInput, weightsHidden);
    }

    public static (float, float, float, float[,], float[,], float[,]) NieuwDiepNetwerk(int inputCount, int firstHiddenCount, int secondHiddenCount, int outputCount)
    {
        // De biases krijgen een willekeurige bias.
        float biasInput = Random.Range(-2.0f, 2.0f);
        float biasHidden = Random.Range(-2.0f, 2.0f);
        float biasSecondHidden = Random.Range(-2.0f, 2.0f);


        // De weights worden willekeurig toegewezen.
        float[,] weightsInput = NeuralNetwork.AssignWeights(inputCount, firstHiddenCount);
        float[,] weightsFirstHidden = NeuralNetwork.AssignWeights(firstHiddenCount, secondHiddenCount);
        float[,] weightsSecondHidden = NeuralNetwork.AssignWeights(secondHiddenCount, outputCount);

        return (biasInput, biasHidden, biasSecondHidden, weightsInput, weightsFirstHidden, weightsSecondHidden);
    }
    public static void CrossOverWeights(float[,] parentOne, float[,] parentTwo, float[,] child)
    {
        for (int j = 0; j < parentOne.GetLength(0); j++)
        {
            for (int i = 0; i < parentOne.GetLength(1); i++)
            {
                int a = Random.Range(0, 22);
                if (a < 10)
                {
                    child[j, i] = parentOne[j, i];
                }
                else if (a >= 10 && a < 20)
                {
                    child[j, i] = parentTwo[j,i];
                }
                else
                {
                    child[j, i] = Random.Range(-1f, 1f);
                }
            }
        }
    }
    public static void Willekeurig(float[,] een)
    {
        for (int j = 0; j < een.GetLength(0); j++)
        {
            for (int i = 0; i < een.GetLength(1); i++)
            {
                een[j, i] = Random.Range(-1.0f, 1.0f);
            }
        }
    }
    public static void CrossOverBias(float parentOne, float parentTwo, float child)
    {
        int a = (Random.Range(0, 22));
        if (a < 10)
            child = parentOne;
        else if (a < 20 && a >= 10)
            child = parentTwo;
        else
            child = Random.Range(-2.0f, 2.0f);
    }

    public static void voegToe(string mean, string path)
    {
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(mean);
        }
    }
    public static void Resette()
    {
        reset = true;
        voegToe(frame + ", " + gen + ", " + ronden + ", " + poging, path2);
        foreach(AutoScript1 auto in autos)
        {
            auto.Reset();
        }
        reset = false;
        poging++;

        if(poging == 21)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }

        frame = 0;
        ronden = 0;
        voegToePublic();
    }

    public static void voegToePublic()
    {
        voegToe(frame + ", " + gen + ", " + ronden, path);
    }
}
