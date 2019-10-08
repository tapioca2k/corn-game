function contains (pos, x, y, width, height)
	return pos.X > x and pos.X < x+width and pos.Y > y and pos.Y < y+height
end

for i=0, 3 do
	pos = players[i]:getPosition()
	if contains(pos, 498, 784, 158, 81) then
		players[i]:damage(-0.1)
		players[i]:setRedFlash(0.1)
	end
end