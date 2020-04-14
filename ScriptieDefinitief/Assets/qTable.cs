using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class qTable : MonoBehaviour
{
    public static int aantalAutos = 0;
    public int botsCount = 0;
    //public float leren = 0.08f;
    // bij generaties werkt 0.5 wellicht. Anders 0.08.
    //public static float learningRate = 0.08f;
    public static float discount = 0.99f;
    public static float epsilon = 0.2f;
    public static int linkslinks = 5;
    public static int linksgrootte = 8;
    public static int middengrootte = 15;
    public static int rechtsgrootte = 8;
    public static int rechtsrechts = 5;
    //public static int snelheidsize = 5;
    public static int aantalacties = 3;
    // een bucketsize van 0.9 werkt goed.
    public static float bucketsize;
    public static (double, int)[,,,,,] QTable;
    public static int gen = 0;
    public static int generatie = 0;
    public static double totalreward = 0;
    public static double max = 0;
    public static float maxMoveSpeed = 10;
    public static bool[] gebotst;
    public static List<QLearning> autos = new List<QLearning>();
    public static int frame = 0;
    public static double totaldistance;
    public static double totdif = 0;
    public static int ronden;
    public static bool reset = false;
    static int aantalGeneraties;
    static double gemiddeldedist;
    static double verschil;

    static string path = @"C:\Users\Ysbra\Scriptie\Q-Learning_extra_data2.txt";
    static string path2 = @"C:\Users\Ysbra\Scriptie\Q-Learning-eind-resultaat2.txt";

    //public static HashSet<(int, int, int, int, int, int)> plekken;

    // Start is called before the first frame update
    void Start()
    {
        //plekken = new HashSet<(int, int, int, int, int, int)>();

        aantalGeneraties = 1;

        ronden = 0;

        bucketsize = 0.9f;

        FillQTable();

        Time.timeScale = 5f;
        foreach (Transform child in transform)
        {
            autos.Add(child.gameObject.GetComponent<QLearning>());
            aantalAutos += 1;
        }

        voegToePublic();

        print(frame + ", " + gen + ", " + totaldistance + ", " + totaldistance / aantalAutos + ", " + ronden + ", " + 0);

        gebotst = new bool[aantalAutos];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //plekken.Clear();
        frame += 1;
        if (frame % 250 == 0)
        {
            gemiddeldedist = 0;
            foreach (Transform child in transform)
            {
                QLearning auto = child.gameObject.GetComponent<QLearning>();
                gemiddeldedist += auto.genreward;
            }


            gemiddeldedist = gemiddeldedist / aantalAutos;

            verschil = (totaldistance / aantalAutos - totdif);

            print(frame + ", " + gen + ", " + totaldistance + ", " + totaldistance / aantalAutos + ", " + verschil + ", " + ronden + ", " + gemiddeldedist + ", " + epsilon);
            totdif = totaldistance / aantalAutos;

        }
    }
    public static void voegToePublic()
    {
        voegToe(frame + ", " + gen + ", " + totaldistance + ", " + totaldistance / aantalAutos + ", " + verschil + ", " + ronden + ", " + gemiddeldedist, path);
    }
    static void FillQTable()
    {
        QTable = new (double, int)[linkslinks, linksgrootte, middengrootte, rechtsgrootte, rechtsrechts, aantalacties];
        for (int h = 0; h < linkslinks - 1; h++)
        {
            for (int i = 0; i < linksgrootte - 1; i++)
            {
                for (int j = 0; j < middengrootte - 1; j++)
                {
                    for (int k = 0; k < rechtsgrootte - 1; k++)
                    {
                        for (int l = 0; l < rechtsrechts - 1; l++)
                        {
                            for (int m = 0; m < aantalacties - 1; m++)
                            {
                                QTable[h, i, j, k, l, m] = (0f,0);
                            }
                        }
                    }
                }
            }
        }

    }
    public static void voegToe(string mean, string path)
    {
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(mean);
        }
    }
    public static void Reset()
    {
        voegToe(frame + ", " + gen + ", " + ronden + ", " + aantalGeneraties, path2);

        FillQTable();
        aantalGeneraties += 1;
        ronden = 0;
        epsilon = 0.2f;
        gen = 0;
        frame = 0;
        max = 0;
        totalreward = 0;
        totaldistance = 0;
        gemiddeldedist = 0;
        verschil = 0;
        voegToePublic();

        foreach (QLearning autos in autos)
        {
            autos.Reset();
        }

        if(aantalGeneraties == 50)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }

        reset = false;
    }
}
