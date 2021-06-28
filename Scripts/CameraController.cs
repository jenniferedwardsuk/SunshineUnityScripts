using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    GameObject player;
    Camera camerasettings;
    public GameObject angleAnchor;
    public int maxZoom;
    public int minZoom;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camerasettings = gameObject.GetComponent<Camera>();
        if (camerasettings)
            newzoom = camerasettings.fieldOfView;
        else
            Debug.LogError("main camera not found for zoom");
    }

    bool amendRotation;
    float newzoom = 50;
    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            //if (player != null)
            //    transform.LookAt(player.transform);
        }
        else
        {
            //move
            this.transform.parent.position = player.transform.position;

            //zoom
            if (camerasettings && Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                newzoom = Mathf.Clamp(camerasettings.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * 50, minZoom, maxZoom);
            }
            else if (!camerasettings)
            {
                Debug.LogError("main camera not found for zoom");
            }
            camerasettings.fieldOfView = Mathf.Lerp(camerasettings.fieldOfView, newzoom, 0.2f);

            //rotate
            if (Input.GetMouseButton(1)) // hold right-mouse to rotate camera
            {
                amendRotation = false;
                transform.RotateAround(transform.parent.position, Vector3.up, Input.GetAxis("Mouse X") * 5);
            }
            else // idle camera rotates to behind player
            {
                if (player.GetComponent<PlayerController>() && player.GetComponent<PlayerController>().moving)
                {
                    amendRotation = true;
                }
                    
                if (amendRotation)
                {
                    //angleAnchor.transform.position = transform.parent.position + player.transform.forward * (new Vector3(0,10,10));

                    //Debug.Log("unadjusted rotation " + angleAnchor.transform.rotation);
                        //Vector3 newRotation = new Vector3(0,
                        //    player.transform.eulerAngles.y,//(player.transform.eulerAngles.y - 180) % 360, 
                        //    0);

                    //angleAnchor.transform.LookAt(player.transform);
                    //this.transform.parent.rotation = Quaternion.Slerp(this.transform.parent.rotation, Quaternion.LookRotation(transform.parent.forward*-1), 0.15f);
                    //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, angleAnchor.transform.rotation, 0.15f);

                    ////Vector3 direction = transform.parent.position - this.transform.position;
                    //Quaternion toRotation = angleAnchor.transform.rotation; // Quaternion.FromToRotation(transform.forward, transform.parent.forward);
                    //Debug.Log("adjusted rotation " + angleAnchor.transform.rotation + ", parent rotation " + transform.parent.rotation);

                        //transform.parent.eulerAngles = Vector3.Lerp(transform.parent.eulerAngles, newRotation, 0.1f); // * Time.deltaTime

                        //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, angleAnchor.transform.rotation, 0.1f); // * Time.deltaTime
                        //this.transform.position = Vector3.Lerp(this.transform.position, angleAnchor.transform.position, 0.1f); // * Time.deltaTime

                    //angleAnchor.transform.eulerAngles = new Vector3(30, -180, 0);
                    //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, angleAnchor.transform.rotation, 0.15f);
                }
            }

            if (player)
            {
                transform.LookAt(player.transform);
                Vector3 adjustedAngle = transform.eulerAngles;
                adjustedAngle.x -= 10;
                transform.eulerAngles = adjustedAngle;
            }
        }
    }
}
