using Godot;
using System;

public partial class ForestBackground : ParallaxBackground
{
    private float ScrollSpeed = -30f;


    public override void _Process(double delta){

        this.ScrollOffset += new Vector2((float)(ScrollSpeed * delta), 0);

    }
}
    