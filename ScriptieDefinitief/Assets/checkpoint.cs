using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    AutoScript1 auto1;
    QLearning auto;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<QLearning>())
        {
            auto = collider.GetComponent<QLearning>();
            auto.aantalRonden += 1;

            if (auto.aantalRonden > qTable.ronden)
            {
                qTable.ronden = auto.aantalRonden;
                qTable.voegToePublic();

                if(auto.aantalRonden == 10)
                {
                    qTable.Reset();
                }
            }

        }
        if (collider.GetComponent<AutoScript1>())
        {
            auto1 = collider.GetComponent<AutoScript1>();
            auto1.aantalRonden += 1;
            
            if (auto1.aantalRonden > NeuralNetwork.ronden)
            {
                print(NeuralNetwork.frame + ", " + auto1.aantalRonden);
                NeuralNetwork.ronden = auto1.aantalRonden;
                NeuralNetwork.voegToePublic();

                if(auto1.aantalRonden == 10)
                {
                    NeuralNetwork.Resette();
                }
            }

            //if (auto.aantalRonden == 10)
            //{
            //    NeuralNetwork.Reset();
            //}
        }
    }
}
