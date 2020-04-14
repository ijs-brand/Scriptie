using UnityEngine;

public class CameraBijhouden : MonoBehaviour
{
    public Transform target;
    public float sSpeed = 10.0f;
    public Vector3 offset;
    public Transform lookTarget;
    

    void Update()
    {
        Vector3 dPos = target.position + offset;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, sSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.position);



        //offset = new Vector3(-2, 1, 0);
        //transform.position = target.position + offset;
    }
}
