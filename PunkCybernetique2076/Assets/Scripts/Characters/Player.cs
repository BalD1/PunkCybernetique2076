using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntities
{
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject gun;
    [SerializeField] private Animator deathAnimation;
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private ParticleSystemForceField fireBurstForceField;

    [SerializeField] private float healCollectiblesAmount = 20f;

    #region animation curves

    [SerializeField] private AnimationCurve healthPerLevelCurve;
    [SerializeField] private AnimationCurve attackPerLevelCurve;
    [SerializeField] private AnimationCurve speedPerLevelCurve;
    [SerializeField] private AnimationCurve neededExpPerLevelCurve;
    [SerializeField] private AnimationCurve fireRateLevelCurve;

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

    private void Awake()
    {
        CallAwake();
        experience = new StatsObject();
        experience.Data(StatsObject.stats.experience, 0, 0);
        playerSource = playerAudio;
    }

    private void Start()
    {
        Initialization();

    }

    private void Initialization()
    {
        this.level.ChangeData(null, 0);
        LevelUp(
                  (float)neededExpPerLevelCurve.Evaluate(level.Value + 1),
                  (float)healthPerLevelCurve.Evaluate(level.Value + 1),
                  (float)attackPerLevelCurve.Evaluate(level.Value + 1),
                  (float)speedPerLevelCurve.Evaluate(level.Value + 1),
                  (float)fireRateLevelCurve.Evaluate(level.Value + 1)
         );
        UIManager.Instance.UpdateLevel("1");
        UIManager.Instance.FillBar(0, "XP");
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameManager.gameState.InGame)
            CameraMovements();
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.GameState != GameManager.gameState.GameOver)
            Pause();

        if (gameOver)
            Death();

    }
    
    private void FixedUpdate()
    {
        if (GameManager.Instance.GameState == GameManager.gameState.InGame)
            PlayerMovements();
    }

    private void Pause()
    {
        if (GameManager.Instance.GameState.Equals(GameManager.gameState.InGame))
            GameManager.Instance.GameState = GameManager.gameState.Pause;
        else if (GameManager.Instance.GameState.Equals(GameManager.gameState.Pause))
            GameManager.Instance.GameState = GameManager.gameState.InGame;
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

        fireBurstForceField.directionX = xMovement * -2;
        fireBurstForceField.directionZ = zMovement * -2;

        move = transform.right * xMovement + transform.forward * zMovement;
        if (move != Vector3.zero)
            characterState = CharacterState.Idle;

        controller.Move(move * speed.Value * Time.deltaTime);
    }

    #endregion

    public void GainExperience(float amount)
    {
        this.experience.ChangeData(null, experience.Value + amount);
        UIManager.Instance.FillBar(experience.Value / experience.Max, "XP");
        if (experience.Value >= experience.Max)
        {
            LevelUp(
                      (int)neededExpPerLevelCurve.Evaluate(level.Value + 1),
                      (int)healthPerLevelCurve.Evaluate(level.Value + 1),
                      (int)attackPerLevelCurve.Evaluate(level.Value + 1),
                      (int)speedPerLevelCurve.Evaluate(level.Value + 1),
                      (int)fireRateLevelCurve.Evaluate(level.Value + 1)
                    );
            SoundManager.Instance.Play2D(SoundManager.ClipsTags.lvlUp);
            GameManager.Instance.GameState = GameManager.gameState.Levelup;
            UIManager.Instance.FillBar(HP.Value / HP.Max, "HP");
            UIManager.Instance.FillBar(experience.Value / experience.Max, "XP");
            UIManager.Instance.UpdateLevel(level.Value.ToString());
        }
        return;
    }

    private new void Death()
    {
        if (GameManager.Instance.GameState == GameManager.gameState.GameOver)  // if Gamestate = GameOver, it means that this has already been called
            return;

        GameManager.Instance.GameState = GameManager.gameState.GameOver;
        deathAnimation.SetBool("GameOver", true);
        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1);
        PostProcessManager.Instance.ScreenFadeOut();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("PickableObject"))
        {
            if (other.gameObject.name.Contains("Heal") && this.HP.Value < this.HP.Max)
            {
                Heal(this.HP.Max - (this.HP.Max * ((100 - healCollectiblesAmount) / 100)));
                PostProcessManager.Instance.Heal();
                Destroy(other.gameObject);
            }
        }
    }

}
