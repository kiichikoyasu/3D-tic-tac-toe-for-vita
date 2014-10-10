using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Graphics;

using System.Collections.Generic;


namespace Dtictactoe
{
	public class Camera
	{
		private Matrix4 view, proj, world;
		public static Matrix4 worldViewProj;
		private static Vector3 eye, center, up;
		private GraphicsContext gc;
		private float aspect, fov, near, far;
		
		private float epsilon = 0.01f;

		/* 前のフレームでタッチしたところを覚えておく */
		private Vector2 prevPoint;

		public Camera (GraphicsContext graphics)
		{
			this.gc = graphics;
		}
		
		public void Initialize()
		{
			aspect = gc.Screen.AspectRatio;
			fov = FMath.Radians(45.0f);
			near = 1.0f;
			far = 1000.0f;
			
			proj = Matrix4.Perspective(fov, aspect, near, far);
			
			eye = new Vector3(0.0f, 0.0f, 10.0f);
			center = new Vector3(0.0f, 0.0f, 0.0f);
			up = Vector3.UnitY;

			/*CalcCameraPosメソッドにおいて注視点が原点じゃない時でも動くかテスト用*/
/*			eye = new Vector3(0.0f, 0.0f, 10.0f);
			center = new Vector3(2.0f, 0.0f, 0.0f);
			cameraUp = new Vector3(0.0f, 1.0f, 0.0f);*/
			
			world = Matrix4.Identity;
			
			prevPoint = new Vector2(0.0f, 0.0f);
		}
		
		public void Update(GamePadData gamePadData, List<TouchData> touchDatalist)
		{			
			Vector2 inputVector;			
			
			if((gamePadData.ButtonsDown & GamePadButtons.Right) != 0){
				CalcPos(new Vector2(1.0f, 0.0f));
			}
			
			if((gamePadData.ButtonsDown & GamePadButtons.Left) != 0){
				CalcPos(new Vector2(-1.0f, 0.0f));
			}
			
			if((gamePadData.ButtonsDown & GamePadButtons.Up) != 0){
				CalcPos(new Vector2(0.0f, 1.0f));
			}

			if((gamePadData.ButtonsDown & GamePadButtons.Down) != 0){
				CalcPos(new Vector2(0.0f, -1.0f));
			}
			
			inputVector = new Vector2(gamePadData.AnalogLeftX, -gamePadData.AnalogLeftY);
			if(inputVector.Length() > epsilon)
			{
				inputVector = inputVector.Normalize();
				CalcPos(inputVector);
			}
			
			
			foreach(TouchData touchData in touchDatalist)
			{
				if(touchData.Status == TouchStatus.Down)
				{
					prevPoint = new Vector2(touchData.X, -touchData.Y);
				}
				
				if(touchData.Status == TouchStatus.Move)
				{
					var currentPoint = new Vector2(touchData.X, -touchData.Y);
					inputVector = Vector2.Subtract(currentPoint, prevPoint);
					/* touchはスティックに比べて値が小さいのでepsilonの調整余地あり */
					if(inputVector.Length() > epsilon)
					{
						inputVector = inputVector.Normalize();
						CalcPos(inputVector);
					}
					prevPoint = currentPoint;
				}
			}
			
			view = Matrix4.LookAt(eye, center, up);
			
			worldViewProj = proj * view * world;
			
		}
		
/*		public Matrix4 WorldViewProj{
			set{worldViewProj = value;}
			get{return worldViewProj;}
		}*/
		
		public static Vector3 Eye
		{
			get{return eye;}
		}
		
		/* カメラの注視点と注視点との距離を変えずにカメラの位置を移動するメソッド 
		 * 画面上の入力と直観的に画面が動く
		 * カメラは注視点を中心とした球面上を動く*/
		private void CalcPos(Vector2 input2D){
			
			/*
			 * カメラと注視点の距離
			 * 計算誤差によって距離が変わらないように正規化してdistanceをかける*/
			float distance = (eye - center).Length();
			
			//画面の入力ベクトルと画面上の上ベクトルとの角度を出す
			Vector2 upScreen = new Vector2(0.0f, 1.0f);
			float inputAngle = upScreen.Angle(input2D);
			
			//その角度分upベクトルを目線ベクトルを軸に回転させる（これが画面の中の世界での入力ベクトルになる）
			Quaternion upQ = new Quaternion (up, 0.0f);
			Quaternion rotationQ = Quaternion.RotationAxis(eye.Negate(), inputAngle);
			Quaternion inputQ = rotationQ.Conjugate().Multiply(upQ).Multiply(rotationQ);
			
			float rotateAngle = input2D.Length() / 10.0f;
			//入力ベクトルと目線ベクトルの法線を軸にカメラ位置とupベクトルを回転させる
			Vector3 input3D = new Vector3(inputQ.X, inputQ.Y, inputQ.Z);
			Vector3 rotationAxis = input3D.Cross(eye.Negate());
			rotationQ = Quaternion.RotationAxis(rotationAxis, rotateAngle);
			
			//カメラの注視点が原点でない場合は回転させてから戻す
			eye = eye.Subtract(center);
//			up = up.Subtract(center);
			Quaternion eyePositionQ = new Quaternion(eye, 0.0f);
			upQ = new Quaternion(up, 0.0f);
			eyePositionQ = rotationQ.Conjugate().Multiply(eyePositionQ).Multiply(rotationQ);
			upQ = rotationQ.Conjugate().Multiply(upQ).Multiply(rotationQ);
			
			eye = new Vector3(eyePositionQ.X, eyePositionQ.Y, eyePositionQ.Z);
			up = new Vector3(upQ.X, upQ.Y, upQ.Z);
			
			eye = eye.Add (center);
			
			//距離調整
			eye = eye.Normalize().Multiply(distance);
//			up = up.Add(center);
		}

	}
}

