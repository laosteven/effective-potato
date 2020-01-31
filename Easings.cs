using System;

public static class Easings
{
	private const float PI = 3.14159274f;

	private const float HALFPI = 1.57079637f;

	public static float BackEaseIn(float p)
	{
		return p * p * p - p * (float)Math.Sin((double)(p * 3.14159274f));
	}

	public static float BackEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			float single = 2f * p;
			return 0.5f * (single * single * single - single * (float)Math.Sin((double)(single * 3.14159274f)));
		}
		float single1 = 1f - (2f * p - 1f);
		return 0.5f * (1f - (single1 * single1 * single1 - single1 * (float)Math.Sin((double)(single1 * 3.14159274f)))) + 0.5f;
	}

	public static float BackEaseOut(float p)
	{
		float single = 1f - p;
		return 1f - (single * single * single - single * (float)Math.Sin((double)(single * 3.14159274f)));
	}

	public static float BounceEaseIn(float p)
	{
		return 1f - Easings.BounceEaseOut(1f - p);
	}

	public static float BounceEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 0.5f * Easings.BounceEaseIn(p * 2f);
		}
		return 0.5f * Easings.BounceEaseOut(p * 2f - 1f) + 0.5f;
	}

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

	public static float CircularEaseIn(float p)
	{
		return 1f - (float)Math.Sqrt((double)(1f - p * p));
	}

	public static float CircularEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 0.5f * (1f - (float)Math.Sqrt((double)(1f - 4f * (p * p))));
		}
		return 0.5f * ((float)Math.Sqrt((double)(-(2f * p - 3f) * (2f * p - 1f))) + 1f);
	}

	public static float CircularEaseOut(float p)
	{
		return (float)Math.Sqrt((double)((2f - p) * p));
	}

	public static float CubicEaseIn(float p)
	{
		return p * p * p;
	}

	public static float CubicEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 4f * p * p * p;
		}
		float single = 2f * p - 2f;
		return 0.5f * single * single * single + 1f;
	}

	public static float CubicEaseOut(float p)
	{
		float single = p - 1f;
		return single * single * single + 1f;
	}

	public static float ElasticEaseIn(float p)
	{
		return (float)Math.Sin((double)(20.4203529f * p)) * (float)Math.Pow(2, (double)(10f * (p - 1f)));
	}

	public static float ElasticEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 0.5f * (float)Math.Sin((double)(20.4203529f * (2f * p))) * (float)Math.Pow(2, (double)(10f * (2f * p - 1f)));
		}
		return 0.5f * ((float)Math.Sin((double)(-20.4203529f * (2f * p - 1f + 1f))) * (float)Math.Pow(2, (double)(-10f * (2f * p - 1f))) + 2f);
	}

	public static float ElasticEaseOut(float p)
	{
		return (float)Math.Sin((double)(-20.4203529f * (p + 1f))) * (float)Math.Pow(2, (double)(-10f * p)) + 1f;
	}

	public static float ExponentialEaseIn(float p)
	{
		if (p == 0f)
		{
			return p;
		}
		return (float)Math.Pow(2, (double)(10f * (p - 1f)));
	}

	public static float ExponentialEaseInOut(float p)
	{
		if ((double)p == 0 || (double)p == 1)
		{
			return p;
		}
		if (p < 0.5f)
		{
			return 0.5f * (float)Math.Pow(2, (double)(20f * p - 10f));
		}
		return -0.5f * (float)Math.Pow(2, (double)(-20f * p + 10f)) + 1f;
	}

	public static float ExponentialEaseOut(float p)
	{
		if (p == 1f)
		{
			return p;
		}
		return 1f - (float)Math.Pow(2, (double)(-10f * p));
	}

	public static float Interpolate(float p, Easings.Functions function)
	{
		switch (function)
		{
			case Easings.Functions.QuadraticEaseIn:
			{
				return Easings.QuadraticEaseIn(p);
			}
			case Easings.Functions.QuadraticEaseOut:
			{
				return Easings.QuadraticEaseOut(p);
			}
			case Easings.Functions.QuadraticEaseInOut:
			{
				return Easings.QuadraticEaseInOut(p);
			}
			case Easings.Functions.CubicEaseIn:
			{
				return Easings.CubicEaseIn(p);
			}
			case Easings.Functions.CubicEaseOut:
			{
				return Easings.CubicEaseOut(p);
			}
			case Easings.Functions.CubicEaseInOut:
			{
				return Easings.CubicEaseInOut(p);
			}
			case Easings.Functions.QuarticEaseIn:
			{
				return Easings.QuarticEaseIn(p);
			}
			case Easings.Functions.QuarticEaseOut:
			{
				return Easings.QuarticEaseOut(p);
			}
			case Easings.Functions.QuarticEaseInOut:
			{
				return Easings.QuarticEaseInOut(p);
			}
			case Easings.Functions.QuinticEaseIn:
			{
				return Easings.QuinticEaseIn(p);
			}
			case Easings.Functions.QuinticEaseOut:
			{
				return Easings.QuinticEaseOut(p);
			}
			case Easings.Functions.QuinticEaseInOut:
			{
				return Easings.QuinticEaseInOut(p);
			}
			case Easings.Functions.SineEaseIn:
			{
				return Easings.SineEaseIn(p);
			}
			case Easings.Functions.SineEaseOut:
			{
				return Easings.SineEaseOut(p);
			}
			case Easings.Functions.SineEaseInOut:
			{
				return Easings.SineEaseInOut(p);
			}
			case Easings.Functions.CircularEaseIn:
			{
				return Easings.CircularEaseIn(p);
			}
			case Easings.Functions.CircularEaseOut:
			{
				return Easings.CircularEaseOut(p);
			}
			case Easings.Functions.CircularEaseInOut:
			{
				return Easings.CircularEaseInOut(p);
			}
			case Easings.Functions.ExponentialEaseIn:
			{
				return Easings.ExponentialEaseIn(p);
			}
			case Easings.Functions.ExponentialEaseOut:
			{
				return Easings.ExponentialEaseOut(p);
			}
			case Easings.Functions.ExponentialEaseInOut:
			{
				return Easings.ExponentialEaseInOut(p);
			}
			case Easings.Functions.ElasticEaseIn:
			{
				return Easings.ElasticEaseIn(p);
			}
			case Easings.Functions.ElasticEaseOut:
			{
				return Easings.ElasticEaseOut(p);
			}
			case Easings.Functions.ElasticEaseInOut:
			{
				return Easings.ElasticEaseInOut(p);
			}
			case Easings.Functions.BackEaseIn:
			{
				return Easings.BackEaseIn(p);
			}
			case Easings.Functions.BackEaseOut:
			{
				return Easings.BackEaseOut(p);
			}
			case Easings.Functions.BackEaseInOut:
			{
				return Easings.BackEaseInOut(p);
			}
			case Easings.Functions.BounceEaseIn:
			{
				return Easings.BounceEaseIn(p);
			}
			case Easings.Functions.BounceEaseOut:
			{
				return Easings.BounceEaseOut(p);
			}
			case Easings.Functions.BounceEaseInOut:
			{
				return Easings.BounceEaseInOut(p);
			}
			default:
			{
				return Easings.Linear(p);
			}
		}
	}

	public static float Linear(float p)
	{
		return p;
	}

	public static float QuadraticEaseIn(float p)
	{
		return p * p;
	}

	public static float QuadraticEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 2f * p * p;
		}
		return -2f * p * p + 4f * p - 1f;
	}

	public static float QuadraticEaseOut(float p)
	{
		return -(p * (p - 2f));
	}

	public static float QuarticEaseIn(float p)
	{
		return p * p * p * p;
	}

	public static float QuarticEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 8f * p * p * p * p;
		}
		float single = p - 1f;
		return -8f * single * single * single * single + 1f;
	}

	public static float QuarticEaseOut(float p)
	{
		float single = p - 1f;
		return single * single * single * (1f - p) + 1f;
	}

	public static float QuinticEaseIn(float p)
	{
		return p * p * p * p * p;
	}

	public static float QuinticEaseInOut(float p)
	{
		if (p < 0.5f)
		{
			return 16f * p * p * p * p * p;
		}
		float single = 2f * p - 2f;
		return 0.5f * single * single * single * single * single + 1f;
	}

	public static float QuinticEaseOut(float p)
	{
		float single = p - 1f;
		return single * single * single * single * single + 1f;
	}

	public static float SineEaseIn(float p)
	{
		return (float)Math.Sin((double)((p - 1f) * 1.57079637f)) + 1f;
	}

	public static float SineEaseInOut(float p)
	{
		return 0.5f * (1f - (float)Math.Cos((double)(p * 3.14159274f)));
	}

	public static float SineEaseOut(float p)
	{
		return (float)Math.Sin((double)(p * 1.57079637f));
	}

	public enum Functions
	{
		Linear,
		QuadraticEaseIn,
		QuadraticEaseOut,
		QuadraticEaseInOut,
		CubicEaseIn,
		CubicEaseOut,
		CubicEaseInOut,
		QuarticEaseIn,
		QuarticEaseOut,
		QuarticEaseInOut,
		QuinticEaseIn,
		QuinticEaseOut,
		QuinticEaseInOut,
		SineEaseIn,
		SineEaseOut,
		SineEaseInOut,
		CircularEaseIn,
		CircularEaseOut,
		CircularEaseInOut,
		ExponentialEaseIn,
		ExponentialEaseOut,
		ExponentialEaseInOut,
		ElasticEaseIn,
		ElasticEaseOut,
		ElasticEaseInOut,
		BackEaseIn,
		BackEaseOut,
		BackEaseInOut,
		BounceEaseIn,
		BounceEaseOut,
		BounceEaseInOut
	}
}