using Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using States;
using System.Collections.Generic;
using Game1;


namespace GameObjects
{
    public class Mario2 : GameObject, IAvatar
    {
        private ISprite sprite;
        private IMarioPowerState powerState;
        private IMarioActionState actionState;
        private IMarioActionState2 actionState2;
        private MarioSpriteFactory spriteFactory;
        public Vector2 Velocity { get; set; }
        public Vector2 Location { get; set; }
        private Texture2D Texture { get; set; }
        private enum actionStates { Idle, Crouching, Jumping, Falling, Running };
        private enum powerStates { Standard, Super, Fire, Dead };
        GraphicsDeviceManager Graphics { get; set; }
        List<IGameObject> objects;

        public Mario2(Texture2D _texture, List<IGameObject> objs) :
            base(_texture)
        {
            spriteFactory = MarioSpriteFactory.Instance;
            sprite = spriteFactory.CreateStandardIdleMario(Location);
            //powerState = new StandardMario(this);
            //actionState = new IdleState(this, false);
            Velocity = new Vector2(0, 0);
            Location = new Vector2(0, 0);
            Texture = _texture;
            objects = objs;
            this.Add(objects);
        } 

        public IMarioPowerState GetPowerState()
        {
            return this.powerState;
        }

        public void SetPowerState(IMarioPowerState powerState)
        {
            this.powerState = powerState;
            sprite = spriteFactory.GetCurrentSprite(Location, actionState, powerState);
        }

        public void SetActionState(IMarioActionState actionState)
        {
            this.actionState = actionState;
        }
        //Temporary for try out copy versions
        public void SetActionState2(IMarioActionState2 actionState2)
        {
            this.actionState2 = actionState2;
        }

        //Update all of Mario's members
        public override void Update(GameTime GameTime)
        {
            //Check Mario collision every objects
            foreach (var obj in objects)
            {
                if (obj != this)
                {
                    this.CheckCollsion(obj);
                }
            }
            //Update Mario location
            this.Location += Velocity;

            sprite.Update();
        }

        //Draw Mario
        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, actionState.GetDirection());
        }
        public void MoveLeft(int pressType)
        {
            //When holding, Mario speed increase
            if (pressType == 2)
            {
                actionState2.FaceLeftTransition();
                if (this.Velocity.X == 0)
                {
                    actionState2.WalkingTransition();
                }
                else if (this.Velocity.X == -4)
                {
                    actionState2.RunningTransition();
                }
            }
            else if (pressType == 3) //When released, Mario stops moving
            {
                if (this.Velocity.X == -4)
                {
                    actionState2.RunningDiscontinueTransition();
                }
                else if (this.Velocity.X == 0)
                {
                    actionState2.WalkingDiscontinueTransition();
                }
            }
            else
            {
                actionState2.FaceLeftTransition();
            }
        }
        public void MoveRight(int pressType)
        {
            //When holding, Mario speed increase
            if (pressType == 2)
            {
                actionState2.FaceRightTransition();
                if (this.Velocity.X == 0)
                {
                    actionState2.WalkingTransition();
                }
                else if (this.Velocity.X == 4)
                {
                    actionState2.RunningTransition();
                }
            } else if (pressType == 3) //When released, Mario stops moving
            {
                if (this.Velocity.X == 4)
                {
                    actionState2.RunningDiscontinueTransition();
                }
                else if (this.Velocity.X == 0)
                {
                    actionState2.WalkingDiscontinueTransition();
                }
            } else
            {
                actionState2.FaceRightTransition();
            }
        }

        public void Up(int pressType)
        {
            //When holding, Mario speed increase
            if (pressType == 2 || pressType == 1)
            {
                actionState2.JumpingTransition();
            }
            else if (pressType == 3) //When released, Mario stops moving
            {
                actionState2.JumpingDiscontinueTransition();
            }
        }
        public void Down(int pressType)
        {
            //When holding, Mario speed increase
            if (pressType == 2 || pressType == 1)
            {
                actionState2.CrouchingTransition();
            }
            else if (pressType == 3) //When released, Mario stops moving
            {
                actionState2.CrouchingDiscontinueTransition();
            }
        }
        public override void Animate()
        {
            //TO DO
        }

        public override void TakeDamage()
        {
            //TO DO
        }
    }
}
