using Godot;
using System;

public partial class Visual : Node2D
{
    private AnimatedSprite2D WarriorAnimations; // Referência ao AnimatedSprite2D

    public override void _Ready()
    {
        // Tenta pegar o nó AnimatedSprite2D
        WarriorAnimations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // Verifica se a referência foi corretamente inicializada
        if (WarriorAnimations == null)
        {
            GD.PrintErr("Erro: O nó AnimatedSprite2D não foi encontrado!");
        }
    }

    // Atualiza a direção do sprite com base no valor de direção X
    public void UpdateSpriteDirection(float directionX)
    {
        // Verifica se o WarriorAnimations não é nulo antes de acessar suas propriedades
        if (WarriorAnimations != null)
        {
            if (directionX < 0)
            {
                WarriorAnimations.FlipH = true; // Inverte o sprite se for movido para a esquerda
            }
            else if (directionX > 0)
            {
                WarriorAnimations.FlipH = false; // Se movendo para a direita
            }
        }
        else
        {
            GD.PrintErr("Erro: WarriorAnimations é nulo.");
        }
    }
}
