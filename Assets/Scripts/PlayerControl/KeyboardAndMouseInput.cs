using UnityEngine;

namespace PlayerControl
{
    /*
     * A PlayerInput that allows for moving with arrow keys or wasd and aiming and shooting with the mouse
     */
    public class KeyboardAndMouseInput : PlayerInput
    {
        private static int activePlayer = 1;
        private bool holdToShoot = false;

        public void DetectInput(Player player)
        {
            DetectPlayerChange();

            if (activePlayer == player.playerNumber)
            {
                Vector2 move = MoveDirection();
                player.HandleMoveDirection(move);
                Vector2 aim = AimDirection(player.transform.position);
                player.HandleAimDirection(aim);

				if (Space())
                {
                    player.HandleShoot();
                }

                player.HandleTapBump(LeftBump());
                player.HandleTapBump(RightBump());
            }
        }

        public void SetInputOnHold(bool hold)
        {
            holdToShoot = hold;
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

        /*
         * Detect mouse location and notify the player
         */
        private Vector2 AimDirection(Vector3 playerPos)
        {
            float mouse_x = Input.mousePosition.x;
            float mouse_y = Input.mousePosition.y;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mouse_x, mouse_y, 0));
            mousePos.x -= playerPos.x;
            mousePos.y -= playerPos.y;
            return (Vector2) mousePos;
        }
			
		private bool Space()
		{
			return Input.GetKeyDown(KeyCode.Space);
		}

        private bool LeftBump()
        {
            return Input.GetKeyDown(KeyCode.LeftShift);
        }

        private bool RightBump()
        {
            return Input.GetKeyDown(KeyCode.RightShift);
        }

        /*
         * If the player presses 1, 2, 3, or 4, change the active player number.
         */
        private void DetectPlayerChange()
        {
            KeyCode[] codes = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

            for (int i = 0; i < codes.Length; ++i)
            {
                if (Input.GetKeyDown(codes[i]))
                {
                    activePlayer = i + 1;
                }
            }
        }
    }
}
