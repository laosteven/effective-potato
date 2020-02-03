using System;

namespace SamEngine
{
	// Token: 0x02000005 RID: 5
	internal class Deck
	{
		// Token: 0x06000027 RID: 39 RVA: 0x0000280E File Offset: 0x00000A0E
		public Deck(int Length)
		{
			this.indices = new int[Length];
			this.Reshuffle();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002828 File Offset: 0x00000A28
		public void Reshuffle()
		{
			for (int i = 0; i < this.indices.Length; i++)
			{
				this.indices[i] = i;
				int num = (int)SamMath.RandomRange(0f, (float)i);
				int num2 = this.indices[i];
				this.indices[i] = this.indices[num];
				this.indices[num] = num2;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002881 File Offset: 0x00000A81
		public int Next()
		{
			int result = this.indices[this.i];
			this.i++;
			if (this.i >= this.indices.Length)
			{
				this.Reshuffle();
				this.i = 0;
			}
			return result;
		}

		// Token: 0x0400000A RID: 10
		public int[] indices;

		// Token: 0x0400000B RID: 11
		private int i;
	}
}
