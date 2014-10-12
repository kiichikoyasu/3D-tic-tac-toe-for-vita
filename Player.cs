using System;

namespace Dtictactoe
{	
	public struct Player
	{
		public int id;
		public bool isHuman;
		public int order;
		/* コンピュータがマスを選ぶ基準に用いる */
		public int[,,,] score;
	}
}

