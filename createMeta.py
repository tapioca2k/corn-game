#!/usr/bin/python

# Quick script for creating meta files

asset_name = input("What is the name of the sprite file? ").strip()
asset_height = int(input("What is the height of the sprite? ").strip())
asset_width = int(input("What is the width of the sprite? ").strip())
num_animations = int(input("How many animations are on the sheet? ").strip());
frames_line = ''
durations_line = ''
loops_line = ''
for x in range(num_animations):
    frames_line += ',' + input("How many frames is animation #" + str(x) + "? ").strip()
    durations_line += "," + input("How many milliseconds is each frame of animation #" + str(x) + "? ").strip()
    loops_line += "," + input("Is this a looping animation? (0/1) ").strip()
frames_line = frames_line[1:]
durations_line = durations_line[1:]
loops_line = loops_line[1:]
meta_file = open('Glamour2/Content/' + asset_name + "_meta", 'w')
meta_file.write(str(asset_height) + "\n")
meta_file.write(str(asset_width) + "\n")
meta_file.write(str(num_animations) + "\n")
meta_file.write(frames_line + "\n")
meta_file.write(durations_line + "\n")
meta_file.write(loops_line + "\n")
meta_file.close()
print("Wrote meta file to directory. Remember to add it to the project and set it to 'Copy if Newer'")
