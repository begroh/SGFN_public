using InControl;
using System;
using UnityEngine;

namespace PlayerControl
{
    /*
     * A PlayerInput that allows for moving with a Controller (XBox)
     */
    public class ControllerInput : PlayerInput
    {
        private InputDevice device;
        private Vector2 lastVec;
        private bool holdToShoot = false;

        /*
         * Construct a new ControllerInput to find a joystick for player 'playerNumber'
         */
        public ControllerInput(int playerNumber)
        {
            try
            {
                this.device = InputManager.Devices[playerNumber - 1];
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.LogWarning("No joystick was found for player " + playerNumber);
            }
            catch (NullReferenceException)
            {
                Debug.LogWarning("No InControl InputManager, setup a InControlManager in this scene");
            }
        }

        public void SetInputOnHold(bool hold)
        {
            holdToShoot = hold;
        }

        public void DetectInput(Player player)
        {
            Vector2 move = MoveDirection();
            player.HandleMoveDirection(move);
            Vector2 aim = AimDirection();
            player.HandleAimDirection(aim);

			if (RightBump() || RightTrigger())
            {
                player.HandleShoot();
            }

			player.HandleTapBump(LeftBump() || LeftTrigger());
            //player.HandleRightBump(RightBump());
        }

        /*
         * Detect player aim direction from the right stick
         */
        private Vector2 AimDirection()
        {
            if  (device != null)
            {
                if (device.RightStickX == 0 && device.RightStickY == 0)
                {
                    return lastVec;
                }
                else
                {
                    lastVec = new Vector2(device.RightStickX, device.RightStickY);
                    return lastVec;
                }
            }
            else
            {
                return Vector2.zero;
            }
        }

        /*
         * Detect player movement direction from the left stick
         */
        private Vector2 MoveDirection()
        {
            if (device == null)
            {
                return Vector2.zero;
            }
            else
            {
                return new Vector2(device.LeftStickX, device.LeftStickY);
            }
        }

        /*
         * Detect player shooting from right trigger
         */
        private bool LeftTrigger()
        {
			return device.LeftTrigger.WasPressed;
        }

		private bool RightTrigger()
		{
			return device.RightTrigger.WasPressed;
		}

        private bool LeftBump()
        {
            return device.LeftBumper.WasPressed;
        }

        private bool RightBump()
        {
            return device.RightBumper.WasPressed;
        }
    }
}
