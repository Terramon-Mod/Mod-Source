using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Terramon.Sounds.UI
{
    public class lowhp : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            //An optional variable to make controlling the volume easier
            float volumeFactor = 0.35f;

            if (soundInstance is null)
            {
                //This is a new sound instance

                soundInstance = sound.CreateInstance();
                soundInstance.Volume = volume * volumeFactor;
                soundInstance.Pan = pan;
                Main.PlaySoundInstance(soundInstance);
                return soundInstance;
            }
            else if (soundInstance.State == SoundState.Stopped)
            {
                //This is an existing sound instance that just stopped (OPTIONAL: use this if you want a looping sound effect!)

                soundInstance.Volume = volume * volumeFactor;
                soundInstance.Pan = pan;
                Main.PlaySoundInstance(soundInstance);
                return soundInstance;
            }

            //This is an existing sound instance that's still playing
            soundInstance.Volume = volume * volumeFactor;
            soundInstance.Pan = pan;
            return soundInstance;
        }
    }
}
