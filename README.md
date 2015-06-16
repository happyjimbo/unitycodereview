# README #

This project is built with Unity 4.6, to open in Unity select 'Open Project' from the File menu in Unity and point to the root directory.

### What is this repository for? ###

This project is designed for the purpose of a code review.

### How do I get set up? ###

To play the game simply open the project in Unity and select play.

To run the Unit Tests you need to select 'Unity Test Tools' from the menubar and then select 'Unit Test Runner'. This will pop open a new UI where you can run the Unit Tests from within Unity. I would advise 'docking' this UI as a tab so that you can alway easily jump to it to run your tests. To run the tests, use the 'Play' button in the Unity Test Runner, this will change the scene to run the tests in order to make sure that we don't accidentally pollute our project hierarchy.

### Code guidelines ###

To view the Unit Tests, look in the 'Editor' -> 'unittests' directory in the 'Project' panel. The unit tests live here so that 
they're not compiled into a built version of our game and instead just live inside the editor environment.

To view where life begins, go to the 'Hierarchy' view, select the GameContext GameObject. In the Inspector you'll see that we have the 'GameContext' script attached - select this script to find our IoC Context which is where life begins for the game.

### What is missing? ###

Please note that there is no UI in this code example, this is due to the project originally using NGUI 3 (more advanced UI system than the recently released 'New UI' by Unity), which is paid for software and needs a license - of which I cannot guarantee that you the reader (or organisation) has. Please note that I'm a fan of the Presentation Model (http://martinfowler.com/eaaDev/PresentationModel.html), which enables us to have a place where we can separate our 'view' code from interactive with the system and is fantastic for creating unit tests without any mocking involved.

### Who do I talk to? ###

Email James Woodward at: james.ejw@gmail.com