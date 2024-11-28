using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float cameraHeight;
    public float cameraVertical;


    public float cameraFollowSpeed;
    public bool cameraTargetOnline = true;
    public GameObject target;


    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    private IEnumerator CameraInit()
    {
        yield return new WaitForSeconds(2);
        CameraMovements(cameraTargetOnline);
    }
    public void CameraMovements(bool online)
    {

        if (online)
        {
            Vector3 targetposition = new Vector3(target.transform.position.x, cameraHeight, target.transform.position.z);




            transform.position = Vector3.Slerp(transform.position, targetposition, cameraFollowSpeed * Time.deltaTime);
        }


        }

    private void Update()
    {
        CameraMovements(cameraTargetOnline);


    }
}
