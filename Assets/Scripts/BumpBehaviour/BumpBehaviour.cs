public interface BumpBehaviour
{
    /*
     * Called every frame, bumping is true if the player is holding the bump button
     */
    void Update(Player player, bool bumping);
}
