using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace Blue.Core
{
	public class InputGroup
	{
		public String Name
		{ get; set; }

		public List<Keys> Keys
		{ get; set; } = new List<Keys>();

		public List<Buttons> Buttons
		{ get; set; } = new List<Buttons>();

		// TODO add player index

		public InputGroup( String name ) => Name = name;

		public bool IsUp( KeyboardState keyboardState, GamePadState gamePadState )
		{
			foreach ( var key in Keys )
			{
				if ( keyboardState.IsKeyUp( key ) )
					return true;
			}

			foreach ( var button in Buttons )
			{
				if ( gamePadState.IsButtonUp( button ) )
					return true;
			}

			return false;
		}

		public bool IsDown( KeyboardState keyboardState, GamePadState gamePadState )
		{
			foreach ( var key in Keys )
			{
				if ( keyboardState.IsKeyDown( key ) )
					return true;
			}

			foreach ( var button in Buttons )
			{
				if ( gamePadState.IsButtonDown( button ) )
					return true;
			}

			return false;
		}

		public bool IsPressed()
		{
			// TODO

			return false;
		}
	}
	public class Input
	{
		public Dictionary<String, InputGroup> Groups
		{ get; set; } = new Dictionary<string, InputGroup>();

		public void CreateInputGroup( String name )
		{
			if ( Groups.ContainsKey( name ) )
				return;

			Groups.Add( name, new InputGroup( name ) );
		}

		public void AddKeyToGroup( String groupName, Keys key )
		{
			if ( !Groups.ContainsKey( groupName ) )
			{
				// TODO assert
				return;
			}

			Groups[groupName].Keys.Add( key );
		}

		public void AddButtonToGroup( String groupName, Buttons button )
		{
			if ( !Groups.ContainsKey( groupName ) )
			{
				// TODO assert
				return;
			}

			Groups[groupName].Buttons.Add( button );
		}

		public bool IsButtonUp( String groupName, int index )
		{
			if ( !Groups.ContainsKey( groupName ) )
			{
				return false;
			}

			return Groups[groupName].IsUp(Keyboard.GetState(), GamePad.GetState( index ));
		}

		public bool IsButtonDown( String groupName, int index )
		{
			if ( !Groups.ContainsKey( groupName ) )
			{
				return false;
			}

			return Groups[groupName].IsDown( Keyboard.GetState(), GamePad.GetState( index ) );
		}

		public bool IsButtonPressed( String groupName, int index )
		{
			// TODO cache button press by index and retrieve it

			return Groups[groupName].IsPressed();
		}

		public void Update()
		{
			// TODO manage button press state
		}
	}
}
