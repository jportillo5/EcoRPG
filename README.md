# EcoRPG
 The repository for the EcoQuest project for the Software Development Capstone course.

 I had attempted to include a runnable build of the game in this repository, however it was ignored when attempting to push it to the repository, and the file size (even as a zip folder) were too big to insert into the repository manually, so you'll have to open up the project through Unity itself. You'll need the Unity Hub desktop application and working Unity License. Additionally, you'll need an editor that supports C# (I recommend Visual Studio Code, NOT Visual Studio). After downloading our project or cloning the repository, from the Unity Hub main page, click on the Add drop menu and select "Add project from disk", then find the EcoRPG folder. You'll then be able to open the project. To create a runnable build, go to "File" and then "Build Settings", then click "Build", there shouldn't be any need to mess with the Build settings.

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
