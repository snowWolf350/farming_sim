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
    public event EventHandler OnPlant3Equipped;
    public event EventHandler OnPlant4Equipped;
    public event EventHandler OnPlant5Equipped;
    public event EventHandler OnTool1Equipped;
    public event EventHandler OnTool2Equipped;

    //camera
    Vector2 mouseInputVector;
    float mouseSensitivity = 25;

    bool mouseIsLocked;


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
        playerInput.player.plant3.performed += Plant3_performed;
        playerInput.player.plant4.performed += Plant4_performed;
        playerInput.player.plant5.performed += Plant5_performed;
        playerInput.player.tool1.performed += Tool1_performed;
        playerInput.player.tool2.performed += Tool2_performed;
        playerInput.player.shop.performed += Shop_performed;
        Cursor.lockState = CursorLockMode.Locked;
        mouseIsLocked = true;
    }


    private void OnEnable()
    {
        playerInput.player.move.Enable();
        playerInput.player.jump.Enable();
        playerInput.player.interact.Enable();
        playerInput.player.mouse.Enable();
        playerInput.player.plant1.Enable();
        playerInput.player.plant2.Enable();
        playerInput.player.plant3.Enable();
        playerInput.player.plant4.Enable();
        playerInput.player.plant5.Enable();
        playerInput.player.tool1.Enable();
        playerInput.player.tool2.Enable();
        playerInput.player.shop.Enable();
    }


    private void Shop_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        mouseIsLocked = !mouseIsLocked;
        if (mouseIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            ShopManager.Instance.Hide();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            ShopManager.Instance.Show();
        }
    }
    private void Tool2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnTool2Equipped?.Invoke(this, EventArgs.Empty);
    }

    private void Tool1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnTool1Equipped?.Invoke(this, EventArgs.Empty);
    }
    private void Plant5_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlant5Equipped?.Invoke(this, EventArgs.Empty);
    }
    private void Plant4_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlant4Equipped?.Invoke(this, EventArgs.Empty);
    }

    private void Plant3_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlant3Equipped.Invoke(this, EventArgs.Empty);
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
        if (mouseIsLocked)
        {
            mouseInputVector = playerInput.player.mouse.ReadValue<Vector2>();
            return new Vector2(mouseInputVector.x * mouseSensitivity * Time.deltaTime, mouseInputVector.y * mouseSensitivity * Time.deltaTime);
        }
        return Vector2.zero;
    }
}
