using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Subject
{
    // movement variables
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _runSpeed = 5f;
    [SerializeField] float _jumpForce = 16f;
    [SerializeField] int _attackDamage = 80;
    float _rotationFactorPerFrame = 15.0f;
    private float _verticalVelocity;
    Vector3 _cameraRelativeMovement;
    bool _isJumpPressed;
    bool _isRunPressed;
    bool _isWalkPressed;

    // component variables
    private CharacterController _cc;
    private Animator _anim;

    // attack variables
    bool _isAttacking = false;
    bool _isAttackComplete = false;
    bool _isAttackSphereEnabled = false;
    [SerializeField] LayerMask _attackableLayer;
    public bool IsAttackSphereEnabled { get { return _isAttackSphereEnabled; } set { _isAttackSphereEnabled = value; } }
    public int SetIsAttackComplete { set { _isAttackComplete = value > 0; } }
    public int SetIsAttackSphereEnabled { set { _isAttackSphereEnabled = value > 0; } }
    [SerializeField] AudioClip _sliceAudio;

    // damage and health variables
    private Renderer[] _renderers;
    private Coroutine _blinkCoroutine;
    [SerializeField] float _blinkDuration = 0.2f;
    [SerializeField] int _blinkCount = 5;
    bool _isAttackable = true;
    int _health = 3;

    AudioSource _playerAudioPlayer;
    [SerializeField] AudioClip[] _gruntAudio;
    [SerializeField] AudioClip _oofAudio;



    void Start()
    {
        _playerAudioPlayer = GetComponent<AudioSource>();
        _renderers = GetComponentsInChildren<Renderer>();
        _cc = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();

        // Notify the player has spawned
        NotifyObservers(PlayerActions.Encounter);
    }

    void Update()
    {
        Vector3 currentMovement = SetInitialMovement();
        HandleJump();
        HandleAttack();


        currentMovement.y = _verticalVelocity;
        HandleRotation(_cameraRelativeMovement);
        _cameraRelativeMovement = ConvertToCameraSpace(currentMovement);
        _cc.Move(_cameraRelativeMovement * Time.deltaTime);
    }

    Vector3 SetInitialMovement() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);

        _isRunPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire3");
        _isWalkPressed = moveDirection.magnitude != 0;
        _isJumpPressed = Input.GetButtonDown("Jump");


        if (_isRunPressed)
        {
            moveDirection *= _runSpeed;
            _anim.SetBool("isRunning", true);
            _anim.SetBool("isWalking", true);
        }
        else if (_isWalkPressed)
        {
            moveDirection *= _moveSpeed;
            _anim.SetBool("isRunning", false);
            _anim.SetBool("isWalking", true);
        }
        else
        {
            _anim.SetBool("isRunning", false);
            _anim.SetBool("isWalking", false);
        }

        return moveDirection;
    }

    void HandleJump()
    {
        // check if on the ground
        if (_cc.isGrounded)
        {
            _anim.SetBool("isJumping", false);
            _anim.SetBool("isGrounded", true);
            _verticalVelocity = -0.5f;

            // perform jump if jump button pressed
            if (_isJumpPressed)
            {
                _playerAudioPlayer.PlayOneShot(_gruntAudio[UnityEngine.Random.Range(0, _gruntAudio.Length)]);
                // Notify Observers that a jump has been performed
                NotifyObservers(PlayerActions.Jump);

                _anim.SetBool("isJumping", true);
                _anim.SetBool("isGrounded", false);
                _verticalVelocity = _jumpForce;
            }
        }
        else
        {
            // apply extra gravity when jump
            _verticalVelocity += Physics.gravity.y * 5f * Time.deltaTime;
        }
    }

    private void HandleAttack()
    {
        _isAttacking = _anim.GetBool("isAttacking");
        bool attackButtonPressed = Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1");

        if (attackButtonPressed && !_isAttacking)
        {
            _anim.SetBool("isAttacking", true);
            _isAttackComplete = false;
            _playerAudioPlayer.PlayOneShot(_gruntAudio[UnityEngine.Random.Range(0, _gruntAudio.Length)]);
        }

        if (_isAttackComplete)
        {
            _anim.SetBool("isAttacking", false);
        }

        if (IsAttackSphereEnabled)
        {
            Vector3 attackPosition = transform.position + transform.forward * 1.5f;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f, _attackableLayer);
            bool isSlicePlayed = false;
            foreach (Collider currentCollider in colliders)
            {
                Slime currentEnemy = currentCollider.GetComponent<Slime>();

                if (currentEnemy.IsAttackable)
                {
                    // Notify that the attack has hit the enemy
                    NotifyObservers(PlayerActions.AttackHit);

                    currentEnemy.TakeDamage(_attackDamage, transform.forward);

                    if (!isSlicePlayed)
                    {
                        _playerAudioPlayer.PlayOneShot(_sliceAudio);
                        isSlicePlayed = true;
                    }
                }
            }
            isSlicePlayed = false;
        }
    }

    void HandleRotation(Vector3 movementInput)
    {
        if (movementInput.x == 0 && movementInput.z == 0)
        {
            return;
        }
        Vector3 positionToLookAt;
        // the change in position our character should point to
        positionToLookAt.x = movementInput.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = movementInput.z;
        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (movementInput.magnitude != 0)
        {
            // creates a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            // rotate the character to face the positionToLookAt            
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }
    }

    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        // store the Y value of the original vector to rotate 
        float currentYValue = vectorToRotate.y;

        // get the forward and right directional vectors of the camera
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // remove the Y values to ignore upward/downward camera angles
        cameraForward.y = 0;
        cameraRight.y = 0;

        // re-normalize both vectors so they each have a magnitude of 1
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        // rotate the X and Z VectorToRotate values to camera space
        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        // the sum of both products is the Vector3 in camera space and set Y value
        Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.CompareTag("Enemy")) {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (!_isAttackable)
        {
            return;
        }

        _isAttackable = false;
        _health -= 1;
        _playerAudioPlayer.PlayOneShot(_oofAudio);



        if (_health <= 0)
        {
            // Notify that the player has died
            NotifyObservers(PlayerActions.Die);

            Die();
        }
        else
        {
            // Notify that player has been damaged
            NotifyObservers(PlayerActions.Hurt);

            _blinkCoroutine = StartCoroutine(Blink());
        }
    }

    void Die()
    {
        _blinkCoroutine = StartCoroutine(Blink(true));
    }

    IEnumerator Blink(bool die = false)
    {
        for (int i = 0; i < _blinkCount; i++)
        {
            for (int j = 0; j < _renderers.Length; j++)
            {
                _renderers[j].enabled = false;
            }
            yield return new WaitForSeconds(_blinkDuration);
            for (int k = 0; k < _renderers.Length; k++)
            {
                _renderers[k].enabled = true;
            }
            yield return new WaitForSeconds(_blinkDuration);
        }

        if (die)
        {
            Destroy(gameObject);
        }
        else
        {
            _isAttackable = true;
        }
    }
}
