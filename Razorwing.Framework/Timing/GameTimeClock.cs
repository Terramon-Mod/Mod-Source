using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Razorwing.Framework.Timing
{
    public class GameTimeClock : IClock, IDisposable
    {
        public GameTime GameTime { get; private set; }

        public double CurrentTime
        {
            get => GameTime.TotalGameTime.TotalMilliseconds;
            private set
            {
                //We can't change in game timers, so no needed
            }

        }
        public double Rate { get; set; }
        public bool IsRunning { get; } = true;

        public void Reset() => CurrentTime = 0;


        public void Start()
        {
        }

        public void UpdateTime(GameTime time)
        {
            GameTime = time;
        }


        public void Dispose()
        {

        }
    }
}
