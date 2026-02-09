using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;

    //player input map
    PlayerInput playerInput;

    //events
    public event EventHandler OnJump;
    public event EventHandler OnInteract;
    public event EventHandler OnPlant1Equipped;
    public event EventHandler OnPlant2Equipped;

    //camera
    Vector2 mouseInputVector;
    float mouseSensitivity = 25;


    private void Awake()
    {
        playerInput = new PlayerInput();
        Instance = this;
    }

    private void Start()
    {
        playerInput.player.jump.performed += Jump_performed;
        playerInput.player.interact.performed += Interact_performed;
        playerInput.player.plant1.performed += Plant1_performed;
        playerInput.player.plant2.performed += Plant2_performed;
        Cursor.lockState = CursorLockMode.Locked;


        playerInput.player.move.Enable();
        playerInput.player.jump.Enable();
        playerInput.player.interact.Enable();
        playerInput.player.mouse.Enable();
        playerInput.player.plant1.Enable();
        playerInput.player.plant2.Enable();

    }

    private void Plant2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlant2Equipped?.Invoke(this,EventArgs.Empty);
    }

    private void Plant1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlant1Equipped?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetInputVectorNormalized()
    {
        Vector2 MoveInputVector = playerInput.player.move.ReadValue<Vector2>();

        return new Vector3(MoveInputVector.x,0,MoveInputVector.y);
    }

    public Vector2 GetMouseDelta()
    {
        mouseInputVector = playerInput.player.mouse.ReadValue<Vector2>();
        return new Vector2(mouseInputVector.x * mouseSensitivity * Time.deltaTime,mouseInputVector.y * mouseSensitivity * Time.deltaTime);
    }
}
