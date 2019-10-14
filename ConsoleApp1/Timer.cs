using System.Diagnostics;

namespace MatrixHierarchies
{
    public class Timer
    {
        Stopwatch stopwatch = new Stopwatch();

        private long currentTime = 0;
        private long lastTime = 0;

        private float deltaTime = 0.005f;

        public Timer()
        {
            stopwatch.Start();
        }

        /// <summary>
        /// Resets the Timer back to 0.
        /// </summary>
        public void Reset()
        {
            stopwatch.Reset();
        }

        /// <summary>
        /// Gets elapsed the time in seconds.
        /// </summary>
        public float Seconds
        {
            get { return stopwatch.ElapsedMilliseconds / 1000.0f; }
        }

        /// <summary>
        /// Tracks the time between now and the time the function was last called. WARNING: Calling this function multiple times per frame will corrupt your deltaTime values.
        /// </summary>
        public float GetDeltaTime()
        {
            lastTime = currentTime;
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;
            return deltaTime;
        }
    }
}
