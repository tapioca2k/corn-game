function contains (pos, x, y, width, height)
	return pos.X > x and pos.X < x+width and pos.Y > y and pos.Y < y+height
end

for i=0, 3 do
	pos = players[i]:getPosition()
	if contains(pos, 1857, 0, 63, 1080) or contains(pos, 0, 0, 102 - 71, 1080) or contains(pos, 0, 977, 1920, 103) or contains(pos, 656, 679, 622, 401) then
		players[i]:fallDeath()
	end
end