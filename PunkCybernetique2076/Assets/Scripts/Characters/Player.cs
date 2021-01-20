using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntities
{
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CharacterController controller;

    #region animation curves

    [SerializeField] private AnimationCurve healthPerLevelCurve;
    [SerializeField] private AnimationCurve attackPerLevelCurve;
    [SerializeField] private AnimationCurve speedPerLevelCurve;
    [SerializeField] private AnimationCurve neededExpPerLevelCurve;

    #endregion

    #region camera variables
    [SerializeField] private float mouseSensitivity;
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;
    #endregion

    #region character movements variables

    private float xMovement;
    private float zMovement;
    private Vector3 move;

    #endregion



    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        this.level.ChangeData(null, 0);
        LevelUp(
                  (int)neededExpPerLevelCurve.Evaluate(level.Value + 1),
                  (int)healthPerLevelCurve.Evaluate(level.Value + 1),
                  (int)attackPerLevelCurve.Evaluate(level.Value + 1),
                  (int)speedPerLevelCurve.Evaluate(level.Value + 1)
         );

        // TEST CODE

        level.DebugLog();
        HP.DebugLog();
        attack.DebugLog();
        speed.DebugLog();

    }

    void Update()
    {
        CameraMovements();
        PlayerMovements();




        // TEST CODE

        if (Input.GetKeyDown(KeyCode.L))
        {
            GainExperience(experience.Max);
            level.DebugLog();
            HP.DebugLog();
            attack.DebugLog();
            speed.DebugLog();
        }
    }

    #region camera + movements

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

        controller.Move(move * speed.Value * Time.deltaTime);
    }

    #endregion

    private void GainExperience(int amount)
    {
        this.experience.ChangeData(null, experience.Value + amount);

        if (experience.Value >= experience.Max)
            LevelUp(
                      (int)neededExpPerLevelCurve.Evaluate(level.Value + 1),
                      (int)healthPerLevelCurve.Evaluate(level.Value + 1),
                      (int)attackPerLevelCurve.Evaluate(level.Value + 1),
                      (int)speedPerLevelCurve.Evaluate(level.Value + 1)
                    );
        return;
    }




}
