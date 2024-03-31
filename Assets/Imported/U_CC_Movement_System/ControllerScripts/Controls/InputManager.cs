using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField]
    PlayerController playerController;
    [SerializeField] private GameEvent inventoryEvent;
    [SerializeField] private GameEvent throwItem;

    PlayerControls controls;
    PlayerControls.LocomotionInputActions locomotionInput;

    Vector2 horizontalInput;
    Vector2 mouseLookInput;

    float running = 0;
    float aiming = 0;

    private void Awake()
    {
        controls = new PlayerControls();
        locomotionInput = controls.LocomotionInput;

        // MOVEMENT INPUT //
        locomotionInput.Movement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        locomotionInput.Run.performed += ctx => running = ctx.ReadValue<float>();

        // MOUSE LOOK INPUT //
        locomotionInput.MouseX.performed += ctx => mouseLookInput.x = ctx.ReadValue<float>();
        locomotionInput.MouseY.performed += ctx => mouseLookInput.y = ctx.ReadValue<float>();

        // AIMING & ACTION INPUT //
        locomotionInput.Aim.performed += ctx => aiming = ctx.ReadValue<float>();

        // // INTERACTION INPUT //
        locomotionInput.Interact.performed += _ => playerController.Interact();

        // locomotionInput.Action.performed += _ => playerController.Action();

        // // INVENTORY //
        locomotionInput.Inventory.performed += _ => inventoryEvent.Raise();

        locomotionInput.Throw.performed += _ => throwItem.Raise();

        // // SLOT SELECTION //
        locomotionInput.SelectSlot1.performed += _ => ControllerReferences.playerInventory.currentlyEquipped.SelectSlot(0);
        locomotionInput.SelectSlot2.performed += _ => ControllerReferences.playerInventory.currentlyEquipped.SelectSlot(1);
        locomotionInput.SelectSlot3.performed += _ => ControllerReferences.playerInventory.currentlyEquipped.SelectSlot(2);;
        locomotionInput.SelectSlot4.performed += _ => ControllerReferences.playerInventory.currentlyEquipped.SelectSlot(3);
        locomotionInput.SelectSlot5.performed += _ => ControllerReferences.playerInventory.currentlyEquipped.SelectSlot(4);
        locomotionInput.SelectSlot6.performed += _ => ControllerReferences.playerInventory.currentlyEquipped.SelectSlot(5);

        locomotionInput.NextItem.performed += _ => ControllerReferences.playerInventory.currentlyEquipped.EquipNextSlot();
        locomotionInput.PreviousItem.performed += _ => ControllerReferences.playerInventory.currentlyEquipped.EquipPreviousSlot();
    }

    private void Update()
    {
        playerController.ReceiveInput(horizontalInput, mouseLookInput, running, aiming);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

}
