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
            STANDARD_JUMP, SUPER_JUMP, STOMP, DEATH, COIN, POWER_UP_APPEAR, POWER_UP_COLLECTED, ONE_UP_COLLECTED, BUMP, BRICK_BREAK, PIPE_TRAVEL, GAME_OVER, LEVEL_CLEAR
        }

        private Dictionary<GameSound, SoundEffect> effects;

        private Song backgroundMusic , backgroundMusicFast, activeSong;
        private bool isMuted, isPaused;
     
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
            if(!(isMuted || isPaused)&& effects.TryGetValue(gameSound, out toPlay))
            {
                if (gameSound == GameSound.DEATH || gameSound == GameSound.GAME_OVER || gameSound == GameSound.LEVEL_CLEAR) MediaPlayer.Stop();
                toPlay.CreateInstance().Play();
                
            }
            
        }

        public void SetBackgroundMusic(Song mainSong, Song fastSong)
        {
            backgroundMusic = mainSong;
            backgroundMusicFast = fastSong;
            activeSong = backgroundMusic;
        }
        public void StartMusic()
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(activeSong);
            MediaPlayer.IsRepeating = true;
            
        }

        public void ToggleMute()
        {
            isMuted = !isMuted;
            MediaPlayer.IsMuted = isMuted;
        }
        public void TogglePaused()
        {
            isPaused = !isPaused;
            if (isPaused)
                MediaPlayer.Pause();
            else
                MediaPlayer.Resume();
        }

        public void TimeWarning()
        {
            if (activeSong != backgroundMusicFast) {
                activeSong = backgroundMusicFast;
                StartMusic();
            }
            
        }

        public void Reset()
        {
            if (activeSong != backgroundMusic)
            {
                activeSong = backgroundMusic;
                StartMusic();
            }

        }

    }

    

    
}
