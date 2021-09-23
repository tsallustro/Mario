// Maxwell Ortwig

namespace Game1
{
    /*
     * This class takes updates from the controllers and is the interface between the controllers commands and the sprites responses without having to pass all sprites to each controller.
     * 
     * A big drawback to this method is that each sprite has to check this every update() and it only works for a limited application, however seeing how there's 
     *      only 4 sprites and they dont need to do much, it's an acceptable method.
     * The pros of this is that each controller only needs to change fields in one object that all sprites have access to.
     * 
     * True corresponds to a command being queued to trigger, false corresponds to no action needed 
     */
    public class ObjectUpdater
    {
        internal bool quitGame { get; set; }
        internal bool fixedSpriteVisibility { get; set; }
        internal bool fixedAnimatedSpriteVisibility { get; set; }
        internal bool movingSpriteVisibility { get; set; }
        internal bool movingAnimatedSpriteVisibility { get; set; }
        public ObjectUpdater()
        {
            quitGame = false;
            fixedSpriteVisibility = false;
            fixedAnimatedSpriteVisibility = false;
            movingSpriteVisibility = false;
            movingAnimatedSpriteVisibility = false;
        }
    }

}
