~Interaction System~

This needs to be used with Unity's Input System.

Here are the steps to get this working with the InHouse CC_Movement_System Asset:
	1. Open Script "Input Manager"
	2. locomotionInput.Interact.performed += _ => playerController.Interact();
	3. Open Script "PlayerController"
	4. Add this Method at Line 240:
		    public void Interact(){
      		  playerCamera.GetComponent<InteractionRaycast>().TriggerInteract();
		    }
	5. Add both the "InteractionRaycast" and the "CameraLock" Script to the Main Camera

You're set!

Just make sure that each item you would like to interact with includes the IInteractable Interface, and implements "public void Interact()"

