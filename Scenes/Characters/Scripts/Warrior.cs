using Godot;
using System;

public partial class Warrior : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;
    private const float GRAVITY = 900.0f;
    private const float Deceleration = 800.0f;

    private AnimatedSprite2D WarriorAnimations;
    private Area2D WarriorAttackArea;
    private CollisionShape2D WarriorAttackCollisionShape;
    private CollisionShape2D WarriorCollisionShape;
    private Vector2 direction;
    private bool isAttacking = false;
    private bool isInAttackAnimation = false;
    private float initialCollisionShapeOffsetX; // Store initial X offset

    public override void _Ready()
    {
        WarriorAnimations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        WarriorAttackArea = GetNode<Area2D>("WarriorAttackArea");
        WarriorAttackCollisionShape = WarriorAttackArea.GetNode<CollisionShape2D>("WarriorAttackCollisionShape");
        WarriorCollisionShape = GetNode<CollisionShape2D>("WarriorCollisionShape");
        WarriorAttackCollisionShape.Disabled = true;
        WarriorAnimations.Play("WarriorIdle");
        WarriorAnimations.AnimationFinished += _on_AttackAnimationFinished;
        initialCollisionShapeOffsetX = WarriorCollisionShape.Position.X; // Store initial offset
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity.Y += GRAVITY * (float)delta;

            if (!isAttacking)
            {
                if (velocity.Y > 0 && WarriorAnimations.Animation != "WarriorFall")
                {
                    WarriorAnimations.Play("WarriorFall");
                }
                else if (velocity.Y <= 0 && WarriorAnimations.Animation != "WarriorJump")
                {
                    WarriorAnimations.Play("WarriorJump");
                }
            }

            direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            if (direction != Vector2.Zero)
            {
                velocity.X = direction.X * Speed;
                WarriorAnimations.FlipH = direction.X < 0;
                WarriorAttackArea.Scale = new Vector2(direction.X < 0 ? -1 : 1, 1);

                if (direction.X < 0)
                {
                    WarriorCollisionShape.Position = new Vector2(-initialCollisionShapeOffsetX, WarriorCollisionShape.Position.Y);
                }
                else
                {
                    WarriorCollisionShape.Position = new Vector2(initialCollisionShapeOffsetX, WarriorCollisionShape.Position.Y);
                }
            }
            else
            {
                if (velocity.X > 0)
                    velocity.X = Mathf.Max(velocity.X - Deceleration * (float)delta, 0);
                else if (velocity.X < 0)
                    velocity.X = Mathf.Min(velocity.X + Deceleration * (float)delta, 0);
            }
        }
        else
        {
            if (Input.IsActionJustPressed("ui_accept") && !isAttacking)
            {
                velocity.Y = JumpVelocity;
                WarriorAnimations.Play("WarriorJump");
            }

            direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

            if (direction != Vector2.Zero && !isAttacking)
            {
                velocity.X = direction.X * Speed;
                if (WarriorAnimations.Animation != "WarriorRunning")
                {
                    WarriorAnimations.Play("WarriorRunning");
                }
                WarriorAnimations.FlipH = direction.X < 0;
                WarriorAttackArea.Scale = new Vector2(direction.X < 0 ? -1 : 1, 1);

                if (direction.X < 0)
                {
                    WarriorCollisionShape.Position = new Vector2(-initialCollisionShapeOffsetX, WarriorCollisionShape.Position.Y);
                }
                else
                {
                    WarriorCollisionShape.Position = new Vector2(initialCollisionShapeOffsetX, WarriorCollisionShape.Position.Y);
                }
            }
            else if (!isAttacking)
            {
                velocity.X = 0;
                if (WarriorAnimations.Animation != "WarriorIdle")
                {
                    WarriorAnimations.Play("WarriorIdle");
                }
            }

            if (Input.IsActionJustPressed("Attack") && !isAttacking)
            {
                isAttacking = true;
                WarriorAttackCollisionShape.Disabled = false;
                WarriorAnimations.Play("WarriorAttack");
            }
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private void _on_AttackAnimationFinished()
    {
        isAttacking = false;
        WarriorAttackCollisionShape.Disabled = true;

        if (Input.GetAxis("ui_left", "ui_right") != 0)
        {
            WarriorAnimations.Play("WarriorRunning");
        }
        else
        {
            WarriorAnimations.Play("WarriorIdle");
        }
    }
}