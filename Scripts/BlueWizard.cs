using Godot;
using System;
using System.Threading.Tasks;

public partial class BlueWizard : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;

	[Export]
	public float JumpVelocity = -400.0f;
	public BlueWizardStates BlueWizardStates { get; private set; }

	[Export]
	public AnimatedSprite2D animatedSprite2D;
	private int jumpCount = 0;

	bool isDashing = false;
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle() * 4;

	public override async void _PhysicsProcess(double delta)
	{
		ChangeFlipH();
		ChangePlayBlueWizardStates();
		//Reset jump count when on floor
		if (IsOnFloor())
		{
			jumpCount = 0;
		}
		// Handle the state machine.
		switch (BlueWizardStates)
		{
			case BlueWizardStates.Idle:
				Idle();
				break;
			case BlueWizardStates.Walk:
				Walk();
				break;
			case BlueWizardStates.Dash:
				if (isDashing)
				{
					break;
				}
				await Dash();
				break;
			case BlueWizardStates.Jump:
				Jump();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && jumpCount < 2)
		{
			velocity.Y = JumpVelocity;
			jumpCount++;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void Jump()
	{
		animatedSprite2D.Play("Jump");
	}

	private async Task Dash()
	{
		//向当前方向冲刺0.8S并停下
		// 1.播放Dash动画
		animatedSprite2D.Play("Dash");
		// 2.向前冲刺
		var originalSpeed = Speed; // 假设Speed是你的角色移动速度的变量
		Speed *= 2
		; // 假设冲刺时速度翻倍
		  // 3.停下
		await Task.Delay(800); // 等待0.8秒
		Speed = originalSpeed; // 恢复原来的速度

	}

	private void Walk()
	{
		if (animatedSprite2D.Animation != "Walk")
			animatedSprite2D.Play("Walk");
		if (Velocity.X != 0)
			animatedSprite2D.FlipH = Velocity.X < 0;
	}


	private void Idle()
	{
		if (animatedSprite2D.Animation != "Idle")
			animatedSprite2D.Play("Idle");
	}

	private void ChangeFlipH()
	{
		if (Velocity.X != 0)
			animatedSprite2D.FlipH = Velocity.X < 0;
	}

	private void ChangePlayBlueWizardStates()
	{
		if (BlueWizardStates == BlueWizardStates.Idle)
		{
			//-->Walk
			if (Velocity.X != 0 && IsOnFloor())
			{
				BlueWizardStates = BlueWizardStates.Walk;
			}
			//-->Jump
			else if (Input.IsActionJustPressed("Jump") && jumpCount < 2)
			{
				BlueWizardStates = BlueWizardStates.Jump;
			}
			// //-->JumpInAir
			// else if (Velocity.Y != 0 && !IsOnFloor())
			// {
			// 	BlueWizardStates = BlueWizardStates.JumpInAir;
			// }
			//-->Dead
			else if (Input.IsActionJustPressed("Dash"))
			{
				BlueWizardStates = BlueWizardStates.Dash;
			}

		}
		else if (BlueWizardStates == BlueWizardStates.Walk)
		{
			//-->Dash
			if (Input.IsActionJustPressed("Dash"))
			{
				BlueWizardStates = BlueWizardStates.Dash;
			}
			//-->Idle
			else if (Velocity.X == 0 && IsOnFloor())
			{
				BlueWizardStates = BlueWizardStates.Idle;
			}
			//-->Jump
			else if (Input.IsActionJustPressed("Jump") && jumpCount < 2)
			{
				BlueWizardStates = BlueWizardStates.Jump;
			}
			// //-->JumpInAir
			// else if (Velocity.Y != 0 || !IsOnFloor())
			// {
			// 	BlueWizardStates = BlueWizardStates.JumpInAir;
			// }
			//-->Dead
		}
		else if (BlueWizardStates == BlueWizardStates.Jump)
		{
			//must complete the Jump animation
			//--->Idle
			if (animatedSprite2D.Animation == "Jump" && animatedSprite2D.Frame == 7)
			{
				BlueWizardStates = BlueWizardStates.Idle;
			}
			//->Dash
			else if (Input.IsActionJustPressed("Dash"))
			{
				BlueWizardStates = BlueWizardStates.Dash;
			}

		}
		else if (BlueWizardStates == BlueWizardStates.Dash)
		{
			isDashing = true;
			//must complete the Dash animation
			//--->JumpInAir
			if (animatedSprite2D.Animation == "Dash" && animatedSprite2D.Frame == 15)
			{
				BlueWizardStates = BlueWizardStates.Idle;
				isDashing = false;
			}
		}
	}

}
