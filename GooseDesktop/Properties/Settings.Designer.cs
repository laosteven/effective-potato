﻿using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace GooseDesktop.Properties
{
	// Token: 0x02000012 RID: 18
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00004E3A File Offset: 0x0000303A
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x04000064 RID: 100
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
