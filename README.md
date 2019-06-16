# PBOer
A console PBO creator for Arma 3

# How to use
Download the files in the "Download" directory and then browse to that folder on your computer and open a console window. Then you could type something like:
PBOer "where/your/arma/mission/is" "where/you/want/your/pbo"
or
PBO "C:\Users\UserName\Documents\Arma 3\missions" "C:\Program Files (x86)\Steam\steamapps\common\Arma 3\MPMissions"

# Filter
In the PBOer.exe.config you'll be able to turn some filters on or off. These filters will exclude files from your mission directory. Default is all filter on.

You can decide among these filters:
  1. SQX - Exclude all *.sqx files from the pbo
  2. Test folder - Exclude all files in 'Tests' folder
  3. Hidden files - Exclude all hidden files.
  4. Tproj - Exclude the *.tproj file (TypeSQF project file)
  5. Git - Exclude everything .git related

# Copyright
Copyright 2019 by Josef Fällman
Licensed under GNU General Public License 3.0

# This program incorporates work covered by the following copyright and permission notice: 
PBOSharp - Copyright 2017 by Paton7 <https://github.com/Paton7/PBOSharp>