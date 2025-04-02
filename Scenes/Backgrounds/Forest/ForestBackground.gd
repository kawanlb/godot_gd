extends ParallaxBackground

var scroll_speed: float = -30.0

func _process(delta: float) -> void:
	self.scroll_offset.x += scroll_speed * delta
