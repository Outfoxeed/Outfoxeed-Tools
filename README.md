# 🦊 Outfoxeed Tools 🦊

This repository is a collection of tools and pre-made scripts imported or made for this repository for Unity:

- Tools enhancing Unity's interface and increase the speed of common manipulations: 
  - [Hierarchy Outliner](#hierarchy-outliner)
  - [GD Window](#gd-window)
  - [Data Editor Window](#data-editor-window)
  - [Toolbar buttons](#toolbar-buttons)
  - [SerializeReference ContextualMenu](#serializereference-contextualmenu)
- A collection of pre-made scripts to facilitate the creation of games:
  - [UI base](#-ui-base)
  - [Custom Attributes](#-custom-attributes)
  - GameManager base

---
## 🟠 IMPORTING
Import as a package in the Unity Package Manager with the git link 
(cf [link to it from github directly](https://docs.unity3d.com/Manual/upm-ui-giturl.html))
`https://github.com/outfoxeed/outfoxeed-tools.git`

OR

Download the zip file from the github page and unzip it in your assets folder.

---
## 🟠 TOOLS
<details id="hierarchy-outliner">
<summary>🟡 Hierarchy Outliner</summary>
Tool overriding the drawing of the GameObjects in the hierarchy of Unity. 
Change the colors, alignment, font style and more depending on the name of the object, its components and its tags.

Easily configurable through an EditorWindow.

<img src="Documentation~/HierarchyOutliner_Demo.png"/>
<img src="Documentation~/HierarchyOutliner_Rules.png" height="300"/>
</details>

<details id="gd-window">
<summary>🟡 GD Window</summary>
Window in which all assets labeled with 'GD' are displayed. Allowing easy access to all the ScriptableObjects a Game Designer would want to edit. 

Clicking on one of the button in the window shows the asset in the inspector.

![GD Window Example](Documentation~/GDWindow_Window.png)

#### Labeled Asset Example
![Asset Labeled to be visible in GD Window example](Documentation~/GDWindow_LabeledAssetExample.png)
</details>

<details id="data-editor-window">
<summary>🟡 Data Editor Window</summary>
Editor Window able to query all ScriptableObjects of given types in the project to edit game's data more easily.
It is also able to delete and add ScriptableObjects of the same given types.

The types of ScriptableObjects to show and the path where to create new instances are easily editable in another EditorWindow openable from the 'Config' button in the DataEditorWindow.

<img src="Documentation~/DataEditorWindow_Demo.png" height="300px">
<img src="Documentation~/DataEditorWindow_DemoConfig.png" height="200px">
</details>

<details id="toolbar-buttons">
<summary>🟡 Toolbar Buttons</summary>
Thanks to [Unity Toolbar Extender](https://github.com/marijnz/unity-toolbar-extender), we can easily add labels and buttons in the Unity toolbar. 
This package adds automatically one button for each scene in the build settings so you can switch of scene easily. 
</details>

<details id="serializereference-contextualmenu">
<summary>🟡 SerializeReference ContextualMenu</summary>
Using SerializeReference fields we can Serialize in the Inspector an abstract class.
But we need more stuff to just create an instance of a concrete class and assign it to the property.

This package contains the ability to recognize SerializeReference fields and to give you the power to 
set the concrete type of the field instance by right clicking on it

![SerializeReference ContextualMenu Image](Documentation~/SerializeReference_ContextualMenu.gif)
</details>

---
## 🟠 PRE-MADE SCRIPTS
### 🟡 UI Base
UI Manager MonoBehaviour storing different pages of the UI by enums and handling the spawning and destruction of the UI menus

UIMenu script used by the UIManager representing a page in the ui and with operations as a page switch/leave
### 🟡 Custom Attributes
- ReadOnly attribute (makes a field non editable in the Unity inspector)
- Label attribute (changes the label of a field in the Unity inspector)

---
## 🟠 DEPENDENCIES
[Unity Toolbar Extender](https://github.com/marijnz/unity-toolbar-extender) by [Marijn *marijnz* Zwemmer](https://github.com/marijnz) 
--> Buttons in the toolbars are made using this repository (unity-toolbar-extender is a subtree of this repository)

---
## 🟠 FUTURE
TODO:
- Import package or add a dependency to a package serializing System.Type and remake HierarchyOutlinerConfigWindow and DataEditorConfigWindow with it.