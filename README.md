UI Object Templating Tool Documentation
Overview
The UI Object Templating Tool is a custom editor window in Unity designed to streamline the creation, customization, and instantiation of UI object hierarchies. With this tool, users can easily define object templates, modify their properties, and generate instances of these templates within the Unity Editor.

Getting Started
Installation:

Place the UIObjectEditorWindow.cs script in the Editor folder of your Unity project.
Opening the UI Object Editor Window:

Navigate to Window > UI Object Editor in the Unity Editor to open the UI Object Editor window.
Creating and Editing Templates
Creating a New Template
Click on "Create New Template":

Press the "Create New Template" button to generate a new UI object template.
Edit Template Properties:

Customize the template's properties such as name, position, rotation, scale, and other UI attributes using the provided fields in the editor.
Loading and Saving Templates
Load from JSON:

Click on "Load JSON" to load a UI object template from a JSON file. Select the desired JSON file when prompted.
Save to JSON:

Click on "Save JSON" to save the currently edited template to a JSON file. Enter a file name and location when prompted.
Instantiating UI in the Scene
Click on "Instantiate UI":

Press the "Instantiate UI" button to create an instance of the template in the Unity scene. Ensure that a Canvas exists in the scene.
Customize Instantiated Objects:

Adjust the position, rotation, and scale of the instantiated objects using the "Customize Instantiated Objects" section.
Apply Changes:

Press the "Apply Changes" button to apply modifications to the instantiated objects in the scene.
Error Handling
The tool includes error handling mechanisms for missing or corrupted JSON files. If an error occurs during JSON file operations, informative error messages will be displayed in the Unity Console.
Notes
Ensure that the UIObjectEditorWindow.cs script is located in the Editor folder to enable its functionality within the Unity Editor.

The tool assumes the presence of a Canvas and an EventSystem in the scene for UI instantiation.

Customize the template's default values and the instantiation process based on your specific UI requirements.
