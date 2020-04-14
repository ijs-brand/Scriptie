using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QLearning : MonoBehaviour
{
    public float learningRate;
    public float reward;
    static int linkslinks = qTable.linkslinks;
    static int linksgrootte = qTable.linksgrootte;
    static int rechtsgrootte = qTable.rechtsgrootte;
    static int rechtsrechts = qTable.rechtsrechts;
    static int middengrootte = qTable.middengrootte;
    public float[] inputValues = new float[6];
    public bool gebotst = false;
    private Vector3 defPos;
    private Quaternion defRot;
    private Vector3 defScale;
    private Vector3 lastPosition;
    public float distanceTraveled = 0;
    public int actie;
    public int linkshoek, links, midden, rechts, rechtshoek, newlinkshoek, newlinks, newmidden, newrechts, newrechtshoek;
    public int maxdigit;
    public double max;
    public int botsCount = 0;
    int stuurrichting = 0;
    int aantalacties = qTable.aantalacties;
    public double totalreward = 0;
    public float genreward = 0;
    public double maximaal = 0;
    public float snelheid = 0.2f;
    public int snelheidbucket = 0;
    int maxsnelheid = 10;
    float visits;
    bool netnieuw = true;
    public int aantalRonden;

    // Start is called before the first frame update
    void Start()
    {
        reward = 0;
        aantalRonden = 0;
        // De beginwaardes worden opgeslagen.
        (defRot, defPos, defScale) = (transform.rotation, transform.position, transform.localScale);
        lastPosition = transform.position;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (netnieuw == false && gebotst == false)// && qTable.plekken.Contains((linkshoek, links, midden, rechts, rechtshoek, maxdigit)) == false)
        {
            // De afgelegde afstand wordt bepaald.
            distanceTraveled = Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;

            // De distance en de reward worden opgeteld.
            qTable.totaldistance += distanceTraveled;
            reward = 0.01f;
            genreward += distanceTraveled;

            // Radargegevens worden opgevraagd
            radarGegevens();

            // De nieuwe input wordt toebedeeld.
            newlinkshoek = bucketMethod(inputValues[0], linkslinks, qTable.bucketsize);
            newlinks = bucketMethod(inputValues[1], linksgrootte, qTable.bucketsize);
            newmidden = bucketMethod(inputValues[2], middengrootte, qTable.bucketsize);
            newrechts = bucketMethod(inputValues[3], rechtsgrootte, qTable.bucketsize);
            newrechtshoek = bucketMethod(inputValues[4], rechtsrechts, qTable.bucketsize);

            // De visits in de Q-Table worden opgeteld.
            qTable.QTable[linkshoek, links, midden, rechts, rechtshoek, maxdigit].Item2 += 1;
            visits = qTable.QTable[linkshoek, links, midden, rechts, rechtshoek, maxdigit].Item2;

            // De learning rate wordt bepaald.
            learningRate = 1 / (1 + visits);

            // Max digit wordt berekend
            int newmaxdigit = Random.Range(0, aantalacties);
            double max_future_q = qTable.QTable[newlinkshoek, newlinks, newmidden, newrechts, newrechtshoek, 0].Item1;
            for (int i = 1; i < aantalacties - 1; i++)
            {
                if (qTable.QTable[newlinkshoek, newlinks, newmidden, newrechts, newrechtshoek, i].Item1 > max_future_q)
                {
                    max_future_q = qTable.QTable[newlinkshoek, newlinks, newmidden, newrechts, newrechtshoek, i].Item1;
                    newmaxdigit = i;
                }
            }

            // De current_q is de huidige Q-waarde (voor de input de hoogste mogelijkheid).
            double current_q = max;
            double new_q = (1 - learningRate) * current_q + learningRate * (reward + qTable.discount * max_future_q);
            qTable.QTable[linkshoek, links, midden, rechts, rechtshoek, maxdigit].Item1 = new_q;
            //qTable.plekken.Add((linkshoek, links, midden, rechts, rechtshoek, maxdigit));
        }
        netnieuw = false;

        //snelheidbucket = bucketMethod(snelheid, qTable.snelheidsize, (maxsnelheid / qTable.snelheidsize));

        // De radargegevens worden opgevraagd voor de volgende run.
        radarGegevens();

        // Dit zijn de gegevens die de volgende beurt gebruikt worden.
        linkshoek = bucketMethod(inputValues[0], linkslinks, qTable.bucketsize);
        links = bucketMethod(inputValues[1], linksgrootte, qTable.bucketsize);
        midden = bucketMethod(inputValues[2], middengrootte, qTable.bucketsize);
        rechts = bucketMethod(inputValues[3], rechtsgrootte, qTable.bucketsize);
        rechtshoek = bucketMethod(inputValues[4], rechtsrechts, qTable.bucketsize);

        // Max value wordt berekend
        maxdigit = Random.Range(0, aantalacties);
        max = qTable.QTable[linkshoek, links, midden, rechts, rechtshoek, 0].Item1;
        for (int i = 1; i < aantalacties; i++)
        {
            if (qTable.QTable[linkshoek, links, midden, rechts, rechtshoek, i].Item1 > max)
            {
                max = qTable.QTable[linkshoek, links, midden, rechts, rechtshoek, i].Item1;
                maxdigit = i;
            }
        }

        // Random actie
        if (qTable.epsilon > Random.Range(0f, 1f))
        {
            maxdigit = (int)Random.Range(0, aantalacties);
        }
               
        // Actie wordt bepaald
        bepaalActie();
        
        // Sturen van de auto
        float rotateSpeed = 15f * snelheid;

        // Niet sturen
        transform.Rotate(Vector3.up * rotateSpeed * stuurrichting * Time.deltaTime);

        // Gas geven
        transform.Translate(snelheid * Time.deltaTime, 0f, 0f);
                    
        reward = 0.01f;

    }
    public void Reset()
    {
        if (aantalRonden > qTable.max)
        {
            qTable.max = aantalRonden;
            //print("totalreward = " + qTable.totalreward/qTable.aantalAutos + ", gen: " + qTable.gen+", max: " + qTable.max + ", epsilon = " + qTable.epsilon + ", learning = " + learningRate + ", visits: " + visits);
        }

        // Variabelen worden gereset
        netnieuw = true;
        distanceTraveled = 0;
        aantalRonden = 0;

        // Reward wordt bepaald
        reward = -10f;
        genreward += reward;

        int newmaxdigit = 0;
        double max_future_q = qTable.QTable[newlinkshoek, newlinks, newmidden, newrechts, newrechtshoek, 0].Item1;
        for (int i = 1; i < aantalacties - 1; i++)
        {
            if (qTable.QTable[newlinkshoek, newlinks, newmidden, newrechts, newrechtshoek, i].Item1 > max_future_q)
            {
                max_future_q = qTable.QTable[newlinkshoek, newlinks, newmidden, newrechts, newrechtshoek, i].Item1;
                newmaxdigit = i;
            }
        }

        double current_q = max;
        double new_q = (1 - learningRate) * current_q + learningRate * (reward + qTable.discount * max_future_q);
        qTable.QTable[linkshoek, links, midden, rechts, rechtshoek, maxdigit].Item1 = new_q;

        qTable.totalreward += genreward;
        qTable.gen += 1;
        
        qTable.epsilon *= 0.9998f;
        genreward = 0;
        //snelheid = 0.2f;
        lastPosition = defPos;
        gebotst = false;

        (transform.rotation, transform.position, transform.localScale) = (defRot, defPos, defScale);
    }
    public static int bucketMethod(float a, int max, float bucketsize)
    {
        float result = a / bucketsize;
        int count = (int)System.Math.Floor(result);

        if (count > max - 1)
            count = max - 1;

        return count;
    }
    public void radarGegevens()
    {
        AfstandRadar Radar = GetComponentInChildren<AfstandRadar>();

        inputValues[0] = Radar.TargetDistanceLeft;
        inputValues[1] = Radar.TargetDistanceLeft45;
        inputValues[2] = Radar.TargetDistanceFWD;
        inputValues[3] = Radar.TargetDistanceRight45;
        inputValues[4] = Radar.TargetDistanceRight;
    }
    void bepaalActie()
    {
        if (maxdigit == 0)
        {
            actie = 1;
            stuurrichting = 0;
        }
        else if (maxdigit == 1)
        {
            actie = 1;
            stuurrichting = 1;
        }
        else if (maxdigit == 2)
        {
            actie = 1;
            stuurrichting = -1;
        }
        //if (maxdigit == 3)
        //{
        //    actie = -1;
        //    stuurrichting = 0;
        //}
        //if (maxdigit == 5)
        //{
        //    actie = 1;
        //    stuurrichting = 0;
        //}
        //if (maxdigit == 6)
        //{
        //    actie = -1;
        //    stuurrichting = 1;
        //}
        //if (maxdigit == 7)
        //{
        //    actie = 0;
        //    stuurrichting = 1;
        //}
        //if (maxdigit == 8)
        //{
        //    actie = 0;
        //    stuurrichting = 0;
        //}
        //if (maxdigit == 9)
        //{
        //    print("yo");
        //}
        //if (maxdigit == 4)
        //{
        //    actie = 1;
        //    stuurrichting = 1;
        //}

        if (snelheid < maxsnelheid && gebotst == false)
        {
            if (actie == 1)
            {
                snelheid += 1f;
            }
            //if (actie == 0 && snelheid > 0.5)
            //{
            //    snelheid -= 0.1f;
            //}
        }
        //if (snelheid > 0.5 && gebotst == false)
        //{
        //    if (actie == -1)
        //    {
        //        snelheid -= 0.3f;
        //    }
        //}

        //public void Restart()
        //{
        //    (transform.rotation, transform.position, transform.localScale) = (defRot, defPos, defScale);
        //    genreward = 0;
        //    stuurrichting = 0;
        //    distanceTraveled = 0;
        //}
    }

}
