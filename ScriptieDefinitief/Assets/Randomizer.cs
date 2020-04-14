using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public static int botsCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(1);
    }


    public static float RandomBias()
    {
        return Random.Range(-2.0f, 2.0f);
    }
    public static float RandomWeight()
    {
        return Random.Range(-1.0f, 1.0f);
    }
}
