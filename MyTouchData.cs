using System;
using Sce.PlayStation.Core.Input;

namespace Dtictactoe
{
	public struct MyTouchData
	{
		public bool Skip;
		public int ID;
		public MyTouchStatus Status;
		public float X;
		public float Y;
		
		public void ConvertTouchData(TouchData touchData)
		{
			Skip = touchData.Skip;
			ID = touchData.ID;
			switch(touchData.Status)
			{
			case TouchStatus.Canceled:
				Status = MyTouchStatus.Canceled;
				break;
			case TouchStatus.Down:
				Status = MyTouchStatus.Down;
				break;
			case TouchStatus.Move:
				Status = MyTouchStatus.Move;
				break;
			case TouchStatus.Up:
				Status = MyTouchStatus.Up;
				break;
			default:
				Status = MyTouchStatus.None;
				break;
			}
			X = touchData.X;
			Y = touchData.Y;
		}
	}
}

