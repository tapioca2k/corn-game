function contains (pos, x, y, width, height)
	return pos.X > x and pos.X < x+width and pos.Y > y and pos.Y < y+height
end

for i=0, 3 do
	pos = players[i]:getPosition()
	if contains(pos, 486-40, 183, 96+20, 161) then
		-- teleport to lower blue
		players[i].pos = players[i]:makePos(1316, 741)
		Game.Music:playSfx("teleportsfx")
	elseif contains(pos, 1289-40, 178, 93+20, 158) then
		-- teleport to lower red
		players[i].pos = players[i]:makePos(488, 741)
		Game.Music:playSfx("teleportsfx")
	elseif contains(pos, 481-40, 651, 96+20, 159) then
		-- teleport to upper red
		players[i].pos = players[i]:makePos(1296, 268)
		Game.Music:playSfx("teleportsfx")
	elseif contains(pos, 1304-40, 651, 95+20, 158) then
		-- teleport to upper blue
		players[i].pos = players[i]:makePos(491, 268)
		Game.Music:playSfx("teleportsfx")
	end
end