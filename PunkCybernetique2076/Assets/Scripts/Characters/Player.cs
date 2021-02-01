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

    [SerializeField] private bool invincible;

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
    [SerializeField] private float intercatingMouseSensitivity;
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;
    #endregion

    #region character movements variables

    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    private float xMovement;
    private float zMovement;
    private Vector3 move;
    private Vector3 velocity;
    private GameManager.gameState gameState;

    #endregion

    private void Awake()
    {
        CallAwake();
        experience = new StatsObject();
        experience.Data(StatsObject.stats.experience, 0, 0);
        playerSource = playerAudio;
        GameManager.Instance.GameState = GameManager.gameState.InHub;
        if (PlayerPrefs.GetFloat("Level") != 0)
        {
            SetLevel(PlayerPrefs.GetFloat("Level"));
        }
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
        if ((GameManager.Instance.GameState == GameManager.gameState.InGame || GameManager.Instance.GameState == GameManager.gameState.InHub))
            CameraMovements();
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.GameState != GameManager.gameState.GameOver)
            Pause();

        if (gameOver)
            Death();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameState == GameManager.gameState.InGame || GameManager.Instance.GameState == GameManager.gameState.InHub)
            PlayerMovements();
    }

    private void Pause()
    {
        if (GameManager.Instance.GameState != GameManager.gameState.Pause)
            gameState = GameManager.Instance.GameState;
        if (GameManager.Instance.GameState.Equals(GameManager.gameState.InGame) || GameManager.Instance.GameState == GameManager.gameState.InHub)
        {
            GameManager.Instance.GameState = GameManager.gameState.Pause;
        }
        else if (GameManager.Instance.GameState.Equals(GameManager.gameState.Pause))
        {
            Debug.Log(gameState);
            GameManager.Instance.GameState = gameState;
        }
    }

    #region camera + movements

    private void CameraMovements()
    {
        if (!GameManager.Instance.IsInteracting)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        }
        else
        {
            mouseX = Input.GetAxis("Mouse X") * intercatingMouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * intercatingMouseSensitivity * Time.deltaTime;
        }
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        characterTransform.Rotate(Vector3.up * mouseX);

    }

    private void PlayerMovements()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");

        fireBurstForceField.directionX = xMovement * -2;
        fireBurstForceField.directionZ = zMovement * -2;

        move = transform.right * xMovement + transform.forward * zMovement;
        if (move != Vector3.zero)
            characterState = CharacterState.Idle;

        controller.Move(move * speed.Value * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
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
            UIManager.Instance.FillBar(HP.Value / HP.Max, "HP");
            UIManager.Instance.FillBar(experience.Value / experience.Max, "XP");
            UIManager.Instance.UpdateLevel(level.Value.ToString());
        }
        return;
    }

    private void SetLevel(float level)
    {
        LevelUp((int)neededExpPerLevelCurve.Evaluate(level),
                      (int)healthPerLevelCurve.Evaluate(level),
                      (int)attackPerLevelCurve.Evaluate(level),
                      (int)speedPerLevelCurve.Evaluate(level),
                      (int)fireRateLevelCurve.Evaluate(level));
        UIManager.Instance.FillBar(HP.Value / HP.Max, "HP");
        UIManager.Instance.FillBar(experience.Value / experience.Max, "XP");
        UIManager.Instance.UpdateLevel(level.ToString());
    }

    private new void Death()
    {
        if (invincible)
            return;
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
                playerAudio.PlayOneShot(SoundManager.Instance.GetAudioClip(SoundManager.ClipsTags.heal));
                PostProcessManager.Instance.Heal();
                Destroy(other.gameObject);
            }
        }
    }

}
