using UnityEngine;

namespace PlayerControl
{
    /*
     * A PlayerInput that allows for moving with arrow keys or wasd and aiming and shooting with the mouse
     */
    public class KeyboardAndMouseInput : PlayerInput
    {
        public void DetectInput(Player player)
        {
            Vector2 move = MoveDirection();
            player.HandleMoveDirection(move);
        }

        /*
         * Detect player movement direction from the keyboard
         * Allows for arrow keys or wasd
         * Allows for diagonal movements
         * Holding all directional keys results in a zero vector
         */
        private Vector2 MoveDirection()
        {
            Vector2 direction = Vector2.zero;

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                direction += Vector2.left;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                direction += Vector2.right;
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                direction += Vector2.up;
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                direction += Vector2.down;
            }

            return direction;
        }
    }
}
