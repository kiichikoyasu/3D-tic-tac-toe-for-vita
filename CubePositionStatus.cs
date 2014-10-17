using System;

namespace Dtictactoe
{
	/**
	 * cubeが3×3×3のどの部分にあたるかの情報
	 * 1列そろっているか判定の際や、スコアの更新に用いる（はず）
	 * 上から
	 * 角
	 * 辺の中心
	 * 面の中心
	 * 立体の中心
	 * 
	 * */
	public enum CubePositionStatus : int
	{
		Vertex = 0,
		EgdeMiddle = 1,
		SurfaceCenter = 2,
		Core = 3,
	}
}

