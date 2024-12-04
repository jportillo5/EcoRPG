# EcoRPG
 The repository for the EcoQuest project for the Software Development Capstone course.

 A runnable build of the game can be found in the directory EcoRPG/Assets/Build
 This file can be ran like any other executable (hopefully it doesn't think the file's a virus).
 If you wish to examine the scenes and code more directly, you'll need the Unity Hub desktop application and working Unity License. Additionally, you'll need an editor that supports C# (I recommend Visual Studio Code, NOT Visual Studio). After downloading our project or cloning the repository, from the Unity Hub main page, click on the Add drop menu and select "Add project from disk", then find the EcoRPG folder. You'll then be able to open the project.

 There are a number of Scenes that aren't included in the Build Settings and were only used for developmental purposes. The scenes included in the Build Settings are:
 - Title Sequence Animation
 - Title Screen
 - Opening Story Cutscene
 - Farm Scene
 - Forest Scene
 - Forest Shrine
 - DeathScene

Some bugs were fixed after the in-class demo. These include:
- The melee attack sound would not play as intended
- Sprout's "take damage" sound would not play as intended
- Enemies would not play their sound effects as intended. The death/take damage sound now plays correctly, but the attack sound still seems to have issues. The decision has been made to leave this bug in.
- Sprout's Spell List seems to have mysteriously been cleared

The background music will only play if you run the game from the Title Sequence Animation scene. This technically is not a bug due to how the Audio Manager component works.
