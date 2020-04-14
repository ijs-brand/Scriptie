using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Methode om de auto's te resetten als allemaal zijn gebotst.
/// </summary>
public class ResetAll : MonoBehaviour
{
    int botsCount = 0;
    bool teVer = false;
    int framecount;
    int gen;
    public static AutoScript1 winnaar;
    public List<AutoScript1> autos = new List<AutoScript1>();
    int parentOne, parentTwo;
    public List<AutoScript1> nieuweRonde = new List<AutoScript1>();


    // Alle auto's worden toegevoegd aan de lijst met autos.
    void Start()
    {
        framecount = 0;
        foreach (Transform child in transform)
        {
            autos.Add(child.gameObject.GetComponent<AutoScript1>());
        }
    }

    void Update()
    {
        framecount += 1;
        // Iteratie door de lijst met auto's
        for (int i = 0; i < autos.Count; i++)
        {
            // Aantal auto's dat is gebotst
            if (autos[i].gebotst == true)
            {
                botsCount += 1;
            }
            // Afstand die de auto heeft afgelegd
            //if (framecount >= 2000)
            //{
            //    teVer = true;
            //}
        }

        // Resetmethode van de auto's
        if (botsCount == NeuralNetwork.aantalAutos)// || teVer == true)
        {
            // Winnaar wordt bijgehouden
            winnaar = autos[NeuralNetwork.winnaar];
            
            // Er wordt geïteerd door de lijst
            for (int i = 0; i < autos.Count; i++)
            {
                // Auto's worden toegevoegd aan nieuweRonde als ze bij de top 20% zitten.
                if (autos[i].distanceTraveled >= NeuralNetwork.grens)
                {
                    nieuweRonde.Add(autos[i]);
                }
            }
            for (int i = 0; i < autos.Count; i++)
            {
                parentOne = Random.Range(0, nieuweRonde.Count);
                parentTwo = Random.Range(0, nieuweRonde.Count);
                //print(nieuweRonde.Count + " " + willekeurig);
                
                // Auto's krijgen een ander neuraal netwerk als ze niet bij de beste 20% horen of niet voldoende afstand hebben afgelegd.
                if (autos[i].distanceTraveled < NeuralNetwork.grens)
                {
                    NeuralNetwork.CrossOverWeights(nieuweRonde[parentOne].weightsInput, nieuweRonde[parentTwo].weightsInput, autos[i].weightsInput);
                    NeuralNetwork.CrossOverWeights(nieuweRonde[parentOne].weightsHidden, nieuweRonde[parentTwo].weightsHidden, autos[i].weightsHidden);
                    NeuralNetwork.CrossOverWeights(nieuweRonde[parentOne].weightsSecondHidden, nieuweRonde[parentTwo].weightsSecondHidden, autos[i].weightsSecondHidden);
                    NeuralNetwork.CrossOverBias(nieuweRonde[parentOne].biasInput, nieuweRonde[parentTwo].biasInput, autos[i].biasInput);
                    NeuralNetwork.CrossOverBias(nieuweRonde[parentOne].biasHidden, nieuweRonde[parentTwo].biasHidden, autos[i].biasHidden);
                    NeuralNetwork.CrossOverBias(nieuweRonde[parentOne].biasSecondHidden, nieuweRonde[parentTwo].biasSecondHidden, autos[i].biasSecondHidden);
                }
                               
                // Zet de auto terug op zijn oorspronkelijke plek
                autos[i].Reset();
            }
            //print("Max: " + NeuralNetwork.rewardListCopy[0] + ", Min: " + NeuralNetwork.rewardListCopy[autos.Count - 1] + ", Mean: " + NeuralNetwork.rewardListCopy.Average() + ", Frame: " + framecount);
            //NeuralNetwork.voegToe("Gen: " + NeuralNetwork.gen + ", Gemiddelde reward: " + NeuralNetwork.rewardListCopy.Average().ToString());
            NeuralNetwork.gen += 1;

            float rewardMean = NeuralNetwork.rewardListCopy.Average();
            teVer = false;
            nieuweRonde.Clear();
            framecount = 0;
           }
        // botsCount wordt na elke update weer op 0 gezet.
        botsCount = 0;
    }
}
