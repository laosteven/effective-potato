using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GooseDesktop
{
	public static class GooseConfig
	{
		private static string filePath;

		public const int GOOSE_CONFIG_VERSION = 0;

		public static GooseConfig.ConfigSettings settings;

		static GooseConfig()
		{
			GooseConfig.filePath = Program.GetPathToFileInAssembly("config.goos");
			GooseConfig.settings = null;
		}

		public static void LoadConfig()
		{
			GooseConfig.settings = GooseConfig.ConfigSettings.ReadFileIntoConfig(GooseConfig.filePath);
		}

		public class ConfigSettings
		{
			public int Version;

			public bool CanAttackAtRandom;

			public float MinWanderingTimeSeconds;

			public float MaxWanderingTimeSeconds;

			public float FirstWanderTimeSeconds;

			public ConfigSettings()
			{
			}

			public static string GenerateTextFromSettings(GooseConfig.ConfigSettings f)
			{
				StringBuilder stringBuilder = new StringBuilder();
				FieldInfo[] fields = typeof(GooseConfig.ConfigSettings).GetFields();
				for (int i = 0; i < (int)fields.Length; i++)
				{
					FieldInfo fieldInfo = fields[i];
					stringBuilder.Append(string.Format("{0}={1}\n", fieldInfo.Name, fieldInfo.GetValue(f).ToString()));
				}
				return stringBuilder.ToString();
			}

			public static GooseConfig.ConfigSettings ReadFileIntoConfig(string configGivenPath)
			{
				GooseConfig.ConfigSettings configSetting;
				GooseConfig.ConfigSettings configSetting1 = new GooseConfig.ConfigSettings();
				if (!File.Exists(configGivenPath))
				{
					MessageBox.Show("Can't find config.goos file! Creating a new one with default values");
					GooseConfig.ConfigSettings.WriteConfigToFile(configGivenPath, configSetting1);
					return configSetting1;
				}
				try
				{
					using (StreamReader streamReader = new StreamReader(configGivenPath))
					{
						Dictionary<string, string> strs = new Dictionary<string, string>();
						while (true)
						{
							string str = streamReader.ReadLine();
							string str1 = str;
							if (str == null)
							{
								break;
							}
							string[] strArrays = str1.Split(new char[] { '=' });
							if ((int)strArrays.Length == 2)
							{
								strs.Add(strArrays[0], strArrays[1]);
							}
						}
						int num = -1;
						int.TryParse(strs["Version"], out num);
						if (num == 0)
						{
							foreach (KeyValuePair<string, string> keyValuePair in strs)
							{
								FieldInfo field = typeof(GooseConfig.ConfigSettings).GetField(keyValuePair.Key);
								try
								{
									field.SetValue(configSetting1, Convert.ChangeType(keyValuePair.Value, field.FieldType));
								}
								catch
								{
									MessageBox.Show(string.Concat("Loading config error: field ", field.Name, "'s value is not valid. Setting it to the default value."));
								}
							}
						}
						else
						{
							MessageBox.Show("config.goos is for the wrong version! Creating a new one with default values!");
							File.Delete(configGivenPath);
							GooseConfig.ConfigSettings.WriteConfigToFile(configGivenPath, configSetting1);
							configSetting = configSetting1;
							return configSetting;
						}
					}
					return configSetting1;
				}
				catch
				{
					MessageBox.Show("config.goos corrupt! Creating a new one!");
					File.Delete(configGivenPath);
					GooseConfig.ConfigSettings.WriteConfigToFile(configGivenPath, configSetting1);
					configSetting = configSetting1;
				}
				return configSetting;
			}

			public static void WriteConfigToFile(string path, GooseConfig.ConfigSettings f)
			{
				using (StreamWriter streamWriter = File.CreateText(path))
				{
					streamWriter.Write(GooseConfig.ConfigSettings.GenerateTextFromSettings(f));
				}
			}
		}
	}
}