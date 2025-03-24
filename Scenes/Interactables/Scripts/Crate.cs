using Godot;
using System;

public partial class Crate : StaticBody2D
{
    [Export] private AnimationPlayer anim; // Referência ao AnimationPlayer
    private Area2D hitbox; // Referência ao Hitbox

    public override void _Ready()
    {
        // Obtém o AnimationPlayer
        anim = GetNodeOrNull<AnimationPlayer>("CrateAnimation");
        if (anim == null)
        {
            GD.PrintErr("CrateAnimation não encontrado! Verifique a cena.");
        }

        // Obtém a hitbox (Area2D) e conecta o sinal
        hitbox = GetNodeOrNull<Area2D>("Hitbox");
        if (hitbox != null)
        {
            hitbox.AreaEntered += OnHitboxAreaEntered; // Conectar sinal de detecção
        }
        else
        {
            GD.PrintErr("Hitbox não encontrada! Verifique a estrutura da cena.");
        }
    }

    private void OnHitboxAreaEntered(Area2D area)
    {
        GD.Print("Crate atingido por: " + area.Name); // Depuração: verifica quem bateu

        if (area.Name == "WarriorSword") // Verifica se foi a espada
        {
            GD.Print("Crate destruído!");
            anim.Play("Destroyed"); // Toca a animação
            _ = WaitForAnimation(); // Aguarda a animação terminar antes de remover
        }
        else
        {
            GD.Print("O objeto que atingiu não é a espada.");
        }
    }

    private async System.Threading.Tasks.Task WaitForAnimation()
    {
        await ToSignal(anim, "animation_finished");
        QueueFree(); 
    }
}
