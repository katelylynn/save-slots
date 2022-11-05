using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    string currentRoom = "bedroom";

    [SerializeField] Camera camera;

    private void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "living-room") {
            
            camera.transform.position = new Vector3(13f, 1f, -10f);
            camera.orthographicSize = 4.3f;

        }
        else if (other.gameObject.tag == "kitchen") {
            
            camera.transform.position = new Vector3(-0.2f, 0.5f, -10f);
            camera.orthographicSize = 4.3f;

        }
        else if (other.gameObject.tag == "hallway") {
            
            camera.transform.position = new Vector3(-10f, 0.11f, -10f);
            camera.orthographicSize = 3.5f;

        }
        else if (other.gameObject.tag == "bedroom") {
            
            camera.transform.position = new Vector3(-18.5f, 0f, -10f);
            camera.orthographicSize = 3.5f;
        }

    }

    public void SetRoom(string room)
    {
        camera.transform.position = new Vector3(-20f, 0f, -10f);
        camera.orthographicSize = 3.5f;
    }
}
