using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerAttackSystem playerAttackSystem;
    [Space]
    [SerializeField] private CameraSpring cameraSpring;
    [SerializeField] private CameraLean cameraLean;
    [SerializeField] private bool useCrouchToggle = true;
    private static float slickValue = 3f;
    private bool escaped = false;
    private PlayerInputActions _inputActions;

    public static float SlickValue
    {
        get
        {
            return slickValue;
        }
        set
        {
            slickValue = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slickValue = 3f;
        Cursor.lockState = CursorLockMode.Locked;
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();

        playerCharacter.Initialize();
        playerCamera.Initialize(playerCharacter.GetCameraTarget());

        cameraSpring.Initialize();
        cameraLean.Initialize();
    }

    private void OnDestroy()
    {
        _inputActions.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(slickValue);
        slickValue -= Time.deltaTime * SlickometerData.CurrentSlickDrainRate;
        slickValue = Mathf.Clamp(slickValue, 1f, 3f);
        playerCharacter.isSpeedCapped = slickValue <= 1f;
        playerCharacter.speedBoostMultiplier = slickValue;

        var input = _inputActions.Player;
        var deltaTime = Time.deltaTime;

        // gets camera input, update rotation
        // Handle Escape key to enter "escaped" state
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            escaped = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        // If escaped, allow refocus on left mouse click
        if (escaped && Mouse.current.leftButton.wasPressedThisFrame)
        {
            escaped = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        // Only update camera rotation if game is focused and not escaped
        if (Application.isFocused && !escaped)
        {
            var cameraInput = new CameraInput { Look = input.Look.ReadValue<Vector2>() };
            playerCamera.UpdateRotation(cameraInput);
        }



        //Debug.Log(PlayerCharacter.instance.gameObject.transform.position);
        //get chracterinput and update
        var characterInput = new CharacterInput
        {
            Rotation = playerCamera.transform.rotation,
            Move = input.Move.ReadValue<Vector2>(),
            Sprint = input.Sprint.IsPressed(),
            Jump = input.Jump.WasPressedThisFrame(),
            JumpSustain = input.Jump.IsPressed(),
            Crouch = useCrouchToggle
                ? (input.Crouch.WasPressedThisFrame() ? CrouchInput.Toggle : CrouchInput.None)
                : (input.Crouch.IsPressed() ? CrouchInput.Crouch : CrouchInput.UnCrouch),
            
            Attack = input.Attack.WasPressedThisFrame()
            //Attack = input.Attack.IsPressed()
        };

        playerCharacter.UpdateInput(characterInput);
        playerCharacter.UpdateBody(deltaTime);
        playerAttackSystem.updateInput(characterInput);

        #if UNITY_EDITOR
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("Teleport");
            var ray = new Ray(playerCamera.transform.position,playerCamera.transform.forward);
            if(Physics.Raycast(ray, out var hit))
            {
                playerCharacter.setPosition(hit.point);
            }
        }

        if (input.Restart.WasPressedThisFrame())
        {
            _inputActions.Disable();
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
        #endif
    }

    private void LateUpdate()
    {
        var deltaTime = Time.deltaTime;
        var cameraTarget = playerCharacter.GetCameraTarget();
        var state = playerCharacter.GetState();

        //playerCamera.UpdatePosition(cameraTarget);
        playerCamera.UpdatePosition(cameraTarget, state.Grounded, state.Velocity.y);
        cameraSpring.UpdateSpring(deltaTime, cameraTarget.up);
        cameraLean.UpdateLean(deltaTime ,state.Stance is Stance.Slide ,state.Acceleration , cameraTarget.up);
    }
}
