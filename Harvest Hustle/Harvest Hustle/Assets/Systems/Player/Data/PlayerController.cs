using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Object to Control")]
    [SerializeField] private ControllableObject objectToControl;
    private ControllableObject _currentPlayer;
    private CharacterController _controller;
    public CharacterController Controller { get => _controller; }

    [Header("Movement Configurations")]
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private Transform _cameraTransform;
    private Vector3 _InputMovement;
    private Vector3 _cameraForward;
    private Vector3 _playerDesiredMovement;
    private bool _IsRuning;
    public bool canAnim = true;

    // Rotation over time
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;
    private bool _isRotating;
    private float _counter;
    private float _percentageComplete;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        GetControl();
    }

    private void OnEnable()
    {
        GameplayManager.instance.GameStateChanged += ResetAnim;
    }

    private void OnDisable()
    {
        GameplayManager.instance.GameStateChanged -= ResetAnim;
    }

    private void Update()
    {
        if (!GameplayManager.instance.globalConfigs.Player_Active) return;

        //_IsRuning = PlayerInputManager.PlayerInput.World.Run.ReadValue<bool>();
        _IsRuning = Input.GetKey(KeyCode.LeftShift);

        if (_currentPlayer != null)
        {
            DoMovement();
            DoRotation();
        }

        if (canAnim)
        {
            _currentPlayer.anim.SetFloat("Speed", _playerDesiredMovement.magnitude);
            _currentPlayer.anim.SetBool("IsRunning", _IsRuning);
        }
    }

    private void ResetAnim()
    {
        //_currentPlayer.anim.Play("Idle");
    }

    private void DoMovement()
    {
        _isGrounded = _controller.isGrounded;

        if (_isGrounded && _playerVelocity.y < 0) _playerVelocity.y = 0f;

        // Calculate camera forward only once per frame
        _cameraTransform = Camera.main.transform;
        _cameraForward = _cameraTransform.forward;
        _cameraForward.y = 0f;
        _cameraForward.Normalize();

        _InputMovement = PlayerInputManager.PlayerInput.World.Movement.ReadValue<Vector2>();
        _playerDesiredMovement = (_InputMovement.x * _cameraTransform.right + _InputMovement.y * _cameraForward);

        float speed = _IsRuning ? objectToControl.movementConfig.runSpeed : objectToControl.movementConfig.walkSpeed;
        _controller.Move(speed * Time.deltaTime * _playerDesiredMovement);

        _playerVelocity.y += objectToControl.movementConfig.gravity * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    private void StartRotation()
    {
        if (_playerDesiredMovement.magnitude < 0.1f) return;

        _counter = 0;
        _currentRotation = transform.forward;
        _targetRotation = _playerDesiredMovement.normalized;
        _isRotating = true;
    }

    private void DoRotation()
    {
        // Calculate and apply rotation
        if (_playerDesiredMovement != Vector3.zero)
        {
            Vector3 desiredDirection = _playerDesiredMovement;
            desiredDirection.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, objectToControl.movementConfig.timeToRotateInSeconds * Time.deltaTime);
        }
    }

    public void SetObjectToControl(Transform objectToControl)
    {
        ControllableObject toControl = objectToControl.GetComponent<ControllableObject>();
        if (toControl == null)
        {
            throw new System.Exception($"The object {objectToControl.name} cannot be controlled");
        }

        this.objectToControl = toControl;
    }

    [ContextMenu("Get Control")]
    private void GetControl()
    {
        // Leave the current control
        if (_currentPlayer != null)
        {
            _currentPlayer.transform.parent = null;
        }

        // Moves the Player Controller to the Current Object
        transform.parent = objectToControl.transform;
        transform.localPosition = Vector3.zero;
        transform.parent = null;

        // Changes the Object to Control to be child of Player Controller
        objectToControl.transform.parent = transform;

        // Updates who is the currentPlayer
        _currentPlayer = objectToControl;

        // Defines the new values from the controller to match the object size
        _controller.height = _currentPlayer.objectSize.y;
        _controller.radius = _currentPlayer.objectSize.x;
    }
}
