extends CharacterBody2D

var speed = 200
var isAttacking = false

func _physics_process(delta):
	velocity = Vector2.ZERO  # Initialize velocity
	
	if Input.is_action_pressed("ui_right") and not isAttacking:
		velocity.x = speed
		$AnimatedSprite2D.play("WarriorRunning")
	elif Input.is_action_pressed("ui_left") and not isAttacking:
		velocity.x = -speed
		$AnimatedSprite2D.play("WarriorRunning")
	else:
		velocity.x = 0
		if not isAttacking:
			$AnimatedSprite2D.play("WarriorIdle")
	
	if Input.is_action_just_pressed("Attack"):
		$AnimatedSprite2D.play("WarriorAttack")
		isAttacking = true

	# Set up_direction (optional based on game logic)
	up_direction = Vector2(0, -1)

	# Call move_and_slide with no arguments, it uses the velocity and up_direction properties
	move_and_slide()

func _on_animated_sprite_2d_animation_finished():
	if $AnimatedSprite2D.animation == "WarriorAttack":
		isAttacking = false
