using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using States;
using Sprites;
using Factories;

namespace GameObjects
{
    public class RedKoopaTroopa : GameObject, IEnemy
    {
        private readonly int boundaryAdjustment = 4;
        /* 
         * IMPORTANT: When establishing AABB, you must divide sprite texture width by number of sprites
         * on that sheet!
         */
        private readonly int numberOfSpritesOnSheet = 3;
        private IEnemyState redKoopaTroopaState;
        private RedKoopaTroopaSpriteFactory spriteFactory;

        public RedKoopaTroopa(Vector2 position)
            : base(position, new Vector2(0, 0), new Vector2(0, 0))
        {
            spriteFactory = RedKoopaTroopaSpriteFactory.Instance;
            Sprite = spriteFactory.CreateIdleRedKoopaTroopa(position);
            AABB = (new Rectangle((int)position.X + (boundaryAdjustment / 2), (int)position.Y + (boundaryAdjustment / 2),
                (Sprite.texture.Width / numberOfSpritesOnSheet) - boundaryAdjustment, Sprite.texture.Height - boundaryAdjustment));
            redKoopaTroopaState = new IdleRedKoopaTroopaState(this);
        }

        public IEnemyState GetRedKoopaTroopaState()
        {
            return this.redKoopaTroopaState;
        }

        public void SetRedKoopaTroopaState(IEnemyState redKoopaTroopaState)
        {
            this.redKoopaTroopaState = redKoopaTroopaState;
        }

        public override void Halt()
        {
            this.SetXVelocity(0);
            this.SetYVelocity(0);
        }

        public override void Damage()
        {
            redKoopaTroopaState.Stomped();
        }

        //Update all of Goomba's members
        public override void Update(GameTime gameTime)
        {
            Sprite = spriteFactory.GetCurrentSprite(Sprite.location, redKoopaTroopaState);
            Sprite.Update();
        }

        //Draw Goomba
        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, true);
            DrawAABBIfVisible(Color.Red, spriteBatch);
        }

        //Change Goomba state to stomped mode
        public void Stomped()
        {
            redKoopaTroopaState.Stomped();
        }

        //Change Goomba state to moving mode
        public void Move()
        {
            redKoopaTroopaState.Move();
        }

        //Change Goomba state to idle mode
        public void StayIdle()
        {
            redKoopaTroopaState.StayIdle();
        }
       

    }
}
