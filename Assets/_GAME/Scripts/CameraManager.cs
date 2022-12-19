using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public enum level {
        Attic,
        Main,
        Basement
    }

    private level currentLevel;

    [SerializeField] Camera camera;

    private Vector3 livingCoords = new Vector3(13f, 1f, -10f);
    private float livingOrtho = 4.3f;

    private Vector3 kitchenCoords = new Vector3(-0.2f, 0.5f, -10f);
    private float kitchenOrtho = 4.3f;

    private Vector3 hallwayCoords = new Vector3(-10f, 0.11f, -10f);
    private float hallwayOrtho = 3.5f;

    private Vector3 bedroomCoords = new Vector3(-18.5f, 0f, -10f);
    private float bedroomOrtho = 3.5f;

    private Vector3 basementGarageCoords = new Vector3(-17.4f, -8.7f, -10f);
    private float basementGarageOrtho = 4.3f;

    private Vector3 basementMainCoords = new Vector3(-3f, -8.7f, -10f);
    private float basementMainOrtho = 4.3f;

    private Vector3 basementStorageCoords = new Vector3(12f, -8.6f, -10f);
    private float basementStorageOrtho = 4.3f;




    void Start() {

        currentLevel = level.Main;
        TransformCamera(livingCoords, livingOrtho);

    }

    public level GetCurrentLevel() {

        return currentLevel;

    }

    public void SetCurrentLevel(level levelToSet) {

        currentLevel = levelToSet;

    }

    public void SetCurrentLevel(string levelToSet) {

        switch (levelToSet) {

            case "Attic":
                currentLevel = level.Attic;
                break;
            case "Main":
                currentLevel = level.Main;
                break;
            case "Basement":
                currentLevel = level.Basement;
                break;
            default:
                print("Nota valid floor level!");
                break;

        }

    }

    public Vector3 GetKitchenCoords(){

        return kitchenCoords;

    }

    public float GetKitchenOrtho(){

        return kitchenOrtho;

    }

    public Vector3 GetBasementMainCoords() {

        return basementMainCoords;

    }

    public float GetBasementMainOrtho() {

        return basementMainOrtho;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        switch (currentLevel) {

            case level.Main:
                HandleMainCollision(other);
                break;
            case level.Basement:
                HandleBasementCollision(other);
                break;
            default:
                print("testx");
                break;

        }

    }

    private void HandleMainCollision(Collider2D other) {

        if (other.gameObject.tag == "living-room") {
            
            TransformCamera(livingCoords, livingOrtho);

        }
        else if (other.gameObject.tag == "kitchen") {
            
            TransformCamera(kitchenCoords, kitchenOrtho);

        }
        else if (other.gameObject.tag == "hallway") {
            
            TransformCamera(hallwayCoords, hallwayOrtho);

        }
        else if (other.gameObject.tag == "bedroom") {
            
            TransformCamera(bedroomCoords, bedroomOrtho);

        }

    }


    private void HandleBasementCollision(Collider2D other) {

        if (other.gameObject.tag == "basement-garage") {
            
            TransformCamera(basementGarageCoords, basementGarageOrtho);

        }
        else if (other.gameObject.tag == "basement-main") {
            
            TransformCamera(basementMainCoords, basementMainOrtho);

        }
        else if (other.gameObject.tag == "basement-storage") {
            
            TransformCamera(basementStorageCoords, basementStorageOrtho);

        }

    }


    public void TransformCamera(Vector3 newCoords, float orthoSize) {

        camera.transform.position = newCoords;
        camera.orthographicSize = orthoSize;

    }

}
