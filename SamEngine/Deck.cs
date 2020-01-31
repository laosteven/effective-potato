using System;

namespace SamEngine
{
	internal class Deck
	{
		public int[] indices;

		private int i;

		public Deck(int Length)
		{
			this.indices = new int[Length];
			this.Reshuffle();
		}

		public int Next()
		{
			int num = this.indices[this.i];
			this.i++;
			if (this.i >= (int)this.indices.Length)
			{
				this.Reshuffle();
				this.i = 0;
			}
			return num;
		}

		public void Reshuffle()
		{
			for (int i = 0; i < (int)this.indices.Length; i++)
			{
				this.indices[i] = i;
				int num = (int)SamMath.RandomRange(0f, (float)i);
				int num1 = this.indices[i];
				this.indices[i] = this.indices[num];
				this.indices[num] = num1;
			}
		}
	}
}