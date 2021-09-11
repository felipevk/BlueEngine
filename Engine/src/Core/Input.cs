using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace Blue.Core
{
	public class InputGroup
	{
		public struct KeyPressedState
		{
			public Keys key;
			public bool isPressed;
			public KeyState previousState;

			public static implicit operator KeyPressedState( Keys keyIn )
			{
				return new KeyPressedState() { key = keyIn, isPressed = false, previousState = KeyState.Up };
			}
		}

		public struct ButtonPressedState
		{
			public Buttons button;
			public bool isPressed;
			public bool wasDown;

			public static implicit operator ButtonPressedState( Buttons buttonIn )
			{
				return new ButtonPressedState() { button = buttonIn, isPressed = false, wasDown = false };
			}
		}

		public String Name
		{ get; set; }

		public List<KeyPressedState> KeyStates
		{ get; set; } = new List<KeyPressedState>();

		public List<ButtonPressedState> ButtonStates
		{ get; set; } = new List<ButtonPressedState>();

		// TODO add player index

		public InputGroup( String name ) => Name = name;

		public bool IsUp( KeyboardState keyboardState, GamePadState gamePadState )
		{
			foreach ( var keyState in KeyStates )
			{
				if ( keyboardState.IsKeyUp( keyState.key ) )
					return true;
			}

			foreach ( var buttonState in ButtonStates )
			{
				if ( gamePadState.IsButtonUp( buttonState.button ) )
					return true;
			}

			return false;
		}

		public bool IsDown( KeyboardState keyboardState, GamePadState gamePadState )
		{
			foreach ( var keyState in KeyStates )
			{
				if ( keyboardState.IsKeyDown( keyState.key ) )
					return true;
			}

			foreach ( var buttonState in ButtonStates )
			{
				if ( gamePadState.IsButtonDown( buttonState.button ) )
					return true;
			}

			return false;
		}

		public bool IsPressed()
		{
			foreach ( var keyState in KeyStates )
			{
				if ( keyState.isPressed )
					return true;
			}

			foreach ( var buttonState in ButtonStates )
			{
				if ( buttonState.isPressed )
					return true;
			}

			return false;
		}

		public void UpdateIsKeyButtonPressed( KeyboardState keyboardState, GamePadState gamePadState )
		{
			for ( int i = 0; i < KeyStates.Count; i++ )
			{
				KeyPressedState keyPressedState = KeyStates[i];
				keyPressedState.isPressed = keyboardState.IsKeyDown( keyPressedState.key ) && keyPressedState.previousState == KeyState.Up;

				keyPressedState.previousState = keyboardState[keyPressedState.key];
				KeyStates[i] = keyPressedState;
			}

			for ( int i = 0; i < ButtonStates.Count; i++ )
			{
				ButtonPressedState buttonPressedState = ButtonStates[i];
				buttonPressedState.isPressed = gamePadState.IsButtonDown( buttonPressedState.button ) && !buttonPressedState.wasDown;

				buttonPressedState.wasDown = gamePadState.IsButtonDown( buttonPressedState.button );
				ButtonStates[i] = buttonPressedState;
			}
		}
	}
	public static class Input
	{
		public static Dictionary<String, InputGroup> Groups
		{ get; set; } = new Dictionary<string, InputGroup>();

		public static void CreateInputGroup( String name )
		{
			if ( Groups.ContainsKey( name ) )
				return;

			Groups.Add( name, new InputGroup( name ) );
		}

		public static void AddKeyToGroup( String groupName, Keys key )
		{
			if ( !Groups.ContainsKey( groupName ) )
			{
				// TODO assert
				return;
			}

			Groups[groupName].KeyStates.Add( key );
		}

		public static void AddButtonToGroup( String groupName, Buttons button )
		{
			if ( !Groups.ContainsKey( groupName ) )
			{
				// TODO assert
				return;
			}

			Groups[groupName].ButtonStates.Add( button );
		}

		public static bool IsButtonUp( String groupName, int index )
		{
			if ( !Groups.ContainsKey( groupName ) )
			{
				return false;
			}

			return Groups[groupName].IsUp(Keyboard.GetState(), GamePad.GetState( index ));
		}

		public static bool IsButtonDown( String groupName, int index )
		{
			if ( !Groups.ContainsKey( groupName ) )
			{
				return false;
			}

			return Groups[groupName].IsDown( Keyboard.GetState(), GamePad.GetState( index ) );
		}

		public static bool IsButtonPressed( String groupName, int index )
		{
			return Groups[groupName].IsPressed();
		}

		public static void Update()
		{
			KeyboardState keyboardState = Keyboard.GetState();

			// TODO properly assign InputGroups to gamepad Index
			GamePadState gamePadState = GamePad.GetState( 0 );
			foreach ( var groupKvp in Groups )
			{
				groupKvp.Value.UpdateIsKeyButtonPressed( keyboardState, gamePadState );
			}

		}
	}
}
