using NetCoreAudio;
using System;

namespace CapstonePacMan.app.Models 
{
    public static class SoundManager
    {
        private static Player _sfxPlayer = new Player();
        private static Player _chompPlayer = new Player(); 

        public static void PlayBeginning()
        {
            _sfxPlayer.Play("Assets/pacman-art/sounds/pacman-beginning/pacman_beginning.wav");
        }

        public static void PlayChomp()
        {

            _chompPlayer.Play("Assets/pacman-art/sounds/pacman-chomp/pacman_chomp.wav");
        }

        public static void PlayDeath()
        {
            _sfxPlayer.Play("Assets/pacman-art/sounds/pacman-death/pacman_death.wav");
        }

        public static void PlayEatFruit()
        {
            _sfxPlayer.Play("Assets/pacman-art/sounds/pacman-eatfruit/pacman_eatfruit.wav");
        }

        public static void PlayEatGhost()
        {
            _sfxPlayer.Play("Assets/pacman-art/sounds/pacman-eatghost/pacman_eatghost.wav");
        }
        
        public static void PlayIntermission()
        {
            _sfxPlayer.Play("Assets/pacman-art/sounds/pacman-intermission/pacman_intermission.wav");
        }
    }
}