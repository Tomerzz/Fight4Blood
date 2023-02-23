using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator anim;
    [SerializeField] CharacterMovements charMove;
    [SerializeField] CharacterAttack charAttack;
    [SerializeField] GameManager gameManager;
    [SerializeField] ScreenFreezer screenFreezer;
    [SerializeField] MeshTrail meshTrail;
    [SerializeField] GameObject root;

    public float health = 100.0f;
    public bool canMove = true;
    public bool canAttack = true;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isGroundedForJump;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform groundCheckForJump;
    [SerializeField] Collider coll;
    float collHeight;
    float timeToRun = 0;
    float lastMoveSpeed;
    bool isRunning = false;
    Vector2 movementInput = Vector2.zero;
    float forwardValue = 1;

    [SerializeField] bool isGuarded;
    public bool isKnockedOut = false;

    [SerializeField] Slider sliderTemp;

    [SerializeField] AudioSource punchSound;
    [SerializeField] AudioSource superPunchSound;
    [SerializeField] AudioSource windSwoosh;
    [SerializeField] AudioSource guardSound;

    public List<GameObject> otherPlayer;
    [SerializeField] float punchForce = 25.0f;
    [Range(0,3)]
    [SerializeField] int dashCount;

    [SerializeField] TextMeshProUGUI nameText;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        screenFreezer = gameManager.gameObject.GetComponent<ScreenFreezer>();

        // GET COMPONENT OF CHARACTER
        /*coll = GetComponentInChildren<Collider>();
        meshTrail = GetComponentInChildren<MeshTrail>();
        lastMoveSpeed = charMove.moveSpeed;*/
    }

    private void FixedUpdate()
    {
        // CHECK IS ON GROUND
        isGrounded = Physics.Raycast(new Ray(groundCheck.position, -Vector3.up), 0.1f);
        if (anim != null) anim.SetBool("Grounded", isGrounded);

        // BETTER FALL
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * charMove.fallMultiplyer * Time.fixedDeltaTime;
        }
    }

    void Update()
    {
        /*if (anim.GetBool("IsPropelled"))
        {
            Debug.Log("Enter");
            root.transform.forward = -rb.velocity; 
            Debug.Log("Velocity : " + rb.velocity + "; root : " + root.transform.forward);

            if (root.transform.forward == new Vector3(0f,1f,0f))
            {
                root.transform.forward = new Vector3(-1f, 0f, 0f);
                anim.SetBool("IsPropelled", false);
            }
        }*/

        nameText.text = this.name;

        ForwardLookingUpdate();

        if (canMove) Movement();

        if (dashCount < 3)
        {
            RecoveredDash();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        bool jumped = context.performed;
        if (jumped && isGrounded && canMove) Jump();
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        bool dashed = context.performed;
        if (dashed && canMove && movementInput.x != 0 && dashCount > 0) Dash(new Vector3(movementInput.x, 0, 0));
    }
    public void OnLightAttack(InputAction.CallbackContext context)
    {
        bool attacked = context.performed;
        if (attacked && canAttack) anim.SetTrigger("Punch");
    }
    public void OnKick(InputAction.CallbackContext context)
    {
        bool kicked = context.performed;
        if (kicked && canAttack) anim.SetTrigger("Kick");
    }
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        bool attacked = context.performed;
        if (attacked && canAttack) anim.SetTrigger("HeavyPunch");
    }
    public void OnSpecialAttack(InputAction.CallbackContext context)
    {
        bool specialAttacked = context.action.triggered;
        if (specialAttacked && canAttack && isGrounded)
        {
            Camera cam = Camera.main;
            cam.gameObject.SetActive(false);

            charAttack.SpecialAttackCam.gameObject.SetActive(true);
            anim.SetTrigger("SpecialAttack");
        }
    }
    public void OnCrounch(InputAction.CallbackContext context)
    {
        float crounched = context.ReadValue<float>();
        if (crounched > 0 && canMove) Crouch();
        else
        {
            anim.SetBool("isCrouching", false);
            //coll.height = collHeight;
        }
    }

    // ALL PLAYER MOVEMENTS
    public void Movement()
    {
        // MOVE
        float horizontalDirection = movementInput.x;
        anim.SetFloat("Move", horizontalDirection * forwardValue);
        Vector3 moveDirection = transform.right * horizontalDirection;
        transform.Translate(-moveDirection * charMove.moveSpeed * Time.deltaTime);

        // ANIMATION MOVE
        if (horizontalDirection > 0 || horizontalDirection < 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        // GUARD
        if (horizontalDirection < 0) isGuarded = true;
        else isGuarded = false;
    }

    public void Jump()
    {
        windSwoosh.pitch = Random.Range(0.8f, 1.2f);
        windSwoosh.Play();
        anim.SetTrigger("Jump");

        rb.AddForce(transform.up * charMove.jumpForce);
    }

    public void Dash(Vector3 direction)
    {
        windSwoosh.pitch = Random.Range(0.8f, 1.2f);
        windSwoosh.Play();

        dashCount -= 1;

        rb.AddForce(direction * charMove.dashForce);

        meshTrail.isTrailActive = true;
    }

    void Crouch()
    {
        anim.SetBool("isCrouching", true);
    }

    public void Attacking(PlayerBehaviour enemy, float damage)
    {
        windSwoosh.pitch = Random.Range(0.4f, 0.6f);
        windSwoosh.Play();

        if (!enemy.isGuarded)
        {
            // NORMAL HIT
            if (!charAttack.isSuperHit)
            {
                GameObject fx = Instantiate(charAttack.vfxHit.gameObject, charAttack.fxTransform.transform);
                fx.transform.parent = null;
                Destroy(fx, 1f);

                //enemy.rb.AddForce(-enemy.transform.forward * punchForce, ForceMode.Impulse);
                //rb.AddForce(transform.forward * punchForce, ForceMode.Impulse);

                punchSound.pitch = Random.Range(0.8f, 1.2f);
                punchSound.Play();
                enemy.TakeDamage(charAttack.damageHit);
            }
            // SUPER HIT
            else
            {
                GameObject fx = Instantiate(charAttack.vfxSuperHit.gameObject, charAttack.fxTransform.transform);
                fx.transform.parent = null;
                Destroy(fx, 1f);

                enemy.rb.AddForce(charAttack.velocityHit * punchForce * 5f, ForceMode.Impulse);
                //enemy.anim.SetBool("IsPropelled", true);

                superPunchSound.pitch = Random.Range(0.8f, 1.2f);
                superPunchSound.Play();
                enemy.TakeDamage(charAttack.damageSuperHit);

                screenFreezer.Freeze();
            }
            // RESET SUPER HIT
            charAttack.isSuperHit = false;
        }
        else
        {
            enemy.Guard(charAttack.fxTransform.transform.position);
        }
    }

    public void SpecialAttack()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject gm in enemys)
        {
            if (gm != this.gameObject &&
                gm.TryGetComponent<PlayerBehaviour>(out PlayerBehaviour enemy))
            {
                enemy.TakeDamage(charAttack.damageSpecialAttack);
            }
        }
    }

    public void Guard(Vector3 point)
    {
        guardSound.Play();

        charAttack.vfxGuard.transform.position = point;
        charAttack.vfxGuard.Play();

        //anime guard
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0.0f)
        {
            anim.SetTrigger("KnockOut");
            canMove = false;
            canAttack = false;

            rb.isKinematic = true;
            isKnockedOut = true;
            coll.enabled = false;

            gameManager.UpdatePlayersKnockedOut();
        }
        else
        {
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Propelled")
            {
                anim.SetTrigger("TakeDamage");
            }
        }
    }

    public void UpdateOtherPlayers(List<GameObject> allPlayers)
    {
        otherPlayer.Clear();

        foreach (GameObject gm in allPlayers)
        {
            if (gm != this.gameObject)
            {
                otherPlayer.Add(gm);
            }
        }
    }

    public void ForwardLookingUpdate()
    {
        if (otherPlayer.Count > 0)
        {
            if (otherPlayer[0].transform.position.x > this.transform.position.x)
            {
                this.transform.rotation = Quaternion.Euler(0, 90, 0);
                this.transform.localScale = new Vector3(1, 1, 1);

                forwardValue = 1;
            }
            else if (otherPlayer[0].transform.position.x < this.transform.position.x)
            {
                this.transform.rotation = Quaternion.Euler(0, -90, 0);
                this.transform.localScale = new Vector3(-1, 1, 1);

                forwardValue = -1;
            }
        }
    }

    float recoveryActualTime = 5f;
    void RecoveredDash()
    {
        if (recoveryActualTime > 0f)
        {
            recoveryActualTime -= Time.deltaTime;
        }
        else
        {
            dashCount += 1;
            recoveryActualTime = 5f;
        }
    }
}