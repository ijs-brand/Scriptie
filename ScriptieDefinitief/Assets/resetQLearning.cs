using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetQLearning : MonoBehaviour
{
    public int botsCount = 0;
    public List<QLearning> autos = new List<QLearning>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            autos.Add(child.gameObject.GetComponent<QLearning>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (qTable.reset)
        {
            foreach (QLearning auto in autos)
            {
                auto.Reset();
            }
        }
    }
}
