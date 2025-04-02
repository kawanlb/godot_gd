extends CharacterBody2D

const SPEED = 250.0
const JUMP_VELOCITY = -400.0
const GRAVITY = 900.0
const DECELERATION = 800.0

@onready var warrior_animations: AnimatedSprite2D = $AnimatedSprite2D
@onready var warrior_sword: Area2D = $WarriorSword
@onready var warrior_attack_collision_shape: CollisionShape2D = $WarriorSword/WarriorAttackCollisionShape
@onready var warrior_collision_shape: CollisionShape2D = $WarriorCollisionShape

var direction := Vector2.ZERO
var is_attacking := false
var initial_collision_shape_offset_x: float

func _ready(): 
	warrior_attack_collision_shape.disabled = true
	warrior_animations.play("WarriorIdle")
	warrior_animations.animation_finished.connect(_on_attack_animation_finished)
	initial_collision_shape_offset_x = warrior_collision_shape.position.x

func _physics_process(delta):
	var vel = velocity  # Usa uma variável local para evitar conflitos

	if not is_on_floor():
		vel.y += GRAVITY * delta

		if not is_attacking:
			if vel.y > 0 and warrior_animations.animation != "WarriorFall":
				warrior_animations.play("WarriorFall")
			elif vel.y <= 0 and warrior_animations.animation != "WarriorJump":
				warrior_animations.play("WarriorJump")

		direction = Vector2(Input.get_axis("ui_left", "ui_right"), 0)
		if direction.x != 0:
			vel.x = direction.x * SPEED
			warrior_animations.flip_h = direction.x < 0
			warrior_sword.scale.x = -1 if direction.x < 0 else 1
			warrior_collision_shape.position.x = -initial_collision_shape_offset_x if direction.x < 0 else initial_collision_shape_offset_x
		else:
			vel.x = move_toward(vel.x, 0, DECELERATION * delta)
	else:
		if Input.is_action_just_pressed("ui_accept") and not is_attacking:
			vel.y = JUMP_VELOCITY
			warrior_animations.play("WarriorJump")

		direction = Vector2(Input.get_axis("ui_left", "ui_right"), 0)

		if direction.x != 0 and not is_attacking:
			vel.x = direction.x * SPEED
			if warrior_animations.animation != "WarriorRunning":
				warrior_animations.play("WarriorRunning")
			warrior_animations.flip_h = direction.x < 0
			warrior_sword.scale.x = -1 if direction.x < 0 else 1
			warrior_collision_shape.position.x = -initial_collision_shape_offset_x if direction.x < 0 else initial_collision_shape_offset_x
		elif not is_attacking:
			vel.x = 0
			if warrior_animations.animation != "WarriorIdle":
				warrior_animations.play("WarriorIdle")

		if Input.is_action_just_pressed("Attack") and not is_attacking:
			is_attacking = true
			warrior_attack_collision_shape.disabled = false
			warrior_animations.play("WarriorAttack")
			vel.x = 0
	
	velocity = vel  # Atualiza a velocidade do CharacterBody2D
	move_and_slide()

func _on_attack_animation_finished():
	is_attacking = false
	warrior_attack_collision_shape.disabled = true

	if Input.get_axis("ui_left", "ui_right") != 0:
		warrior_animations.play("WarriorRunning")
	else:
		warrior_animations.play("WarriorIdle")
