using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Sce.PlayStation.Core.Input;

namespace Dtictactoe
{
	public class TouchDataList : System.Collections.ObjectModel.Collection<MyTouchData>
	{
		public TouchDataList ()
		{
		}
		
		public new void Add(MyTouchData data)
		{
			if (IsContainStatus(MyTouchStatus.Down) && data.Status == MyTouchStatus.Up)
			{
				/* 今フレームでUpで特定フレーム以内にDownがあれば、クリック状態とみなす */
				data.Status = MyTouchStatus.Clicked;
			}
			base.Add(data);
			if(base.Count > GameParameters.PrevFrameSize)
			{
				/* 特定フレーム以上前のタッチデータは消す */
				base.RemoveAt(0);
			}
		}
		
		public MyTouchData getCurrentFrameData()
		{
			return base[base.Count - 1];
		}
		
		public MyTouchData getPrevFrameData()
		{
			return base[base.Count - 2];
		}

		public void Update(List<TouchData> touchDataList)
		{
			if(touchDataList.Count == 0)
			{
				/* タッチがなかったとき */
				var nonTouchData = new MyTouchData();
				nonTouchData.Status = MyTouchStatus.None;
				Add(nonTouchData);
			}
			
			foreach(TouchData touchData in touchDataList)
			{
				if(touchData.ID != 0)
				{
					/* 指1本だけ対応 */
					continue;
				}
				var myTouchData = new MyTouchData();
				myTouchData.ConvertTouchData(touchData);
				Add(myTouchData);
			}
		}
		
		private bool IsContainStatus(MyTouchStatus status)
		{
			bool isContain = false;
			for(int i = 0; i < base.Count && !isContain; i++)
			{
				isContain |= base[i].Status == status;
			}
			return isContain;
		}
		
	}
}

