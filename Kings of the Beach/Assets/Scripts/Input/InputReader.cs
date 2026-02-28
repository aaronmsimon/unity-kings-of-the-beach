using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IBetweenPointsActions, GameInput.IMenuActions
{
	// Gameplay
	public event UnityAction<Vector2> moveEvent;
	public event UnityAction<Vector2> rightStickEvent;
	public event UnityAction testEvent;
	public event UnityAction testCanceledEvent;
	public event UnityAction bumpEvent;
	public event UnityAction bumpAcrossEvent;
	public event UnityAction setEvent;
	public event UnityAction jumpEvent;
	public event UnityAction feintEvent;
	public event UnityAction pauseEvent;

	// Between Points
	public event UnityAction interactEvent;

	// Menu
	public event UnityAction selectEvent;
	public event UnityAction startEvent;
	public event UnityAction selectionUpEvent;
	public event UnityAction selectionDownEvent;
	public event UnityAction selectionLeftEvent;
	public event UnityAction selectionRightEvent;

	private GameInput gameInput;

	private void OnEnable()
	{
		if (gameInput == null)
		{
			gameInput = new GameInput();
			gameInput.Gameplay.SetCallbacks(this);
			gameInput.BetweenPoints.SetCallbacks(this);
			gameInput.Menu.SetCallbacks(this);
		}

		EnableGameplayInput();
	}

	private void OnDisable()
	{
		DisableAllInput();
	}

	// Gameplay

	public void OnMove(InputAction.CallbackContext context)
	{
		if (moveEvent != null)
		{
			moveEvent?.Invoke(context.ReadValue<Vector2>());
		}
	}

	public void OnBump(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			bumpEvent?.Invoke();
	}

	public void OnBumpAcross(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed) {
			bumpAcrossEvent?.Invoke();
		}
	}

	public void OnSet(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			setEvent?.Invoke();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			jumpEvent?.Invoke();
	}

	public void OnFeint(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			feintEvent?.Invoke();
	}

    public void OnRightStick(InputAction.CallbackContext context)
    {
		if (rightStickEvent != null)
		{
			rightStickEvent?.Invoke(context.ReadValue<Vector2>());
		}
    }

	public void OnPause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			pauseEvent?.Invoke();
	}

	public void OnTest(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
			testEvent?.Invoke();
		
		if (context.phase == InputActionPhase.Canceled)
			testCanceledEvent?.Invoke();
	}

	// Between Points

    public void OnInteract(InputAction.CallbackContext context)
    {
		if (context.phase == InputActionPhase.Performed)
			interactEvent?.Invoke();
    }

	// Menu

    public void OnSelect(InputAction.CallbackContext context)
    {
		if (context.phase == InputActionPhase.Performed)
			selectEvent?.Invoke();
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
			startEvent?.Invoke();
    }

    public void OnSelectionUp(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
			selectionUpEvent?.Invoke();
    }

    public void OnSelectionDown(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
			selectionDownEvent?.Invoke();
    }

    public void OnSelectionLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
			selectionLeftEvent?.Invoke();
    }

    public void OnSelectionRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
			selectionRightEvent?.Invoke();
    }

	// Enable/Disable

	public void EnableGameplayInput()
	{
		gameInput.Gameplay.Enable();
		gameInput.BetweenPoints.Disable();
		gameInput.Menu.Disable();
	}

	public void EnableBetweenPointsInput()
	{
		gameInput.Gameplay.Disable();
		gameInput.BetweenPoints.Enable();
		gameInput.Menu.Disable();
	}

	public void EnableMenuInput()
	{
		gameInput.Gameplay.Disable();
		gameInput.BetweenPoints.Disable();
		gameInput.Menu.Enable();
	}

	public void DisableAllInput()
	{
		gameInput.Gameplay.Disable();
		gameInput.BetweenPoints.Disable();
		gameInput.Menu.Disable();
	}
}