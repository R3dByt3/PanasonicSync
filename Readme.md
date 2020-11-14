# Welcome to PanasonicSync!
**The project currently does not work properly after checkout and compilation!**

This is caused by a change of a ffmpeg build service.
As long as the project is not adapted to this change the following steps must be taken after compilation:

 1. Create a folder named "bin" inside the bin/Debug or bin/Release folder.
 2. Copy the info.bin from the solution Misc folder in this directory.
 3. Place ffmpeg.exe ffplay.exe and ffprobe.exe in this folder (Available from this [site](https://www.gyan.dev/ffmpeg/builds/))

