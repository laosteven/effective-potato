using System;

namespace SamEngine
{
	// Token: 0x02000006 RID: 6
	public struct Vector2
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000028BB File Offset: 0x00000ABB
		public Vector2(float _x, float _y)
		{
			this.x = _x;
			this.y = _y;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000028CB File Offset: 0x00000ACB
		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x + b.x, a.y + b.y);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000028EC File Offset: 0x00000AEC
		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x - b.x, a.y - b.y);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000290D File Offset: 0x00000B0D
		public static Vector2 operator -(Vector2 a)
		{
			return a * -1f;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000291A File Offset: 0x00000B1A
		public static Vector2 operator *(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x * b.x, a.y * b.y);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000293B File Offset: 0x00000B3B
		public static Vector2 operator *(Vector2 a, float b)
		{
			return new Vector2(a.x * b, a.y * b);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002952 File Offset: 0x00000B52
		public static Vector2 operator /(Vector2 a, float b)
		{
			return new Vector2(a.x / b, a.y / b);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002969 File Offset: 0x00000B69
		public static Vector2 GetFromAngleDegrees(float angle)
		{
			return new Vector2((float)Math.Cos((double)(angle * 0.0174532924f)), (float)Math.Sin((double)(angle * 0.0174532924f)));
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000298C File Offset: 0x00000B8C
		public static float Distance(Vector2 a, Vector2 b)
		{
			Vector2 vector = new Vector2(a.x - b.x, a.y - b.y);
			return (float)Math.Sqrt((double)(vector.x * vector.x + vector.y * vector.y));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000029DC File Offset: 0x00000BDC
		public static Vector2 Lerp(Vector2 a, Vector2 b, float p)
		{
			return new Vector2(SamMath.Lerp(a.x, b.x, p), SamMath.Lerp(a.y, b.y, p));
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002A07 File Offset: 0x00000C07
		public static float Dot(Vector2 a, Vector2 b)
		{
			return a.x * b.x + a.y * b.y;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002A24 File Offset: 0x00000C24
		public static Vector2 Normalize(Vector2 a)
		{
			if (a.x == 0f && a.y == 0f)
			{
				return Vector2.zero;
			}
			float num = (float)Math.Sqrt((double)(a.x * a.x + a.y * a.y));
			return new Vector2(a.x / num, a.y / num);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A89 File Offset: 0x00000C89
		public static float Magnitude(Vector2 a)
		{
			return (float)Math.Sqrt((double)(a.x * a.x + a.y * a.y));
		}

		// Token: 0x0400000C RID: 12
		public float x;

		// Token: 0x0400000D RID: 13
		public float y;

		// Token: 0x0400000E RID: 14
		public static readonly Vector2 zero = new Vector2(0f, 0f);
	}
}
