using System;
using System.Diagnostics;

namespace SamEngine
{
	public static class Time
	{
		public const int framerate = 120;

		public const float deltaTime = 0.008333334f;

		public static Stopwatch timeStopwatch;

		public static float time;

		static Time()
		{
			Time.timeStopwatch = new Stopwatch();
			Time.timeStopwatch.Start();
			Time.TickTime();
		}

		public static void TickTime()
		{
			Time.time = (float)Time.timeStopwatch.Elapsed.TotalSeconds;
		}
	}
}