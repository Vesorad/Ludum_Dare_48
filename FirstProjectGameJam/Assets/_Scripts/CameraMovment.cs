using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    [SerializeField] private Transform objectToMove; 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private int speed = 5;

    [Header("Wartość")] 
    [SerializeField] private int xMax;
    [SerializeField] private int xMin;
    
     private bool _isCamMoving;
    [SerializeField] private int _screenWidth=1000;

    private void Update()
     {
         MoveCam();
     }

     void MoveCam()
     {
         Vector3 camPos = mainCamera.transform.position;
         if (Input.mousePosition.y > _screenWidth - 30 && camPos.y <=xMax )
         {
             _isCamMoving = true;
             camPos.y += speed * Time.deltaTime;
             mainCamera.transform.position = camPos;
             objectToMove.position = new Vector3(objectToMove.position.x,camPos.y-7f, objectToMove.position.z);
         }
         else if (Input.mousePosition.y < 30 && camPos.y >=xMin)
         {
             _isCamMoving = true;
             camPos.y -= speed*Time.deltaTime;
             mainCamera.transform.position = camPos ;
             objectToMove.position = new Vector3(objectToMove.position.x,camPos.y-7f, objectToMove.position.z);
         }
         else
         {
             _isCamMoving = false;
         }
     }

}
