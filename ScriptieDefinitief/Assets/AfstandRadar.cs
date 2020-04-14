using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfstandRadar : MonoBehaviour
{
    private static AfstandRadar d = new AfstandRadar(0,0,0,0,0);
    public float TargetDistanceFWD;
    public float TargetDistanceLeft, TargetDistanceRight, TargetDistanceLeft45, TargetDistanceRight45;

    public AfstandRadar(float TargetDistanceLeft, float TargetDistanceLeft45, float TargetDistanceFWD, float TargetDistanceRight45, float TargetDistanceRight)
    {
        this.TargetDistanceLeft = TargetDistanceLeft;
        this.TargetDistanceLeft45 = TargetDistanceLeft45;
        this.TargetDistanceFWD = TargetDistanceFWD;
        this.TargetDistanceRight45 = TargetDistanceRight45;
        this.TargetDistanceRight = TargetDistanceRight;
    }
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

        RaycastHit hitFWD;
        RaycastHit hitLeft;
        RaycastHit hitRight;
        RaycastHit hitLeft45;
        RaycastHit hitRight45;

        Vector3 Rechts = (Quaternion.Euler(0, 180, 0) * Vector3.forward);
        Vector3 Rechts45 = (Quaternion.Euler(0, 120, 0) * Vector3.forward);
        Vector3 FWD = (Quaternion.Euler(0, 90, 0) * Vector3.forward);
        Vector3 Links45 = (Quaternion.Euler(0, 60, 0) * Vector3.forward);
        Vector3 Links = (Quaternion.Euler(0, 0, 0) * Vector3.forward);


        // Vanaf positie, in de richting, totdat het hit
        if (Physics.Raycast(transform.position, transform.TransformDirection(Rechts), out hitRight))
        {
            TargetDistanceRight = hitRight.distance;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Rechts45), out hitRight45))
        {
            TargetDistanceRight45 = hitRight45.distance;
        }

        if (Physics.Raycast (transform.position, transform.TransformDirection(FWD), out hitFWD))
        {
            TargetDistanceFWD = hitFWD.distance;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Links45), out hitLeft45))
        {
            TargetDistanceLeft45 = hitLeft45.distance;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Links), out hitLeft))
        {
            TargetDistanceLeft = hitLeft.distance;
        }
        

        d.TargetDistanceLeft = TargetDistanceLeft;
        d.TargetDistanceLeft45 = TargetDistanceLeft45;
        d.TargetDistanceFWD = TargetDistanceFWD;
        d.TargetDistanceRight45 = TargetDistanceRight45;
        d.TargetDistanceRight = TargetDistanceRight;

        //if(NeuralNetwork.debugRadar == true)
        {
            // Maximale lengte is: middengrootte * bucketsize

            float lengteFWD = d.TargetDistanceFWD;
            //if (lengteFWD >= qTable.middengrootte * qTable.bucketsize)
            //{
            //    lengteFWD = qTable.middengrootte * qTable.bucketsize;
            //}
            float lengteLeft45 = d.TargetDistanceLeft45;
            //if (lengteLeft45 >= qTable.linksgrootte * qTable.bucketsize)
            //{
            //    lengteLeft45 = qTable.linksgrootte * qTable.bucketsize;
            //}
            float lengteRight45 = d.TargetDistanceRight45;
            //if (lengteRight45 >= qTable.rechtsgrootte * qTable.bucketsize)
            //{
            //    lengteRight45 = qTable.rechtsgrootte * qTable.bucketsize;
            //}
            float lengteRight = d.TargetDistanceRight;
            //if (lengteRight >= qTable.rechtsrechts * qTable.bucketsize)
            //{
            //    lengteRight = qTable.rechtsrechts * qTable.bucketsize;
            //}
            float lengteLeft = d.TargetDistanceLeft;
            //if (lengteLeft >= qTable.linkslinks * qTable.bucketsize)
            //{
            //    lengteLeft = qTable.linkslinks * qTable.bucketsize;
            //}

            //debug
            Vector3 forward = transform.TransformDirection(Quaternion.Euler(0, 90, 0) * Vector3.forward) * lengteFWD;
            Debug.DrawRay(transform.position, forward, Color.red);

            // debug
            Vector3 left45 = transform.TransformDirection(Quaternion.Euler(0, 60, 0) * Vector3.forward) * lengteLeft45;
            Debug.DrawRay(transform.position, left45, Color.blue);

            // debug
            Vector3 right45 = transform.TransformDirection(Quaternion.Euler(0, 120, 0) * Vector3.forward) * lengteRight45;
            Debug.DrawRay(transform.position, right45, Color.blue);

            // debug
            Vector3 right = transform.TransformDirection(Quaternion.Euler(0, 180, 0) * Vector3.forward) * lengteRight;
            Debug.DrawRay(transform.position, right, Color.green);

            // debug
            Vector3 left = transform.TransformDirection(Quaternion.Euler(0, 0, 0) * Vector3.forward) * lengteLeft;
            Debug.DrawRay(transform.position, left, Color.green);
        }
    }
}
