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

        public void DetectInput(Player player)
        {
            Vector2 move = MoveDirection();
            player.HandleMoveDirection(move);

            if (Shoot())
            {
                player.HandleShoot();
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
        private bool Shoot()
        {
            if (device.RightTrigger)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
