using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    CameraManager cameraScript;

    private Vector3 kitchenStairs = new Vector3(-4.2f, -2f, 0f);
    private Vector3 basementStairs = new Vector3(-6.5f, -11f, 0f);


    void OnTriggerStay2D(Collider2D other) {

        if (Input.GetKeyDown(KeyCode.T) && other.name == "Character") {

            cameraScript = other.gameObject.GetComponent(typeof(CameraManager)) as CameraManager;

            if (gameObject.tag == "main-basement-stairs" && cameraScript.GetCurrentLevel() == CameraManager.level.Main)
                ChangeLevels(other, "Basement");
            else if (gameObject.tag == "main-basement-stairs" && cameraScript.GetCurrentLevel() == CameraManager.level.Basement)
                ChangeLevels(other, "Main");

        }

    }

    void ChangeLevels(Collider2D other, string newLevel){

        if (newLevel == "Main") {

            cameraScript.SetCurrentLevel(CameraManager.level.Main);
            cameraScript.TransformCamera(cameraScript.GetKitchenCoords(), cameraScript.GetKitchenOrtho());  

            other.transform.position = kitchenStairs; 

        } else if (newLevel == "Basement") {

            cameraScript.SetCurrentLevel(CameraManager.level.Basement);
            cameraScript.TransformCamera(cameraScript.GetBasementMainCoords(), cameraScript.GetBasementMainOrtho());  

            other.transform.position = basementStairs;    

        }

    }

}
