using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
namespace Sound
{
    
    class SoundManager
    {
        private static SoundManager _instance;
        public static SoundManager Instance 
        {
            get { if (_instance == null)
                {
                    _instance = new SoundManager();
                }

                return _instance;
             }
        }
        public enum GameSound
        {
            STANDARD_JUMP, SUPER_JUMP, STOMP, DEATH, COIN, POWER_UP_APPEAR, POWER_UP_COLLECTED, ONE_UP_COLLECTED, BUMP, BRICK_BREAK, PIPE_TRAVEL, TIME_WARNING, GAME_OVER
        }

        private Dictionary<GameSound, SoundEffect> effects;

        private Song backgroundMusic;
        private bool isMuted;
        private SoundManager()
        {
            effects = new Dictionary<GameSound, SoundEffect>();
            isMuted = false;
        }
        public void MapSound(GameSound gameSound, SoundEffect soundEffect)
        {
            this.effects.Add(gameSound, soundEffect);
        }

        public void PlaySound(GameSound gameSound)
        {
            SoundEffect toPlay;
            if(!isMuted && effects.TryGetValue(gameSound, out toPlay))
            {
                
                toPlay.CreateInstance().Play();
                
            }
            
        }

        public void SetBackgroundMusic(Song song)
        {
            backgroundMusic = song;
        }
        public void StartMusic()
        {
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            
        }

        public void ToggleMute()
        {
            isMuted = !isMuted;
            MediaPlayer.IsMuted = isMuted;
        }
    }

    

    
}
