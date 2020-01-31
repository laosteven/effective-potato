using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GooseDesktop.Properties
{
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		private static System.Resources.ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		internal static UnmanagedMemoryStream Pat1
		{
			get
			{
				return Resources.ResourceManager.GetStream("Pat1", Resources.resourceCulture);
			}
		}

		internal static UnmanagedMemoryStream Pat2
		{
			get
			{
				return Resources.ResourceManager.GetStream("Pat2", Resources.resourceCulture);
			}
		}

		internal static UnmanagedMemoryStream Pat3
		{
			get
			{
				return Resources.ResourceManager.GetStream("Pat3", Resources.resourceCulture);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static System.Resources.ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new System.Resources.ResourceManager("GooseDesktop.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		internal Resources()
		{
		}
	}
}