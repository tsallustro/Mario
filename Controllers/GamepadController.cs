using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class GamepadController : IController
    {
        private GamePadState state;
        private GamePadState oldState;
        private ObjectUpdater objectUpdater;

        // Default constructor
        public GamepadController(ObjectUpdater OU)
        {
            objectUpdater = OU;
        }
        public void Update()
        {
            oldState = state;
            state = GamePad.GetState(PlayerIndex.One);

            if (state.IsButtonDown(Buttons.Start) && oldState.IsButtonUp(Buttons.Start))
            {
                objectUpdater.quitGame = true;
            }
            if (state.IsButtonDown(Buttons.A) && oldState.IsButtonUp(Buttons.A))
            {
                objectUpdater.fixedSpriteVisibility = true;
            }

            if (state.IsButtonDown(Buttons.B) && oldState.IsButtonUp(Buttons.B))
            {
                objectUpdater.fixedAnimatedSpriteVisibility = true;
            }

            if (state.IsButtonDown(Buttons.Y) && oldState.IsButtonUp(Buttons.Y))
            {
                objectUpdater.movingSpriteVisibility = true;
            }

            if (state.IsButtonDown(Buttons.X) && oldState.IsButtonUp(Buttons.X))
            {
                objectUpdater.movingAnimatedSpriteVisibility = true;
            }
        }
    }
}
