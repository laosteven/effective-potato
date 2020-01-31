using SamEngine;
using System;

namespace GooseDesktop
{
	internal struct HeartParticle
	{
		public Vector2 position;

		public float deathTime;

		private const float lifetime = 3f;

		private const float velY = 150f;
	}
}