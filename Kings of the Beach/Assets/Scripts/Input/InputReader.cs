﻿using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
	// Gameplay
	public event UnityAction<Vector2> moveEvent;

	private GameInput gameInput;

	private void OnEnable()
	{
		if (gameInput == null)
		{
			gameInput = new GameInput();
			gameInput.Gameplay.SetCallbacks(this);
		}

		EnableGameplayInput();
	}

	private void OnDisable()
	{
		DisableAllInput();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		if (moveEvent != null)
		{
			moveEvent.Invoke(context.ReadValue<Vector2>());
		}
	}

	public void EnableGameplayInput()
	{
		gameInput.Gameplay.Enable();
	}

	public void DisableAllInput()
	{
		gameInput.Gameplay.Disable();
	}
}