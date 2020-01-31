using System;

namespace SamEngine
{
	public struct Vector2
	{
		public float x;

		public float y;

		public readonly static Vector2 zero;

		static Vector2()
		{
			Vector2.zero = new Vector2(0f, 0f);
		}

		public Vector2(float _x, float _y)
		{
			this.x = _x;
			this.y = _y;
		}

		public static float Distance(Vector2 a, Vector2 b)
		{
			Vector2 vector2 = new Vector2(a.x - b.x, a.y - b.y);
			return (float)Math.Sqrt((double)(vector2.x * vector2.x + vector2.y * vector2.y));
		}

		public static float Dot(Vector2 a, Vector2 b)
		{
			return a.x * b.x + a.y * b.y;
		}

		public static Vector2 GetFromAngleDegrees(float angle)
		{
			return new Vector2((float)Math.Cos((double)(angle * 0.0174532924f)), (float)Math.Sin((double)(angle * 0.0174532924f)));
		}

		public static Vector2 Lerp(Vector2 a, Vector2 b, float p)
		{
			return new Vector2(SamMath.Lerp(a.x, b.x, p), SamMath.Lerp(a.y, b.y, p));
		}

		public static float Magnitude(Vector2 a)
		{
			return (float)Math.Sqrt((double)(a.x * a.x + a.y * a.y));
		}

		public static Vector2 Normalize(Vector2 a)
		{
			if (a.x == 0f && a.y == 0f)
			{
				return Vector2.zero;
			}
			float single = (float)Math.Sqrt((double)(a.x * a.x + a.y * a.y));
			return new Vector2(a.x / single, a.y / single);
		}

		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x + b.x, a.y + b.y);
		}

		public static Vector2 operator /(Vector2 a, float b)
		{
			return new Vector2(a.x / b, a.y / b);
		}

		public static Vector2 operator *(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x * b.x, a.y * b.y);
		}

		public static Vector2 operator *(Vector2 a, float b)
		{
			return new Vector2(a.x * b, a.y * b);
		}

		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x - b.x, a.y - b.y);
		}

		public static Vector2 operator -(Vector2 a)
		{
			return a * -1f;
		}
	}
}