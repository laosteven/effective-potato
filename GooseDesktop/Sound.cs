using GooseDesktop.Properties;
using SamEngine;
using System;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;

namespace GooseDesktop
{
	internal static class Sound
	{
		public static Sound.Mp3Player honkBiteSoundPlayer;

		public static Sound.Mp3Player musicPlayer;

		public static Sound.Mp3Player environmentSoundsPlayer;

		private readonly static Stream[] patSources;

		private static SoundPlayer[] patSoundPool;

		private readonly static string[] honkSources;

		private readonly static string biteSource;

		static Sound()
		{
			Sound.patSources = new Stream[] { Resources.Pat1, Resources.Pat2, Resources.Pat3 };
			Sound.honkSources = new string[] { Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/Honk1.mp3"), Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/Honk2.mp3"), Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/Honk3.mp3"), Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/Honk4.mp3") };
			Sound.biteSource = Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/BITE.mp3");
		}

		public static void CHOMP()
		{
			Sound.honkBiteSoundPlayer.Pause();
			Sound.honkBiteSoundPlayer.Dispose();
			Sound.honkBiteSoundPlayer.ChangeFile(Sound.biteSource);
			Sound.honkBiteSoundPlayer.SetVolume(0.07f);
			Sound.honkBiteSoundPlayer.Play();
		}

		public static void HONCC()
		{
			int num = (int)(SamMath.Rand.NextDouble() * (double)((int)Sound.honkSources.Length));
			Sound.honkBiteSoundPlayer.Pause();
			Sound.honkBiteSoundPlayer.Dispose();
			Sound.honkBiteSoundPlayer.ChangeFile(Sound.honkSources[num]);
			Sound.honkBiteSoundPlayer.SetVolume(0.8f);
			Sound.honkBiteSoundPlayer.Play();
		}

		public static void Init()
		{
			Sound.honkBiteSoundPlayer = new Sound.Mp3Player(Sound.honkSources[0], "honkPlayer");
			Sound.patSoundPool = new SoundPlayer[(int)Sound.patSources.Length];
			for (int i = 0; i < (int)Sound.patSources.Length; i++)
			{
				Sound.patSoundPool[i] = new SoundPlayer(Sound.patSources[i]);
				Sound.patSoundPool[i].Load();
			}
			Sound.environmentSoundsPlayer = new Sound.Mp3Player(Program.GetPathToFileInAssembly("Assets/Sound/NotEmbedded/MudSquith.mp3"), "assortedEnvironment");
			string pathToFileInAssembly = Program.GetPathToFileInAssembly("Assets/Sound/Music/Music.mp3");
			if (File.Exists(pathToFileInAssembly))
			{
				Sound.musicPlayer = new Sound.Mp3Player(pathToFileInAssembly, "musicPlayer")
				{
					loop = true
				};
				Sound.musicPlayer.SetVolume(0.5f);
				Sound.musicPlayer.Play();
			}
		}

		public static void PlayMudSquith()
		{
			Sound.environmentSoundsPlayer.Restart();
			Sound.environmentSoundsPlayer.Play();
		}

		public static void PlayPat()
		{
			int num = (int)(SamMath.Rand.NextDouble() * (double)((int)Sound.patSoundPool.Length));
			SoundPlayer soundPlayer = Sound.patSoundPool[num];
			if (soundPlayer.Stream.CanSeek)
			{
				soundPlayer.Stream.Seek((long)0, SeekOrigin.Begin);
			}
			soundPlayer.Play();
		}

		public class Mp3Player
		{
			public bool loop;

			private string @alias;

			public Mp3Player(string filename, string playerAlias)
			{
				this.@alias = playerAlias;
				Sound.Mp3Player.mciSendString(string.Format("open \"{0}\" type MPEGVideo alias {1}", filename, this.@alias), null, 0, IntPtr.Zero);
			}

			public void ChangeFile(string newFilePath)
			{
				Sound.Mp3Player.mciSendString(string.Format("open \"{0}\" type MPEGVideo alias {1}", newFilePath, this.@alias), null, 0, IntPtr.Zero);
			}

			public void Dispose()
			{
				Sound.Mp3Player.mciSendString(string.Format("close {0}", this.@alias), null, 0, IntPtr.Zero);
			}

			[DllImport("winmm.dll", CharSet=CharSet.None, ExactSpelling=false)]
			private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hWndCallback);

			public void Pause()
			{
				Sound.Mp3Player.mciSendString(string.Format("stop {0}", this.@alias), null, 0, IntPtr.Zero);
			}

			public void Play()
			{
				string str = "play {0}";
				str = string.Format(str, this.@alias);
				if (this.loop)
				{
					str = string.Concat(str, " REPEAT");
				}
				Sound.Mp3Player.mciSendString(str, null, 0, IntPtr.Zero);
			}

			public void Restart()
			{
				Sound.Mp3Player.mciSendString(string.Format("seek {0} to start", this.@alias), null, 0, IntPtr.Zero);
			}

			public void SetVolume(float volume)
			{
				int num = (int)Math.Max(Math.Min(volume * 1000f, 1000f), 0f);
				Sound.Mp3Player.mciSendString(string.Format("setaudio {0} volume to {1}", this.@alias, num), null, 0, IntPtr.Zero);
			}
		}
	}
}