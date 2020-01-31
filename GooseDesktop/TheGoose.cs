using SamEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace GooseDesktop
{
	internal static class TheGoose
	{
		private static Vector2 position;

		private static Vector2 velocity;

		private static float direction;

		private static Vector2 targetDirection;

		private static bool overrideExtendNeck;

		private const TheGoose.GooseTask FirstUX_FirstTask = TheGoose.GooseTask.TrackMud;

		private const TheGoose.GooseTask FirstUX_SecondTask = TheGoose.GooseTask.CollectWindow_Meme;

		private static Vector2 targetPos;

		private static float targetDir;

		private static float currentSpeed;

		private static float currentAcceleration;

		private static float stepTime;

		private const float WalkSpeed = 80f;

		private const float RunSpeed = 200f;

		private const float ChargeSpeed = 400f;

		private const float turnSpeed = 120f;

		private const float AccelerationNormal = 1300f;

		private const float AccelerationCharged = 2300f;

		private const float StopRadius = -10f;

		private const float StepTimeNormal = 0.2f;

		private const float StepTimeCharged = 0.1f;

		private static float trackMudEndTime;

		private const float DurationToTrackMud = 15f;

		private static Pen DrawingPen;

		private static Bitmap shadowBitmap;

		private static TextureBrush shadowBrush;

		private static Pen shadowPen;

		private static FootMark[] footMarks;

		private static int footMarkIndex;

		private static bool lastFrameMouseButtonPressed;

		private static TheGoose.GooseTask currentTask;

		private static TheGoose.Task_Wander taskWanderInfo;

		private static TheGoose.Task_NabMouse taskNabMouseInfo;

		private static Rectangle tmpRect;

		private static Size tmpSize;

		private static bool hasAskedForDonation;

		private static TheGoose.Task_CollectWindow taskCollectWindowInfo;

		private static TheGoose.Task_TrackMud taskTrackMudInfo;

		private static TheGoose.GooseTask[] gooseTaskWeightedList;

		private static Deck taskPickerDeck;

		private static Vector2 lFootPos;

		private static Vector2 rFootPos;

		private static float lFootMoveTimeStart;

		private static float rFootMoveTimeStart;

		private static Vector2 lFootMoveOrigin;

		private static Vector2 rFootMoveOrigin;

		private static Vector2 lFootMoveDir;

		private static Vector2 rFootMoveDir;

		private const float wantStepAtDistance = 5f;

		private const int feetDistanceApart = 6;

		private const float overshootFraction = 0.4f;

		private static TheGoose.Rig gooseRig;

		static TheGoose()
		{
			TheGoose.position = new Vector2(300f, 300f);
			TheGoose.velocity = new Vector2(0f, 0f);
			TheGoose.direction = 90f;
			TheGoose.targetPos = new Vector2(300f, 300f);
			TheGoose.targetDir = 90f;
			TheGoose.currentSpeed = 80f;
			TheGoose.currentAcceleration = 1300f;
			TheGoose.stepTime = 0.2f;
			TheGoose.trackMudEndTime = -1f;
			TheGoose.footMarks = new FootMark[64];
			TheGoose.footMarkIndex = 0;
			TheGoose.lastFrameMouseButtonPressed = false;
			TheGoose.tmpRect = new Rectangle();
			TheGoose.tmpSize = new Size();
			TheGoose.hasAskedForDonation = false;
			TheGoose.gooseTaskWeightedList = new TheGoose.GooseTask[] { typeof(<PrivateImplementationDetails>).GetField("B21070F238393EF14283880FC38D1F9F08A78F04").FieldHandle };
			TheGoose.taskPickerDeck = new Deck((int)TheGoose.gooseTaskWeightedList.Length);
			TheGoose.lFootMoveTimeStart = -1f;
			TheGoose.rFootMoveTimeStart = -1f;
		}

		private static void AddFootMark(Vector2 markPos)
		{
			TheGoose.footMarks[TheGoose.footMarkIndex].time = Time.time;
			TheGoose.footMarks[TheGoose.footMarkIndex].position = markPos;
			TheGoose.footMarkIndex++;
			if (TheGoose.footMarkIndex >= (int)TheGoose.footMarks.Length)
			{
				TheGoose.footMarkIndex = 0;
			}
		}

		private static void ChooseNextTask()
		{
			if (!GooseConfig.settings.CanAttackAtRandom && Time.time < GooseConfig.settings.FirstWanderTimeSeconds + 1f)
			{
				TheGoose.SetTask(TheGoose.GooseTask.TrackMud);
				return;
			}
			if (Time.time > 8f * 60f && !TheGoose.hasAskedForDonation)
			{
				TheGoose.hasAskedForDonation = true;
				TheGoose.SetTask(TheGoose.GooseTask.CollectWindow_Donate);
				return;
			}
			TheGoose.GooseTask gooseTask = TheGoose.gooseTaskWeightedList[TheGoose.taskPickerDeck.Next()];
			while (!GooseConfig.settings.CanAttackAtRandom && gooseTask == TheGoose.GooseTask.NabMouse)
			{
				gooseTask = TheGoose.gooseTaskWeightedList[TheGoose.taskPickerDeck.Next()];
			}
			TheGoose.SetTask(gooseTask);
		}

		private static void CollectMemeTask_CancelEarly(object sender, FormClosingEventArgs args)
		{
			TheGoose.SetTask(TheGoose.GooseTask.NabMouse);
		}

		public static void FillCircleFromCenter(Graphics g, Brush brush, Vector2 pos, int radius)
		{
			TheGoose.FillEllipseFromCenter(g, brush, (int)pos.x, (int)pos.y, radius, radius);
		}

		public static void FillCircleFromCenter(Graphics g, Brush brush, int x, int y, int radius)
		{
			TheGoose.FillEllipseFromCenter(g, brush, x, y, radius, radius);
		}

		public static void FillEllipseFromCenter(Graphics g, Brush brush, int x, int y, int xRadius, int yRadius)
		{
			g.FillEllipse(brush, x - xRadius, y - yRadius, xRadius * 2, yRadius * 2);
		}

		public static void FillEllipseFromCenter(Graphics g, Brush brush, Vector2 position, Vector2 xyRadius)
		{
			g.FillEllipse(brush, position.x - xyRadius.x, position.y - xyRadius.y, xyRadius.x * 2f, xyRadius.y * 2f);
		}

		private static Vector2 GetFootHome(bool rightFoot)
		{
			object obj;
			if (rightFoot)
			{
				obj = 1;
			}
			else
			{
				obj = null;
			}
			float single = (float)obj;
			Vector2 fromAngleDegrees = Vector2.GetFromAngleDegrees(TheGoose.direction + 90f) * single;
			return TheGoose.position + (fromAngleDegrees * 6f);
		}

		public static void Init()
		{
			TheGoose.position = new Vector2(-20f, 120f);
			TheGoose.targetPos = new Vector2(100f, 150f);
			if (!GooseConfig.settings.CanAttackAtRandom)
			{
				int num = Array.IndexOf<int>(TheGoose.taskPickerDeck.indices, Array.IndexOf<TheGoose.GooseTask>(TheGoose.gooseTaskWeightedList, TheGoose.GooseTask.CollectWindow_Meme));
				int num1 = TheGoose.taskPickerDeck.indices[0];
				TheGoose.taskPickerDeck.indices[0] = TheGoose.taskPickerDeck.indices[num];
				TheGoose.taskPickerDeck.indices[num] = num1;
			}
			TheGoose.lFootPos = TheGoose.GetFootHome(false);
			TheGoose.rFootPos = TheGoose.GetFootHome(true);
			TheGoose.shadowBitmap = new Bitmap(2, 2);
			TheGoose.shadowBitmap.SetPixel(0, 0, Color.Transparent);
			TheGoose.shadowBitmap.SetPixel(1, 1, Color.Transparent);
			TheGoose.shadowBitmap.SetPixel(1, 0, Color.Transparent);
			TheGoose.shadowBitmap.SetPixel(0, 1, Color.DarkGray);
			TheGoose.shadowBrush = new TextureBrush(TheGoose.shadowBitmap);
			TheGoose.shadowPen = new Pen(TheGoose.shadowBrush);
			Pen pen = TheGoose.shadowPen;
			int num2 = 2;
			LineCap lineCap = (LineCap)num2;
			TheGoose.shadowPen.EndCap = (LineCap)num2;
			pen.StartCap = lineCap;
			TheGoose.DrawingPen = new Pen(Brushes.White);
			Pen drawingPen = TheGoose.DrawingPen;
			int num3 = 2;
			lineCap = (LineCap)num3;
			TheGoose.DrawingPen.StartCap = (LineCap)num3;
			drawingPen.EndCap = lineCap;
			TheGoose.SetTask(TheGoose.GooseTask.Wander);
		}

		public static void Render(Graphics g)
		{
			for (int i = 0; i < (int)TheGoose.footMarks.Length; i++)
			{
				if (TheGoose.footMarks[i].time != 0f)
				{
					float single = TheGoose.footMarks[i].time + 8.5f;
					float single1 = SamMath.Clamp(Time.time - single, 0f, 1f) / 1f;
					float single2 = SamMath.Lerp(3f, 0f, single1);
					TheGoose.FillCircleFromCenter(g, Brushes.SaddleBrown, TheGoose.footMarks[i].position, (int)single2);
				}
			}
			TheGoose.UpdateRig();
			float single3 = TheGoose.direction;
			int num = (int)TheGoose.position.x;
			int num1 = (int)TheGoose.position.y;
			Vector2 vector2 = new Vector2((float)num, (float)num1);
			Vector2 vector21 = new Vector2(1.3f, 0.4f);
			Vector2 fromAngleDegrees = Vector2.GetFromAngleDegrees(single3);
			Vector2 fromAngleDegrees1 = Vector2.GetFromAngleDegrees(single3 + 90f);
			Vector2 vector22 = new Vector2(0f, -1f);
			int num2 = 2;
			TheGoose.DrawingPen.Brush = Brushes.White;
			TheGoose.FillCircleFromCenter(g, Brushes.Orange, TheGoose.lFootPos, 4);
			TheGoose.FillCircleFromCenter(g, Brushes.Orange, TheGoose.rFootPos, 4);
			TheGoose.FillEllipseFromCenter(g, TheGoose.shadowBrush, (int)vector2.x, (int)vector2.y, 20, 15);
			TheGoose.DrawingPen.Color = Color.LightGray;
			TheGoose.DrawingPen.Width = (float)(22 + num2);
			g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.bodyCenter + (fromAngleDegrees * 11f)), TheGoose.ToIntPoint(TheGoose.gooseRig.bodyCenter - (fromAngleDegrees * 11f)));
			TheGoose.DrawingPen.Width = (float)(13 + num2);
			g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.neckBase), TheGoose.ToIntPoint(TheGoose.gooseRig.neckHeadPoint));
			TheGoose.DrawingPen.Width = (float)(15 + num2);
			g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.neckHeadPoint), TheGoose.ToIntPoint(TheGoose.gooseRig.head1EndPoint));
			TheGoose.DrawingPen.Width = (float)(10 + num2);
			g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.head1EndPoint), TheGoose.ToIntPoint(TheGoose.gooseRig.head2EndPoint));
			TheGoose.DrawingPen.Color = Color.LightGray;
			TheGoose.DrawingPen.Width = 15f;
			g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.underbodyCenter + (fromAngleDegrees * 7f)), TheGoose.ToIntPoint(TheGoose.gooseRig.underbodyCenter - (fromAngleDegrees * 7f)));
			TheGoose.DrawingPen.Color = Color.White;
			TheGoose.DrawingPen.Width = 22f;
			g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.bodyCenter + (fromAngleDegrees * 11f)), TheGoose.ToIntPoint(TheGoose.gooseRig.bodyCenter - (fromAngleDegrees * 11f)));
			TheGoose.DrawingPen.Width = 13f;
			g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.neckBase), TheGoose.ToIntPoint(TheGoose.gooseRig.neckHeadPoint));
			TheGoose.DrawingPen.Width = 15f;
			g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.neckHeadPoint), TheGoose.ToIntPoint(TheGoose.gooseRig.head1EndPoint));
			TheGoose.DrawingPen.Width = 10f;
			g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.head1EndPoint), TheGoose.ToIntPoint(TheGoose.gooseRig.head2EndPoint));
			int num3 = 9;
			int num4 = 3;
			TheGoose.DrawingPen.Width = (float)num3;
			TheGoose.DrawingPen.Brush = Brushes.Orange;
			Vector2 vector23 = TheGoose.gooseRig.head2EndPoint + (fromAngleDegrees * (float)num4);
			g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.head2EndPoint), TheGoose.ToIntPoint(vector23));
			Vector2 vector24 = ((TheGoose.gooseRig.neckHeadPoint + (vector22 * 3f)) + ((-fromAngleDegrees1 * vector21) * 5f)) + (fromAngleDegrees * 5f);
			Vector2 vector25 = ((TheGoose.gooseRig.neckHeadPoint + (vector22 * 3f)) + ((fromAngleDegrees1 * vector21) * 5f)) + (fromAngleDegrees * 5f);
			TheGoose.FillCircleFromCenter(g, Brushes.Black, vector24, 2);
			TheGoose.FillCircleFromCenter(g, Brushes.Black, vector25, 2);
		}

		private static void RunAI()
		{
			switch (TheGoose.currentTask)
			{
				case TheGoose.GooseTask.Wander:
				{
					TheGoose.RunWander();
					return;
				}
				case TheGoose.GooseTask.NabMouse:
				{
					TheGoose.RunNabMouse();
					return;
				}
				case TheGoose.GooseTask.CollectWindow_Meme:
				case TheGoose.GooseTask.CollectWindow_Notepad:
				case TheGoose.GooseTask.CollectWindow_Donate:
				{
					return;
				}
				case TheGoose.GooseTask.CollectWindow_DONOTSET:
				{
					TheGoose.RunCollectWindow();
					return;
				}
				case TheGoose.GooseTask.TrackMud:
				{
					TheGoose.RunTrackMud();
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private static void RunCollectWindow()
		{
			switch (TheGoose.taskCollectWindowInfo.stage)
			{
				case TheGoose.Task_CollectWindow.Stage.WalkingOffscreen:
				{
					if (Vector2.Distance(TheGoose.position, TheGoose.targetPos) >= 5f)
					{
						break;
					}
					TheGoose.taskCollectWindowInfo.secsToWait = TheGoose.Task_CollectWindow.GetWaitTime();
					TheGoose.taskCollectWindowInfo.waitStartTime = Time.time;
					TheGoose.taskCollectWindowInfo.stage = TheGoose.Task_CollectWindow.Stage.WaitingToBringWindowBack;
					return;
				}
				case TheGoose.Task_CollectWindow.Stage.WaitingToBringWindowBack:
				{
					if (Time.time - TheGoose.taskCollectWindowInfo.waitStartTime <= TheGoose.taskCollectWindowInfo.secsToWait)
					{
						break;
					}
					TheGoose.taskCollectWindowInfo.mainForm.FormClosing += new FormClosingEventHandler(TheGoose.CollectMemeTask_CancelEarly);
					(new Thread(() => TheGoose.taskCollectWindowInfo.mainForm.ShowDialog())).Start();
					switch (TheGoose.taskCollectWindowInfo.screenDirection)
					{
						case TheGoose.Task_CollectWindow.ScreenDirection.Left:
						{
							TheGoose.targetPos.y = SamMath.Lerp(TheGoose.position.y, (float)(Program.mainForm.Height / 2), SamMath.RandomRange(0.2f, 0.3f));
							TheGoose.targetPos.x = (float)TheGoose.taskCollectWindowInfo.mainForm.Width + SamMath.RandomRange(15f, 20f);
							break;
						}
						case TheGoose.Task_CollectWindow.ScreenDirection.Top:
						{
							TheGoose.targetPos.y = (float)TheGoose.taskCollectWindowInfo.mainForm.Height + SamMath.RandomRange(80f, 100f);
							TheGoose.targetPos.x = SamMath.Lerp(TheGoose.position.x, (float)(Program.mainForm.Width / 2), SamMath.RandomRange(0.2f, 0.3f));
							break;
						}
						case TheGoose.Task_CollectWindow.ScreenDirection.Right:
						{
							TheGoose.targetPos.y = SamMath.Lerp(TheGoose.position.y, (float)(Program.mainForm.Height / 2), SamMath.RandomRange(0.2f, 0.3f));
							TheGoose.targetPos.x = (float)Program.mainForm.Width - ((float)TheGoose.taskCollectWindowInfo.mainForm.Width + SamMath.RandomRange(20f, 30f));
							break;
						}
					}
					TheGoose.targetPos.x = SamMath.Clamp(TheGoose.targetPos.x, (float)(TheGoose.taskCollectWindowInfo.mainForm.Width + 55), (float)(Program.mainForm.Width - (TheGoose.taskCollectWindowInfo.mainForm.Width + 55)));
					TheGoose.targetPos.y = SamMath.Clamp(TheGoose.targetPos.y, (float)(TheGoose.taskCollectWindowInfo.mainForm.Height + 80), (float)Program.mainForm.Height);
					TheGoose.taskCollectWindowInfo.stage = TheGoose.Task_CollectWindow.Stage.DraggingWindowBack;
					return;
				}
				case TheGoose.Task_CollectWindow.Stage.DraggingWindowBack:
				{
					if (Vector2.Distance(TheGoose.position, TheGoose.targetPos) < 5f)
					{
						TheGoose.targetPos = TheGoose.position + (Vector2.GetFromAngleDegrees(TheGoose.direction + 180f) * 40f);
						TheGoose.SetTask(TheGoose.GooseTask.Wander);
						return;
					}
					TheGoose.overrideExtendNeck = true;
					TheGoose.targetDirection = TheGoose.position - TheGoose.targetPos;
					TheGoose.taskCollectWindowInfo.mainForm.SetWindowPositionThreadsafe(TheGoose.ToIntPoint(TheGoose.gooseRig.head2EndPoint - TheGoose.taskCollectWindowInfo.windowOffsetToBeak));
					break;
				}
				default:
				{
					return;
				}
			}
		}

		private static void RunNabMouse()
		{
			Vector2 vector2 = new Vector2((float)Cursor.Position.X, (float)Cursor.Position.Y);
			Vector2 vector21 = TheGoose.gooseRig.head2EndPoint;
			if (TheGoose.taskNabMouseInfo.currentStage == TheGoose.Task_NabMouse.Stage.SeekingMouse)
			{
				TheGoose.SetSpeed(TheGoose.SpeedTiers.Charge);
				TheGoose.targetPos = vector2 - (TheGoose.gooseRig.head2EndPoint - TheGoose.position);
				if (Vector2.Distance(vector21, vector2) < 15f)
				{
					TheGoose.taskNabMouseInfo.originalVectorToMouse = vector2 - vector21;
					TheGoose.taskNabMouseInfo.grabbedOriginalTime = Time.time;
					TheGoose.taskNabMouseInfo.dragToPoint = TheGoose.position;
					while (Vector2.Distance(TheGoose.taskNabMouseInfo.dragToPoint, TheGoose.position) / 400f < 1.2f)
					{
						TheGoose.taskNabMouseInfo.dragToPoint = new Vector2((float)SamMath.Rand.NextDouble() * (float)Program.mainForm.Width, (float)SamMath.Rand.NextDouble() * (float)Program.mainForm.Height);
					}
					TheGoose.targetPos = TheGoose.taskNabMouseInfo.dragToPoint;
					TheGoose.SetForegroundWindow(Program.mainForm.Handle);
					Sound.CHOMP();
					TheGoose.taskNabMouseInfo.currentStage = TheGoose.Task_NabMouse.Stage.DraggingMouseAway;
				}
				if (Time.time > TheGoose.taskNabMouseInfo.chaseStartTime + 9f)
				{
					TheGoose.taskNabMouseInfo.currentStage = TheGoose.Task_NabMouse.Stage.Decelerating;
				}
			}
			if (TheGoose.taskNabMouseInfo.currentStage == TheGoose.Task_NabMouse.Stage.DraggingMouseAway)
			{
				if (Vector2.Distance(TheGoose.position, TheGoose.targetPos) >= 30f)
				{
					float single = Math.Min((Time.time - TheGoose.taskNabMouseInfo.grabbedOriginalTime) / 0.06f, 1f);
					Vector2 vector22 = Vector2.Lerp(TheGoose.taskNabMouseInfo.originalVectorToMouse, TheGoose.Task_NabMouse.StruggleRange, single);
					Vector2 vector23 = new Vector2()
					{
						x = (vector22.x < 0f ? vector21.x + vector22.x : vector21.x),
						y = (vector22.y < 0f ? vector21.y + vector22.y : vector21.y)
					};
					TheGoose.tmpRect.Location = TheGoose.ToIntPoint(vector23);
					TheGoose.tmpSize.Width = Math.Abs((int)vector22.x);
					TheGoose.tmpSize.Height = Math.Abs((int)vector22.y);
					TheGoose.tmpRect.Size = TheGoose.tmpSize;
					Cursor.Clip = TheGoose.tmpRect;
				}
				else
				{
					Cursor.Clip = Rectangle.Empty;
					TheGoose.taskNabMouseInfo.currentStage = TheGoose.Task_NabMouse.Stage.Decelerating;
				}
			}
			if (TheGoose.taskNabMouseInfo.currentStage == TheGoose.Task_NabMouse.Stage.Decelerating)
			{
				TheGoose.targetPos = TheGoose.position + (Vector2.Normalize(TheGoose.velocity) * 5f);
				TheGoose.velocity = TheGoose.velocity - (((Vector2.Normalize(TheGoose.velocity) * TheGoose.currentAcceleration) * 2f) * 0.008333334f);
				if (Vector2.Magnitude(TheGoose.velocity) < 80f)
				{
					TheGoose.SetTask(TheGoose.GooseTask.Wander);
				}
			}
		}

		private static void RunTrackMud()
		{
			switch (TheGoose.taskTrackMudInfo.stage)
			{
				case TheGoose.Task_TrackMud.Stage.DecideToRun:
				{
					TheGoose.SetTargetOffscreen(false);
					TheGoose.SetSpeed(TheGoose.SpeedTiers.Run);
					TheGoose.taskTrackMudInfo.stage = TheGoose.Task_TrackMud.Stage.RunningOffscreen;
					return;
				}
				case TheGoose.Task_TrackMud.Stage.RunningOffscreen:
				{
					if (Vector2.Distance(TheGoose.position, TheGoose.targetPos) >= 5f)
					{
						break;
					}
					TheGoose.targetPos = new Vector2(SamMath.RandomRange(0f, (float)Program.mainForm.Width), SamMath.RandomRange(0f, (float)Program.mainForm.Height));
					TheGoose.taskTrackMudInfo.nextDirChangeTime = Time.time + TheGoose.Task_TrackMud.GetDirChangeInterval();
					TheGoose.taskTrackMudInfo.timeToStopRunning = Time.time + 2f;
					TheGoose.trackMudEndTime = Time.time + 15f;
					TheGoose.taskTrackMudInfo.stage = TheGoose.Task_TrackMud.Stage.RunningWandering;
					Sound.PlayMudSquith();
					return;
				}
				case TheGoose.Task_TrackMud.Stage.RunningWandering:
				{
					if (Vector2.Distance(TheGoose.position, TheGoose.targetPos) < 5f || Time.time > TheGoose.taskTrackMudInfo.nextDirChangeTime)
					{
						TheGoose.targetPos = new Vector2(SamMath.RandomRange(0f, (float)Program.mainForm.Width), SamMath.RandomRange(0f, (float)Program.mainForm.Height));
						TheGoose.taskTrackMudInfo.nextDirChangeTime = Time.time + TheGoose.Task_TrackMud.GetDirChangeInterval();
					}
					if (Time.time <= TheGoose.taskTrackMudInfo.timeToStopRunning)
					{
						break;
					}
					TheGoose.targetPos = TheGoose.position + new Vector2(30f, 3f);
					TheGoose.targetPos.x = SamMath.Clamp(TheGoose.targetPos.x, 55f, (float)(Program.mainForm.Width - 55));
					TheGoose.targetPos.y = SamMath.Clamp(TheGoose.targetPos.y, 80f, (float)(Program.mainForm.Height - 80));
					TheGoose.SetTask(TheGoose.GooseTask.Wander, false);
					break;
				}
				default:
				{
					return;
				}
			}
		}

		private static void RunWander()
		{
			if (Time.time - TheGoose.taskWanderInfo.wanderingStartTime > TheGoose.taskWanderInfo.wanderingDuration)
			{
				TheGoose.ChooseNextTask();
				return;
			}
			if (TheGoose.taskWanderInfo.pauseStartTime <= 0f)
			{
				if (Vector2.Distance(TheGoose.position, TheGoose.targetPos) >= 20f)
				{
					return;
				}
				TheGoose.taskWanderInfo.pauseStartTime = Time.time;
				TheGoose.taskWanderInfo.pauseDuration = TheGoose.Task_Wander.GetRandomPauseDuration();
				return;
			}
			if (Time.time - TheGoose.taskWanderInfo.pauseStartTime <= TheGoose.taskWanderInfo.pauseDuration)
			{
				TheGoose.velocity = Vector2.zero;
				return;
			}
			TheGoose.taskWanderInfo.pauseStartTime = -1f;
			float randomWalkTime = TheGoose.Task_Wander.GetRandomWalkTime() * TheGoose.currentSpeed;
			TheGoose.targetPos = new Vector2(SamMath.RandomRange(0f, (float)Program.mainForm.Width), SamMath.RandomRange(0f, (float)Program.mainForm.Height));
			if (Vector2.Distance(TheGoose.position, TheGoose.targetPos) > randomWalkTime)
			{
				TheGoose.targetPos = TheGoose.position + (Vector2.Normalize(TheGoose.targetPos - TheGoose.position) * randomWalkTime);
			}
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		private static void SetSpeed(TheGoose.SpeedTiers tier)
		{
			switch (tier)
			{
				case TheGoose.SpeedTiers.Walk:
				{
					TheGoose.currentSpeed = 80f;
					TheGoose.currentAcceleration = 1300f;
					TheGoose.stepTime = 0.2f;
					return;
				}
				case TheGoose.SpeedTiers.Run:
				{
					TheGoose.currentSpeed = 200f;
					TheGoose.currentAcceleration = 1300f;
					TheGoose.stepTime = 0.2f;
					return;
				}
				case TheGoose.SpeedTiers.Charge:
				{
					TheGoose.currentSpeed = 400f;
					TheGoose.currentAcceleration = 2300f;
					TheGoose.stepTime = 0.1f;
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private static TheGoose.Task_CollectWindow.ScreenDirection SetTargetOffscreen(bool canExitTop = false)
		{
			int width = (int)TheGoose.position.x;
			TheGoose.Task_CollectWindow.ScreenDirection screenDirection = TheGoose.Task_CollectWindow.ScreenDirection.Left;
			TheGoose.targetPos = new Vector2(-50f, SamMath.Lerp(TheGoose.position.y, (float)(Program.mainForm.Height / 2), 0.4f));
			if (width > Program.mainForm.Width / 2)
			{
				width = Program.mainForm.Width - (int)TheGoose.position.x;
				screenDirection = TheGoose.Task_CollectWindow.ScreenDirection.Right;
				TheGoose.targetPos = new Vector2((float)(Program.mainForm.Width + 50), SamMath.Lerp(TheGoose.position.y, (float)(Program.mainForm.Height / 2), 0.4f));
			}
			if (canExitTop && (float)width > TheGoose.position.y)
			{
				screenDirection = TheGoose.Task_CollectWindow.ScreenDirection.Top;
				TheGoose.targetPos = new Vector2(SamMath.Lerp(TheGoose.position.x, (float)(Program.mainForm.Width / 2), 0.4f), -50f);
			}
			return screenDirection;
		}

		private static void SetTask(TheGoose.GooseTask task)
		{
			TheGoose.SetTask(task, true);
		}

		private static void SetTask(TheGoose.GooseTask task, bool honck)
		{
			if (honck)
			{
				Sound.HONCC();
			}
			TheGoose.currentTask = task;
			switch (task)
			{
				case TheGoose.GooseTask.Wander:
				{
					TheGoose.SetSpeed(TheGoose.SpeedTiers.Walk);
					TheGoose.taskWanderInfo = new TheGoose.Task_Wander()
					{
						pauseStartTime = -1f,
						wanderingStartTime = Time.time,
						wanderingDuration = TheGoose.Task_Wander.GetRandomWanderDuration()
					};
					return;
				}
				case TheGoose.GooseTask.NabMouse:
				{
					TheGoose.taskNabMouseInfo = new TheGoose.Task_NabMouse()
					{
						chaseStartTime = Time.time
					};
					return;
				}
				case TheGoose.GooseTask.CollectWindow_Meme:
				{
					TheGoose.taskCollectWindowInfo = new TheGoose.Task_CollectWindow()
					{
						mainForm = new TheGoose.SimpleImageForm()
					};
					TheGoose.SetTask(TheGoose.GooseTask.CollectWindow_DONOTSET, false);
					return;
				}
				case TheGoose.GooseTask.CollectWindow_Notepad:
				{
					TheGoose.taskCollectWindowInfo = new TheGoose.Task_CollectWindow()
					{
						mainForm = new TheGoose.SimpleTextForm()
					};
					TheGoose.SetTask(TheGoose.GooseTask.CollectWindow_DONOTSET, false);
					return;
				}
				case TheGoose.GooseTask.CollectWindow_Donate:
				{
					TheGoose.taskCollectWindowInfo = new TheGoose.Task_CollectWindow()
					{
						mainForm = new TheGoose.SimpleDonateForm()
					};
					TheGoose.SetTask(TheGoose.GooseTask.CollectWindow_DONOTSET, false);
					return;
				}
				case TheGoose.GooseTask.CollectWindow_DONOTSET:
				{
					TheGoose.taskCollectWindowInfo.screenDirection = TheGoose.SetTargetOffscreen(false);
					switch (TheGoose.taskCollectWindowInfo.screenDirection)
					{
						case TheGoose.Task_CollectWindow.ScreenDirection.Left:
						{
							TheGoose.taskCollectWindowInfo.windowOffsetToBeak = new Vector2((float)TheGoose.taskCollectWindowInfo.mainForm.Width, (float)(TheGoose.taskCollectWindowInfo.mainForm.Height / 2));
							return;
						}
						case TheGoose.Task_CollectWindow.ScreenDirection.Top:
						{
							TheGoose.taskCollectWindowInfo.windowOffsetToBeak = new Vector2((float)(TheGoose.taskCollectWindowInfo.mainForm.Width / 2), (float)TheGoose.taskCollectWindowInfo.mainForm.Height);
							return;
						}
						case TheGoose.Task_CollectWindow.ScreenDirection.Right:
						{
							TheGoose.taskCollectWindowInfo.windowOffsetToBeak = new Vector2(0f, (float)(TheGoose.taskCollectWindowInfo.mainForm.Height / 2));
							return;
						}
						default:
						{
							return;
						}
					}
					break;
				}
				case TheGoose.GooseTask.TrackMud:
				{
					TheGoose.taskTrackMudInfo = new TheGoose.Task_TrackMud();
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private static void SolveFeet()
		{
			Vector2.GetFromAngleDegrees(TheGoose.direction);
			Vector2.GetFromAngleDegrees(TheGoose.direction + 90f);
			Vector2 footHome = TheGoose.GetFootHome(false);
			Vector2 vector2 = TheGoose.GetFootHome(true);
			if (TheGoose.lFootMoveTimeStart < 0f && TheGoose.rFootMoveTimeStart < 0f)
			{
				if (Vector2.Distance(TheGoose.lFootPos, footHome) > 5f)
				{
					TheGoose.lFootMoveOrigin = TheGoose.lFootPos;
					TheGoose.lFootMoveDir = Vector2.Normalize(footHome - TheGoose.lFootPos);
					TheGoose.lFootMoveTimeStart = Time.time;
					return;
				}
				if (Vector2.Distance(TheGoose.rFootPos, vector2) > 5f)
				{
					TheGoose.rFootMoveOrigin = TheGoose.rFootPos;
					TheGoose.rFootMoveDir = Vector2.Normalize(vector2 - TheGoose.rFootPos);
					TheGoose.rFootMoveTimeStart = Time.time;
					return;
				}
			}
			else if (TheGoose.lFootMoveTimeStart > 0f)
			{
				Vector2 vector21 = footHome + ((TheGoose.lFootMoveDir * 0.4f) * 5f);
				if (Time.time <= TheGoose.lFootMoveTimeStart + TheGoose.stepTime)
				{
					float single = (Time.time - TheGoose.lFootMoveTimeStart) / TheGoose.stepTime;
					TheGoose.lFootPos = Vector2.Lerp(TheGoose.lFootMoveOrigin, vector21, Easings.CubicEaseInOut(single));
					return;
				}
				TheGoose.lFootPos = vector21;
				TheGoose.lFootMoveTimeStart = -1f;
				Sound.PlayPat();
				if (Time.time < TheGoose.trackMudEndTime)
				{
					TheGoose.AddFootMark(TheGoose.lFootPos);
					return;
				}
			}
			else if (TheGoose.rFootMoveTimeStart > 0f)
			{
				Vector2 vector22 = vector2 + ((TheGoose.rFootMoveDir * 0.4f) * 5f);
				if (Time.time <= TheGoose.rFootMoveTimeStart + TheGoose.stepTime)
				{
					float single1 = (Time.time - TheGoose.rFootMoveTimeStart) / TheGoose.stepTime;
					TheGoose.rFootPos = Vector2.Lerp(TheGoose.rFootMoveOrigin, vector22, Easings.CubicEaseInOut(single1));
				}
				else
				{
					TheGoose.rFootPos = vector22;
					TheGoose.rFootMoveTimeStart = -1f;
					Sound.PlayPat();
					if (Time.time < TheGoose.trackMudEndTime)
					{
						TheGoose.AddFootMark(TheGoose.rFootPos);
						return;
					}
				}
			}
		}

		public static void Tick()
		{
			Cursor.Clip = Rectangle.Empty;
			if (TheGoose.currentTask != TheGoose.GooseTask.NabMouse && (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left && !TheGoose.lastFrameMouseButtonPressed && Vector2.Distance(TheGoose.position + new Vector2(0f, 14f), new Vector2((float)Cursor.Position.X, (float)Cursor.Position.Y)) < 30f)
			{
				TheGoose.SetTask(TheGoose.GooseTask.NabMouse);
			}
			TheGoose.lastFrameMouseButtonPressed = (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left;
			TheGoose.targetDirection = Vector2.Normalize(TheGoose.targetPos - TheGoose.position);
			TheGoose.overrideExtendNeck = false;
			TheGoose.RunAI();
			Vector2 vector2 = Vector2.Lerp(Vector2.GetFromAngleDegrees(TheGoose.direction), TheGoose.targetDirection, 0.25f);
			TheGoose.direction = (float)Math.Atan2((double)vector2.y, (double)vector2.x) * 57.2957764f;
			if (Vector2.Magnitude(TheGoose.velocity) > TheGoose.currentSpeed)
			{
				TheGoose.velocity = Vector2.Normalize(TheGoose.velocity) * TheGoose.currentSpeed;
			}
			TheGoose.velocity = TheGoose.velocity + ((Vector2.Normalize(TheGoose.targetPos - TheGoose.position) * TheGoose.currentAcceleration) * 0.008333334f);
			TheGoose.position = TheGoose.position + (TheGoose.velocity * 0.008333334f);
			TheGoose.SolveFeet();
			Vector2.Magnitude(TheGoose.velocity);
			int num = (TheGoose.overrideExtendNeck | TheGoose.currentSpeed >= 200f ? 1 : 0);
			TheGoose.gooseRig.neckLerpPercent = SamMath.Lerp(TheGoose.gooseRig.neckLerpPercent, (float)num, 0.075f);
		}

		private static Point ToIntPoint(Vector2 vector)
		{
			return new Point((int)vector.x, (int)vector.y);
		}

		public static void UpdateRig()
		{
			float single = TheGoose.direction;
			int num = (int)TheGoose.position.x;
			int num1 = (int)TheGoose.position.y;
			Vector2 vector2 = new Vector2((float)num, (float)num1);
			Vector2 vector21 = new Vector2(1.3f, 0.4f);
			Vector2 fromAngleDegrees = Vector2.GetFromAngleDegrees(single);
			Vector2 vector22 = new Vector2(0f, -1f);
			TheGoose.gooseRig.underbodyCenter = vector2 + (vector22 * 9f);
			TheGoose.gooseRig.bodyCenter = vector2 + (vector22 * 14f);
			int num2 = (int)SamMath.Lerp(20f, 10f, TheGoose.gooseRig.neckLerpPercent);
			int num3 = (int)SamMath.Lerp(3f, 16f, TheGoose.gooseRig.neckLerpPercent);
			TheGoose.gooseRig.neckCenter = vector2 + (vector22 * (float)(14 + num2));
			TheGoose.gooseRig.neckBase = TheGoose.gooseRig.bodyCenter + (fromAngleDegrees * 15f);
			TheGoose.gooseRig.neckHeadPoint = (TheGoose.gooseRig.neckBase + (fromAngleDegrees * (float)num3)) + (vector22 * (float)num2);
			TheGoose.gooseRig.head1EndPoint = (TheGoose.gooseRig.neckHeadPoint + (fromAngleDegrees * 3f)) - (vector22 * 1f);
			TheGoose.gooseRig.head2EndPoint = TheGoose.gooseRig.head1EndPoint + (fromAngleDegrees * 5f);
		}

		private enum GooseTask
		{
			Wander,
			NabMouse,
			CollectWindow_Meme,
			CollectWindow_Notepad,
			CollectWindow_Donate,
			CollectWindow_DONOTSET,
			TrackMud,
			Count
		}

		private class MovableForm : Form
		{
			public MovableForm()
			{
				base.StartPosition = FormStartPosition.Manual;
				base.Width = 400;
				base.Height = 400;
				this.BackColor = Color.DimGray;
				base.Icon = null;
				base.ShowIcon = false;
				this.SetWindowResizableThreadsafe(false);
			}

			public void SetWindowPositionThreadsafe(Point p)
			{
				if (base.InvokeRequired)
				{
					base.BeginInvoke(new MethodInvoker(() => {
						this.Location = p;
						this.TopMost = true;
					}));
					return;
				}
				base.Location = p;
				base.TopMost = true;
			}

			public void SetWindowResizableThreadsafe(bool canResize)
			{
				if (base.InvokeRequired)
				{
					base.BeginInvoke(new MethodInvoker(() => {
						this.FormBorderStyle = (canResize ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle);
						TheGoose.MovableForm u003cu003e4_this = this;
						TheGoose.MovableForm movableForm = this;
						bool flag = canResize;
						bool flag1 = flag;
						movableForm.MinimizeBox = flag;
						u003cu003e4_this.MaximizeBox = flag1;
					}));
					return;
				}
				base.FormBorderStyle = (canResize ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle);
				bool flag2 = canResize;
				bool flag3 = flag2;
				base.MinimizeBox = flag2;
				base.MaximizeBox = flag3;
			}
		}

		private struct Rig
		{
			public const int UnderBodyRadius = 15;

			public const int UnderBodyLength = 7;

			public const int UnderBodyElevation = 9;

			public Vector2 underbodyCenter;

			public const int BodyRadius = 22;

			public const int BodyLength = 11;

			public const int BodyElevation = 14;

			public Vector2 bodyCenter;

			public const int NeccRadius = 13;

			public const int NeccHeight1 = 20;

			public const int NeccExtendForward1 = 3;

			public const int NeccHeight2 = 10;

			public const int NeccExtendForward2 = 16;

			public float neckLerpPercent;

			public Vector2 neckCenter;

			public Vector2 neckBase;

			public Vector2 neckHeadPoint;

			public const int HeadRadius1 = 15;

			public const int HeadLength1 = 3;

			public const int HeadRadius2 = 10;

			public const int HeadLength2 = 5;

			public Vector2 head1EndPoint;

			public Vector2 head2EndPoint;

			public const int EyeRadius = 2;

			public const int EyeElevation = 3;

			public const float IPD = 5f;

			public const float EyesForward = 5f;
		}

		private class SimpleDonateForm : TheGoose.MovableForm
		{
			private const string patreonLink = "https://www.patreon.com/bePatron?u=3541875";

			private const string paypalLink = "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=WUKYHY7SZ275Q&currency_code=USD&source=url";

			private const string twitterLink = "https://www.twitter.com/samnchiet";

			private const string discordLink = "https://discord.gg/PCJS6DH";

			private static string donationGraphicSrc;

			private float scale;

			static SimpleDonateForm()
			{
				TheGoose.SimpleDonateForm.donationGraphicSrc = Program.GetPathToFileInAssembly("Assets/Images/OtherGfx/DonatePage.png");
			}

			public SimpleDonateForm()
			{
				PictureBox pictureBox = new PictureBox();
				base.ClientSize = new Size((int)(250f * this.scale), (int)(300f * this.scale));
				try
				{
					this.BackgroundImage = Image.FromFile(TheGoose.SimpleDonateForm.donationGraphicSrc);
				}
				catch
				{
					Label label = new Label()
					{
						Text = "Can't find the donation image... are you messing with the game files?\nCheck out my Twitter at twitter.com/samnchiet I guess?",
						Location = new Point(0, 0),
						Width = base.ClientSize.Width,
						Height = base.ClientSize.Height,
						BackColor = Color.White,
						TextAlign = ContentAlignment.MiddleCenter
					};
					base.Controls.Add(label);
				}
				this.BackgroundImageLayout = ImageLayout.Stretch;
				base.Controls.Add(this.SetupButton(111, 407, 390, 475, new EventHandler(this.OpenPatreonLink), true));
				base.Controls.Add(this.SetupButton(174, 500, 325, 545, new EventHandler(this.OpenPaypalLink), true));
				base.Controls.Add(this.SetupButton(381, 302, 433, 360, new EventHandler(this.OpenDiscordLink), true));
				base.Controls.Add(this.SetupButton(403, 247, 472, 312, new EventHandler(this.OpenTwitterLink), true));
			}

			private void OpenDiscordLink(object sender, EventArgs a)
			{
				Process.Start("https://discord.gg/PCJS6DH");
			}

			private void OpenPatreonLink(object sender, EventArgs a)
			{
				Process.Start("https://www.patreon.com/bePatron?u=3541875");
			}

			private void OpenPaypalLink(object sender, EventArgs a)
			{
				Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=WUKYHY7SZ275Q&currency_code=USD&source=url");
			}

			private void OpenTwitterLink(object sender, EventArgs a)
			{
				Process.Start("https://www.twitter.com/samnchiet");
			}

			private Button SetupButton(int point1X, int point1Y, int point2X, int point2Y, EventHandler handler, bool showHoverClick = true)
			{
				Button button = new Button()
				{
					Location = new Point((int)((float)point1X * this.scale) / 2, (int)((float)point1Y * this.scale) / 2),
					Size = new Size((int)((float)(point2X - point1X) * this.scale) / 2, (int)((float)(point2Y - point1Y) * this.scale) / 2)
				};
				button.Click += handler;
				button.Cursor = Cursors.Hand;
				button.BackColor = Color.Transparent;
				button.ForeColor = Color.Transparent;
				button.FlatStyle = FlatStyle.Flat;
				button.FlatAppearance.MouseOverBackColor = (showHoverClick ? Color.FromArgb(40, Color.White) : Color.Transparent);
				button.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, Color.White);
				button.FlatAppearance.BorderSize = 0;
				button.TabStop = false;
				return button;
			}
		}

		private class SimpleImageForm : TheGoose.MovableForm
		{
			private readonly static string memesRootFolder;

			private Image[] localImages;

			private Deck localImageDeck;

			private static string[] imageURLs;

			private static Deck imageURLDeck;

			static SimpleImageForm()
			{
				TheGoose.SimpleImageForm.memesRootFolder = Program.GetPathToFileInAssembly("Assets/Images/Memes/");
				TheGoose.SimpleImageForm.imageURLs = new string[] { "https://preview.redd.it/dsfjv8aev0p31.png?width=960&crop=smart&auto=webp&s=1d58948acc5c6dd60df1092c1bd2a59a509069fd", "https://i.redd.it/4ojv59zvglp31.jpg", "https://i.redd.it/4bamd6lnso241.jpg", "https://i.redd.it/5i5et9p1vsp31.jpg", "https://i.redd.it/j2f1i9djx5p31.jpg" };
				TheGoose.SimpleImageForm.imageURLDeck = new Deck((int)TheGoose.SimpleImageForm.imageURLs.Length);
			}

			public SimpleImageForm()
			{
				List<Image> images = new List<Image>();
				try
				{
					string[] files = Directory.GetFiles(TheGoose.SimpleImageForm.memesRootFolder);
					for (int i = 0; i < (int)files.Length; i++)
					{
						Image image = Image.FromFile(files[i]);
						if (image != null)
						{
							images.Add(image);
						}
					}
				}
				catch
				{
				}
				this.localImages = images.ToArray();
				this.localImageDeck = new Deck((int)this.localImages.Length);
				PictureBox pictureBox = new PictureBox()
				{
					Dock = DockStyle.Fill
				};
				try
				{
					pictureBox.Image = this.localImages[this.localImageDeck.Next()];
				}
				catch
				{
					pictureBox.LoadAsync(TheGoose.SimpleImageForm.imageURLs[TheGoose.SimpleImageForm.imageURLDeck.Next()]);
				}
				pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
				base.Controls.Add(pictureBox);
			}
		}

		private class SimpleTextForm : TheGoose.MovableForm
		{
			private static string[] possiblePhrases;

			private static Deck textIndices;

			static SimpleTextForm()
			{
				TheGoose.SimpleTextForm.possiblePhrases = new string[] { "am goose hjonk", "good work", "nsfdafdsaafsdjl\r\nasdas       sorry\r\nhard to type withh feet", "i cause problems on purpose", "\"peace was never an option\"\r\n   -the goose (me)", "\r\n\r\n  >o) \r\n    (_>" };
				TheGoose.SimpleTextForm.textIndices = new Deck((int)TheGoose.SimpleTextForm.possiblePhrases.Length);
			}

			public SimpleTextForm()
			{
				base.Width = 200;
				base.Height = 150;
				this.Text = "Goose \"Not-epad\"";
				TextBox textBox = new TextBox()
				{
					Multiline = true,
					AcceptsReturn = true,
					Text = TheGoose.SimpleTextForm.possiblePhrases[TheGoose.SimpleTextForm.textIndices.Next()],
					Location = new Point(0, 0),
					Width = base.ClientSize.Width,
					Height = base.ClientSize.Height - 5
				};
				textBox.Select(textBox.Text.Length, 0);
				textBox.Font = new Font(textBox.Font.FontFamily, 10f, FontStyle.Regular);
				base.Controls.Add(textBox);
				string str = string.Concat(Environment.SystemDirectory, "\\notepad.exe");
				if (File.Exists(str))
				{
					try
					{
						base.Icon = Icon.ExtractAssociatedIcon(str);
						base.ShowIcon = true;
					}
					catch
					{
					}
				}
			}

			private void ExitWindow(object sender, EventArgs args)
			{
				base.Close();
			}
		}

		private enum SpeedTiers
		{
			Walk,
			Run,
			Charge
		}

		private struct Task_CollectWindow
		{
			public TheGoose.MovableForm mainForm;

			public TheGoose.Task_CollectWindow.Stage stage;

			public float secsToWait;

			public float waitStartTime;

			public TheGoose.Task_CollectWindow.ScreenDirection screenDirection;

			public Vector2 windowOffsetToBeak;

			public static float GetWaitTime()
			{
				return SamMath.RandomRange(2f, 3.5f);
			}

			public enum ScreenDirection
			{
				Left,
				Top,
				Right
			}

			public enum Stage
			{
				WalkingOffscreen,
				WaitingToBringWindowBack,
				DraggingWindowBack
			}
		}

		private struct Task_NabMouse
		{
			public TheGoose.Task_NabMouse.Stage currentStage;

			public Vector2 dragToPoint;

			public float grabbedOriginalTime;

			public float chaseStartTime;

			public Vector2 originalVectorToMouse;

			public const float MouseGrabDistance = 15f;

			public const float MouseSuccTime = 0.06f;

			public const float MouseDropDistance = 30f;

			public const float MinRunTime = 2f;

			public const float MaxRunTime = 4f;

			public const float GiveUpTime = 9f;

			public readonly static Vector2 StruggleRange;

			static Task_NabMouse()
			{
				TheGoose.Task_NabMouse.StruggleRange = new Vector2(3f, 3f);
			}

			public enum Stage
			{
				SeekingMouse,
				DraggingMouseAway,
				Decelerating
			}
		}

		private struct Task_TrackMud
		{
			public const float DurationToRunAmok = 2f;

			public float nextDirChangeTime;

			public float timeToStopRunning;

			public TheGoose.Task_TrackMud.Stage stage;

			public static float GetDirChangeInterval()
			{
				return 100f;
			}

			public enum Stage
			{
				DecideToRun,
				RunningOffscreen,
				RunningWandering
			}
		}

		private struct Task_Wander
		{
			private const float MinPauseTime = 1f;

			private const float MaxPauseTime = 2f;

			public const float GoodEnoughDistance = 20f;

			public float wanderingStartTime;

			public float wanderingDuration;

			public float pauseStartTime;

			public float pauseDuration;

			public static float GetRandomPauseDuration()
			{
				return 1f + (float)SamMath.Rand.NextDouble() * 1f;
			}

			public static float GetRandomWalkTime()
			{
				return SamMath.RandomRange(1f, 6f);
			}

			public static float GetRandomWanderDuration()
			{
				if (Time.time < 1f)
				{
					return GooseConfig.settings.FirstWanderTimeSeconds;
				}
				return SamMath.RandomRange(GooseConfig.settings.MinWanderingTimeSeconds, GooseConfig.settings.MaxWanderingTimeSeconds);
			}
		}
	}
}