extends StaticBody2D

@export var anim: AnimationPlayer = null # Referência ao AnimationPlayer
var hitbox: Area2D = null # Referência ao Hitbox

func _ready():
    # Obtém o AnimationPlayer
    anim = $CrateAnimation if has_node("CrateAnimation") else null
    if anim == null:
        print("CrateAnimation não encontrado! Verifique a cena.")
    
    # Obtém a hitbox (Area2D) e conecta o sinal
    hitbox = $Hitbox if has_node("Hitbox") else null
    if hitbox != null:
        hitbox.connect("area_entered", Callable(self, "_on_hitbox_area_entered"))
    else:
        print("Hitbox não encontrada! Verifique a estrutura da cena.")

func _on_hitbox_area_entered(area: Area2D):
    print("Crate atingido por: " + area.name) # Depuração: verifica quem bateu
    
    if area.name == "WarriorSword": # Verifica se foi a espada
        print("Crate destruído!")
        if anim:
            anim.play("Destroyed") # Toca a animação
            await anim.animation_finished # Aguarda a animação terminar antes de remover
        queue_free()
    else:
        print("O objeto que atingiu não é a espada.")
