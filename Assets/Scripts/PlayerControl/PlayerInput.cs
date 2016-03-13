namespace PlayerControl
{
    /*
     * A generic interface to control Player objects
     */
    public interface PlayerInput
    {
        /*
         * Called in the Update() step of the Player object
         */
        void DetectInput(Player player);
    }
}
