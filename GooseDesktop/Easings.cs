﻿using System;

// Token: 0x02000002 RID: 2
public static class Easings
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static float Interpolate(float p, Easings.Functions function)
	{
		switch (function)
		{
		default:
			return Easings.Linear(p);
		case Easings.Functions.QuadraticEaseIn:
			return Easings.QuadraticEaseIn(p);
		case Easings.Functions.QuadraticEaseOut:
			return Easings.QuadraticEaseOut(p);
		case Easings.Functions.QuadraticEaseInOut:
			return Easings.QuadraticEaseInOut(p);
		case Easings.Functions.CubicEaseIn:
			return Easings.CubicEaseIn(p);
		case Easings.Functions.CubicEaseOut:
			return Easings.CubicEaseOut(p);
		case Easings.Functions.CubicEaseInOut:
			return Easings.CubicEaseInOut(p);
		case Easings.Functions.QuarticEaseIn:
			return Easings.QuarticEaseIn(p);
		case Easings.Functions.QuarticEaseOut:
			return Easings.QuarticEaseOut(p);
		case Easings.Functions.QuarticEaseInOut:
			return Easings.QuarticEaseInOut(p);
		case Easings.Functions.QuinticEaseIn:
			return Easings.QuinticEaseIn(p);
		case Easings.Functions.QuinticEaseOut:
			return Easings.QuinticEaseOut(p);
		case Easings.Functions.QuinticEaseInOut:
			return Easings.QuinticEaseInOut(p);
		case Easings.Functions.SineEaseIn:
			return Easings.SineEaseIn(p);
		case Easings.Functions.SineEaseOut:
			return Easings.SineEaseOut(p);
		case Easings.Functions.SineEaseInOut:
			return Easings.SineEaseInOut(p);
		case Easings.Functions.CircularEaseIn:
			return Easings.CircularEaseIn(p);
		case Easings.Functions.CircularEaseOut:
			return Easings.CircularEaseOut(p);
		case Easings.Functions.CircularEaseInOut:
			return Easings.CircularEaseInOut(p);
		case Easings.Functions.ExponentialEaseIn:
			return Easings.ExponentialEaseIn(p);
		case Easings.Functions.ExponentialEaseOut:
			return Easings.ExponentialEaseOut(p);
		case Easings.Functions.ExponentialEaseInOut:
			return Easings.ExponentialEaseInOut(p);
		case Easings.Functions.ElasticEaseIn:
			return Easings.ElasticEaseIn(p);
		case Easings.Functions.ElasticEaseOut:
			return Easings.ElasticEaseOut(p);
		case Easings.Functions.ElasticEaseInOut:
			return Easings.ElasticEaseInOut(p);
		case Easings.Functions.BackEaseIn:
			return Easings.BackEaseIn(p);
		case Easings.Functions.BackEaseOut:
			return Easings.BackEaseOut(p);
		case Easings.Functions.BackEaseInOut:
			return Easings.BackEaseInOut(p);
		case Easings.Functions.BounceEaseIn:
			return Easings.BounceEaseIn(p);
		case Easings.Functions.BounceEaseOut:
			return Easings.BounceEaseOut(p);
		case Easings.Functions.BounceEaseInOut:
			return Easings.BounceEaseInOut(p);
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x000021B7 File Offset: 0x000003B7
	public static float Linear(float p)
	{
		return p;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000021BA File Offset: 0x000003BA
	public static float QuadraticEaseIn(float p)
	{
		return p * p;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000021BF File Offset: 0x000003BF
	public static float QuadraticEaseOut(float p)
	{
		return -(p * (p - 2f));
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000021CB File Offset: 0x000003CB
	public static float QuadraticEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 2f * p * p;
		}
		return -2f * p * p + 4f * p - 1f;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000021F6 File Offset: 0x000003F6
	public static float CubicEaseIn(float p)
	{
		return p * p * p;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002200 File Offset: 0x00000400
	public static float CubicEaseOut(float p)
	{
		float num = p - 1f;
		return num * num * num + 1f;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002220 File Offset: 0x00000420
	public static float CubicEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 4f * p * p * p;
		}
		float num = 2f * p - 2f;
		return 0.5f * num * num * num + 1f;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002260 File Offset: 0x00000460
	public static float QuarticEaseIn(float p)
	{
		return p * p * p * p;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x0000226C File Offset: 0x0000046C
	public static float QuarticEaseOut(float p)
	{
		float num = p - 1f;
		return num * num * num * (1f - p) + 1f;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002294 File Offset: 0x00000494
	public static float QuarticEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 8f * p * p * p * p;
		}
		float num = p - 1f;
		return -8f * num * num * num * num + 1f;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000022D2 File Offset: 0x000004D2
	public static float QuinticEaseIn(float p)
	{
		return p * p * p * p * p;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000022E0 File Offset: 0x000004E0
	public static float QuinticEaseOut(float p)
	{
		float num = p - 1f;
		return num * num * num * num * num + 1f;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002304 File Offset: 0x00000504
	public static float QuinticEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 16f * p * p * p * p * p;
		}
		float num = 2f * p - 2f;
		return 0.5f * num * num * num * num * num + 1f;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000234C File Offset: 0x0000054C
	public static float SineEaseIn(float p)
	{
		return (float)Math.Sin((double)((p - 1f) * 1.57079637f)) + 1f;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002368 File Offset: 0x00000568
	public static float SineEaseOut(float p)
	{
		return (float)Math.Sin((double)(p * 1.57079637f));
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002378 File Offset: 0x00000578
	public static float SineEaseInOut(float p)
	{
		return 0.5f * (1f - (float)Math.Cos((double)(p * 3.14159274f)));
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002394 File Offset: 0x00000594
	public static float CircularEaseIn(float p)
	{
		return 1f - (float)Math.Sqrt((double)(1f - p * p));
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000023AC File Offset: 0x000005AC
	public static float CircularEaseOut(float p)
	{
		return (float)Math.Sqrt((double)((2f - p) * p));
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000023C0 File Offset: 0x000005C0
	public static float CircularEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 0.5f * (1f - (float)Math.Sqrt((double)(1f - 4f * (p * p))));
		}
		return 0.5f * ((float)Math.Sqrt((double)(-(double)(2f * p - 3f) * (2f * p - 1f))) + 1f);
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002427 File Offset: 0x00000627
	public static float ExponentialEaseIn(float p)
	{
		if (p != 0f)
		{
			return (float)Math.Pow(2.0, (double)(10f * (p - 1f)));
		}
		return p;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002450 File Offset: 0x00000650
	public static float ExponentialEaseOut(float p)
	{
		if (p != 1f)
		{
			return 1f - (float)Math.Pow(2.0, (double)(-10f * p));
		}
		return p;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000247C File Offset: 0x0000067C
	public static float ExponentialEaseInOut(float p)
	{
		if ((double)p == 0.0 || (double)p == 1.0)
		{
			return p;
		}
		if (p < 0.5f)
		{
			return 0.5f * (float)Math.Pow(2.0, (double)(20f * p - 10f));
		}
		return -0.5f * (float)Math.Pow(2.0, (double)(-20f * p + 10f)) + 1f;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x000024FA File Offset: 0x000006FA
	public static float ElasticEaseIn(float p)
	{
		return (float)Math.Sin((double)(20.4203529f * p)) * (float)Math.Pow(2.0, (double)(10f * (p - 1f)));
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002528 File Offset: 0x00000728
	public static float ElasticEaseOut(float p)
	{
		return (float)Math.Sin((double)(-20.4203529f * (p + 1f))) * (float)Math.Pow(2.0, (double)(-10f * p)) + 1f;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000255C File Offset: 0x0000075C
	public static float ElasticEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 0.5f * (float)Math.Sin((double)(20.4203529f * (2f * p))) * (float)Math.Pow(2.0, (double)(10f * (2f * p - 1f)));
		}
		return 0.5f * ((float)Math.Sin((double)(-20.4203529f * (2f * p - 1f + 1f))) * (float)Math.Pow(2.0, (double)(-10f * (2f * p - 1f))) + 2f);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002600 File Offset: 0x00000800
	public static float BackEaseIn(float p)
	{
		return p * p * p - p * (float)Math.Sin((double)(p * 3.14159274f));
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002618 File Offset: 0x00000818
	public static float BackEaseOut(float p)
	{
		float num = 1f - p;
		return 1f - (num * num * num - num * (float)Math.Sin((double)(num * 3.14159274f)));
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000264C File Offset: 0x0000084C
	public static float BackEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			float num = 2f * p;
			return 0.5f * (num * num * num - num * (float)Math.Sin((double)(num * 3.14159274f)));
		}
		float num2 = 1f - (2f * p - 1f);
		return 0.5f * (1f - (num2 * num2 * num2 - num2 * (float)Math.Sin((double)(num2 * 3.14159274f)))) + 0.5f;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000026C2 File Offset: 0x000008C2
	public static float BounceEaseIn(float p)
	{
		return 1f - Easings.BounceEaseOut(1f - p);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000026D8 File Offset: 0x000008D8
	public static float BounceEaseOut(float p)
	{
		if (p < 0.363636374f)
		{
			return 121f * p * p / 16f;
		}
		if (p < 0.727272749f)
		{
			return 9.075f * p * p - 9.9f * p + 3.4f;
		}
		if (p < 0.9f)
		{
			return 12.0664816f * p * p - 19.635458f * p + 8.898061f;
		}
		return 10.8f * p * p - 20.52f * p + 10.72f;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002754 File Offset: 0x00000954
	public static float BounceEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 0.5f * Easings.BounceEaseIn(p * 2f);
		}
		return 0.5f * Easings.BounceEaseOut(p * 2f - 1f) + 0.5f;
	}

	// Token: 0x04000001 RID: 1
	private const float PI = 3.14159274f;

	// Token: 0x04000002 RID: 2
	private const float HALFPI = 1.57079637f;

	// Token: 0x02000014 RID: 20
	public enum Functions
	{
		// Token: 0x04000067 RID: 103
		Linear,
		// Token: 0x04000068 RID: 104
		QuadraticEaseIn,
		// Token: 0x04000069 RID: 105
		QuadraticEaseOut,
		// Token: 0x0400006A RID: 106
		QuadraticEaseInOut,
		// Token: 0x0400006B RID: 107
		CubicEaseIn,
		// Token: 0x0400006C RID: 108
		CubicEaseOut,
		// Token: 0x0400006D RID: 109
		CubicEaseInOut,
		// Token: 0x0400006E RID: 110
		QuarticEaseIn,
		// Token: 0x0400006F RID: 111
		QuarticEaseOut,
		// Token: 0x04000070 RID: 112
		QuarticEaseInOut,
		// Token: 0x04000071 RID: 113
		QuinticEaseIn,
		// Token: 0x04000072 RID: 114
		QuinticEaseOut,
		// Token: 0x04000073 RID: 115
		QuinticEaseInOut,
		// Token: 0x04000074 RID: 116
		SineEaseIn,
		// Token: 0x04000075 RID: 117
		SineEaseOut,
		// Token: 0x04000076 RID: 118
		SineEaseInOut,
		// Token: 0x04000077 RID: 119
		CircularEaseIn,
		// Token: 0x04000078 RID: 120
		CircularEaseOut,
		// Token: 0x04000079 RID: 121
		CircularEaseInOut,
		// Token: 0x0400007A RID: 122
		ExponentialEaseIn,
		// Token: 0x0400007B RID: 123
		ExponentialEaseOut,
		// Token: 0x0400007C RID: 124
		ExponentialEaseInOut,
		// Token: 0x0400007D RID: 125
		ElasticEaseIn,
		// Token: 0x0400007E RID: 126
		ElasticEaseOut,
		// Token: 0x0400007F RID: 127
		ElasticEaseInOut,
		// Token: 0x04000080 RID: 128
		BackEaseIn,
		// Token: 0x04000081 RID: 129
		BackEaseOut,
		// Token: 0x04000082 RID: 130
		BackEaseInOut,
		// Token: 0x04000083 RID: 131
		BounceEaseIn,
		// Token: 0x04000084 RID: 132
		BounceEaseOut,
		// Token: 0x04000085 RID: 133
		BounceEaseInOut
	}
}
