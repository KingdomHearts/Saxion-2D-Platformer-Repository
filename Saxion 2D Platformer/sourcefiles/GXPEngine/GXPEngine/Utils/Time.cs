using System;

namespace GXPEngine
{
	/// <summary>
	/// Contains various time related functions.
	/// </summary>
	public class Time
	{
		private static int startTime;
		private static int previousTime;
		
		static Time() {
			startTime = now;
			newFrame();
		}
		
		/// <summary>
		/// Returns the current system time in milliseconds
		/// </summary>
		public static int now {
			get { return System.Environment.TickCount; }
		}
		
		/// <summary>
		/// Returns this time in milliseconds since the program started		
		/// </summary>
		/// <value>
		/// The time.
		/// </value>
		public static int time {
			get { return now - startTime; }
		}
		
		/// <summary>
		/// Returns the time in milliseconds that has passed since the previous frame
		/// </summary>
		/// <value>
		/// The delta time.
		/// </value>
		public static int deltaTime {
			get { return now - previousTime; }
		}
		
		internal static void newFrame() {
			previousTime = now;
		}
	}
}

