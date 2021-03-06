using System;
using System.Collections.Generic;
using System.Text;
using GameObjects;
using Game1;
using Sound;

namespace States
{
    public interface IMarioPowerState
    {
        //For future use
        void FireFlower();
        void Mushroom();
        void TakeDamage();

        //For sprint 1 proof of concept
        void SmallMario();
        void BigMario();
        void FlameMario();
        void BossPowerUp();
    }

    public class StandardMario : IMarioPowerState
    {
        private Mario mario;

        public StandardMario(Mario mario)
        {
            this.mario = mario;

            //Construct sprite
        }

        public void FireFlower()
        {
            mario.SetPowerState(new FireMario(mario));
        }

        public void Mushroom()
        {
            mario.SetPowerState(new SuperMario(mario));
        }

        public void TakeDamage()
        {
            mario.SetPowerState(new DeadMario(mario));
        }

        public void SmallMario()
        {
            //Do nothing, already Standard Mario
        }

        public void BigMario()
        {
            mario.SetPowerState(new SuperMario(mario));
        }

        public void FlameMario()
        {
            mario.SetPowerState(new FireMario(mario));
        }

        public void BossPowerUp()
        {
            mario.SetPowerState(new BossPowerUpMario(mario));
        }
    }

    public class SuperMario : IMarioPowerState
    {
        private Mario mario;

        public SuperMario(Mario mario)
        {
            this.mario = mario;

            //Construct sprite
        }

        public void FireFlower()
        {
            mario.SetPowerState(new FireMario(mario));
        }

        public void Mushroom()
        {
            //Do nothing, already Super Mario
        }

        public void TakeDamage()
        {
            mario.SetPowerState(new StandardMario(mario));
        }

        public void SmallMario()
        {
            mario.SetPowerState(new StandardMario(mario));
        }

        public void BigMario()
        {
            //Do nothing, already Super Mario
        }

        public void FlameMario()
        {
            mario.SetPowerState(new FireMario(mario));
        }

        public void BossPowerUp()
        {
            mario.SetPowerState(new BossPowerUpMario(mario));
        }
    }

    public class FireMario : IMarioPowerState
    {
        private Mario mario;

        public FireMario(Mario mario)
        {
            this.mario = mario;

            //Construct sprite
        }

        public void FireFlower()
        {
            //Do nothing, already Fire Mario
        }

        public void Mushroom()
        {
            //Do nothing, already Fire Mario
        }

        public void TakeDamage()
        {
            mario.SetPowerState(new SuperMario(mario));
        }

        public void SmallMario()
        {
            mario.SetPowerState(new StandardMario(mario));
        }

        public void BigMario()
        {
            mario.SetPowerState(new SuperMario(mario));
        }

        public void FlameMario()
        {
            //Do nothing, already Fire Mario
        }

        public void BossPowerUp()
        {
            mario.SetPowerState(new BossPowerUpMario(mario));
        }
    }

    public class BossPowerUpMario : IMarioPowerState
    {
        private Mario mario;

        public BossPowerUpMario(Mario mario)
        {
            this.mario = mario;

            //Construct sprite
        }

        public void FireFlower()
        {
            //Do nothing
        }

        public void Mushroom()
        {
            //Do nothing
        }

        public void TakeDamage()
        {
            mario.SetPowerState(new SuperMario(mario));
        }

        public void SmallMario()
        {
            mario.SetPowerState(new StandardMario(mario));
        }

        public void BigMario()
        {
            mario.SetPowerState(new SuperMario(mario));
        }

        public void FlameMario()
        {
            //Do nothing
        }

        public void BossPowerUp()
        {
            //Do nothing, already Boss Powerup
        }
    }

    public class DeadMario : IMarioPowerState
    {
        private Mario mario;

        public DeadMario(Mario mario)
        {
            this.mario = mario;
            mario.DecrementLivesRemaining();
            if(mario.GetLivesRemaining() > 0) SoundManager.Instance.PlaySound(SoundManager.GameSound.DEATH);
            else SoundManager.Instance.PlaySound(SoundManager.GameSound.GAME_OVER);
        }

        public void FireFlower()
        {
            //Do nothing, dead
        }

        public void Mushroom()
        {
            //Do nothing, dead
        }

        public void TakeDamage()
        {
            //Do nothing, dead
        }

        public void SmallMario()
        {
            //Do nothing, dead
        }

        public void BigMario()
        {
            //Do nothing, dead
        }

        public void FlameMario()
        {
            //Do nothing, dead
        }

        public void BossPowerUp()
        {
            //Do nothing, dead
        }
        public void Update()
        {
            
            mario.SetYVelocity((float)100);

        }
    }
}