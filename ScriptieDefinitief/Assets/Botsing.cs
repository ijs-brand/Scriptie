using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botsing : MonoBehaviour
{
    AutoScript1 auto1;
    QLearning auto;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<QLearning>())
        {
            auto = collider.GetComponent<QLearning>();
            auto.snelheid = 0;
            auto.gebotst = true;
            auto.Reset();
        }
        if (collider.GetComponent<AutoScript1>())
        {
            auto1 = collider.GetComponent<AutoScript1>();
            auto1.moveSpeed = 0;
            auto1.gebotst = true;
        }
    }
}
