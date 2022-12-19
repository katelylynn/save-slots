using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    CameraManager cameraScript;

    private Vector3 newPos = new Vector3(-5, -11, 0);

    void Start() {


    }

    void OnTriggerStay2D(Collider2D other) {

        if (other.name == "Character" && Input.GetKeyDown(KeyCode.T)) {

            cameraScript = other.gameObject.GetComponent(typeof(CameraManager)) as CameraManager;
            
            cameraScript.SetCurrentLevel(CameraManager.level.Basement);
            cameraScript.TransformCamera(cameraScript.GetBasementMainCoords(), cameraScript.GetBasementMainOrtho());     

            other.transform.position = newPos;      

        }

    }

}
