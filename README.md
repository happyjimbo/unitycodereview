# README #

This project is built with Unity 4.6, to open in Unity select 'Open Project' from the File menu in Unity and point to the root directory.

### What is this repository for? ###

This project is designed for the purpose of a code review.

### How do I get set up? ###

To play the game simply open the project in Unity and select play.

To run the Unit Tests you need to select 'Unity Test Tools' from the menubar and then select 'Unit Test Runner'. This will pop open a new UI where you can run the Unit Tests from within Unity. I would advise 'docking' this UI as a tab so that you can easily jump access the test runner. To run the tests, use the 'Play' icon button in the Unity Test Runner. Please note: this is set to run the tests on a new scene in order to prevent the possibility of polluting the project hierarchy.

### Code guidelines ###

To view the Unit Tests, look in the 'Editor' -> 'UnitTests' directory in the 'Project' panel. The unit tests live here so that 
they're not compiled into a built version of the game and instead just live inside the editor environment.

To view where life begins, go to the 'Hierarchy' view, select the GameContext GameObject. In the Inspector you'll find the 'GameContext' script attached - select this script to find our IoC Context which is where life begins for the game.

### What is missing? ###

Please note that there is no UI in this code example, this is due to the project originally using NGUI 3 (more advanced UI system than the recently released 'New UI' by Unity), which is paid for software and needs a license - of which I cannot guarantee that the reader (or organisation) has. Please note that I'm a fan of the Presentation Model (http://martinfowler.com/eaaDev/PresentationModel.html), which enables a separation of concern over and prevents our UI from directly interacting with system code (and visa versa) and is fantastic for creating unit tests without any mocking of view objects involved.

### How to Play? ###

Just simply select a tile and with your finger held down mouse/device select a matching tile that is in any of the 8 grid spaces next to the tile that you've currently selected. You need a minimum of 3 to make a successful match.

### Who do I talk to? ###

Email James Woodward at: james.ejw@gmail.com