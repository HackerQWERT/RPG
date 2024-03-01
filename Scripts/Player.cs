using Godot;
using System;

public partial class Player : CharacterBody2D
{

    [Export]
    public float Speed = 300.0f;
    public bool IsSurvive = true;
    public double delta;

    //二段跳
    public bool CanDoubleJump = true;
    public float JumpVelocity = -400.0f;

    public PlayerState State { get; private set; }

    [Export]
    public AnimatedSprite2D animatedSprite2D;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();



    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            //检测左右移动
            if (eventKey.IsActionPressed("Left"))
            {
                Velocity = new Vector2(-Speed, Velocity.Y);
                GD.Print("Move Left");
            }
            if (eventKey.IsActionPressed("Right"))
            {
                Velocity = new Vector2(Speed, Velocity.Y);
                GD.Print("Move Right");
            }
            //检测跳跃
            if (eventKey.IsActionPressed("Jump") && IsOnFloor() || eventKey.IsActionPressed("Jump") && CanDoubleJump)
            {
                Velocity = new Vector2(Velocity.X, JumpVelocity);
                GD.Print("Jump");
            }
            //弹开取消移动
            if (eventKey.IsActionReleased("Left") && Velocity.X < 0)
            {
                Velocity = new Vector2(0, Velocity.Y);
                GD.Print("Stop Move");
            }
            if (eventKey.IsActionReleased("Right") && Velocity.X > 0)
            {
                Velocity = new Vector2(0, Velocity.Y);
                GD.Print("Stop Move");
            }
            //检测攻击  
            if (eventKey.IsActionPressed("Attack"))
            {
                GD.Print("Attack");
            }

        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Velocity.Y != 0)
        {
            GD.Print("velocity: " + Velocity);
        }

        this.delta = delta;
        // Add gravity.
        AddGravity();
        //Flip X  when change direction
        ChangeFlipH();
        //Change the state of the player
        ChangePlayState();

        // Handle the state machine.
        switch (State)
        {
            case PlayerState.Idle:
                IDle();
                break;
            case PlayerState.Running:
                Running();
                break;
            case PlayerState.Attack:
                Attack();
                break;
            case PlayerState.Dead:
                Dead();
                break;
            case PlayerState.JumpStart:
                JumpStart();
                break;
            case PlayerState.JumpInAir:
                JumpInAir();
                break;
            case PlayerState.JumpEnd:
                JumpEnd();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        MoveAndSlide();
    }


    private void ChangePlayState()
    {
        if (State == PlayerState.Idle)
        {
            //-->Attack
            if (Input.IsActionJustPressed("Attack"))
            {
                State = PlayerState.Attack;
            }
            //-->Running
            else if (Velocity.X != 0 && IsOnFloor())
            {
                State = PlayerState.Running;
            }
            //-->JumpStart
            else if (Input.IsActionJustPressed("Jump") && IsOnFloor())
            {
                State = PlayerState.JumpStart;
            }
            //-->JumpInAir
            else if (Velocity.Y != 0 && !IsOnFloor())
            {
                State = PlayerState.JumpInAir;
            }
            //-->Dead
            else if (!IsSurvive)
            {
                State = PlayerState.Dead;
                //TODO Dead
            }

        }
        else if (State == PlayerState.Running)
        {
            //-->Attack
            if (Input.IsActionJustPressed("Attack"))
            {
                State = PlayerState.Attack;
            }
            //-->Idle
            else if (Velocity.X == 0 && IsOnFloor())
            {
                State = PlayerState.Idle;
            }
            //-->JumpStart
            else if (Input.IsActionJustPressed("Jump") && IsOnFloor())
            {
                State = PlayerState.JumpStart;
            }
            //-->JumpInAir
            else if (Velocity.Y != 0 || !IsOnFloor())
            {
                State = PlayerState.JumpInAir;
            }
            //-->Dead
            else if (!IsSurvive)
            {
                State = PlayerState.Dead;
                //TODO Dead
            }
        }
        else if (State == PlayerState.Attack)
        {
            //must complete the attack animation
            //--->Idle
            if (animatedSprite2D.Animation == "Attack" && animatedSprite2D.Frame == 7)
            {
                State = PlayerState.Idle;
            }
        }
        else if (State == PlayerState.JumpStart)
        {
            //must complete the JumpStart animation
            //--->JumpInAir
            if (animatedSprite2D.Animation == "JumpStart" && animatedSprite2D.Frame == 3)
            {
                State = PlayerState.JumpInAir;
            }
        }
        else if (State == PlayerState.JumpInAir)
        {
            //-->JmpStart 二段跳
            if (Input.IsActionJustPressed("Jump") && CanDoubleJump)
            {
                State = PlayerState.JumpStart;
                CanDoubleJump = false;
            }
            //--->JumpEnd
            if (Velocity.Y == 0 || IsOnFloor())
            {
                State = PlayerState.JumpEnd;
            }
            //-->Attack
            else if (Input.IsActionJustPressed("Attack"))
            {
                State = PlayerState.Attack;
            }
            //-->Dead
            else if (!IsSurvive)
            {
                State = PlayerState.Dead;
                //TODO Dead
            }
        }
        else if (State == PlayerState.JumpEnd)
        {
            //must complete the JumpEnd animation
            //--->Idle
            if (animatedSprite2D.Animation == "JumpEnd" && animatedSprite2D.Frame == 2)
            {
                State = PlayerState.Idle;
                CanDoubleJump = true;
            }
        }

    }

    //Flip H  when change direction

    private void ChangeFlipH()
    {
        if (Velocity.X != 0)
            animatedSprite2D.FlipH = Velocity.X < 0;
    }

    #region Animation
    //only play for once
    private void Dead()
    {
        animatedSprite2D.Play("Dead");
    }

    //once the animation is finished, it will automatically return to the idle state
    private void Attack()
    {
        animatedSprite2D.Play("Attack");
    }

    //cycle play,only change to JumpEnd when the player is on the ground 
    private void JumpInAir()
    {
        animatedSprite2D.Play("JumpInAir");
    } 

    //only play for once
    //once the animation is finished, it will automatically return to the idle state
    private void JumpEnd()
    {
        animatedSprite2D.Play("JumpEnd");
    }

    //only play for once
    //once the animation is finished, it will only change to JumpInAir
    private void JumpStart()
    {
        animatedSprite2D.Play("JumpStart");
    }

    //cycle play
    //only play when the Velocity.X is not 0
    private void Running()
    {
        if (animatedSprite2D.Animation != "Running")
            animatedSprite2D.Play("Running");
        if (Velocity.X != 0)
            animatedSprite2D.FlipH = Velocity.X < 0;
    }

    //cycle play
    //only play when the Velocity.X is 0
    private void IDle()
    {
        if (animatedSprite2D.Animation != "Idle")
            animatedSprite2D.Play("Idle");
    }


    public void AddGravity()
    {
        Vector2 velocity = Velocity;
        if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;
        Velocity = velocity;
    }
    #endregion

}
