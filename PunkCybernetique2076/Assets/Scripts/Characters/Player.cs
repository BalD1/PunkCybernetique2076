using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntities
{
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CharacterController controller;

    #region camera variables
    [SerializeField] private float mouseSensitivity;
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;
    #endregion

    private float xMovement;
    private float zMovement;
    private Vector3 move;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        AlterateStat(StatsObject.stats.HP, 20, 20);
        AlterateStat(StatsObject.stats.attack, 2, 2);
        AlterateStat(StatsObject.stats.speed, 10, 10);
    }

    void Update()
    {
        CameraMovements();
        PlayerMovements();
    }

    private void CameraMovements()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        characterTransform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerMovements()
    {
        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");

        move = transform.right * xMovement + transform.forward * zMovement;

        controller.Move(move * speed.Current * Time.deltaTime);
    }




}
