# Instructions for Blender to Unity
I forget how to do this stuff each time I go back to it since I don't do it very often. These instructions act as a reminder to help.

## Exporting FBX File
### Animations
Ensure all animations have a fake user, then remove them from NLA editor
1. Select animations from Action Editor
2. Click down arrow to add animation to list
3. 

### Export Settings
- Selected Objects
- FBX Units Scale
- Z Forward
- Y Up
- Apply Unit
- Use Space Transform
- Animations
	- NLA Strips
	- Deselect All Actions

## Creating New Animations
Make sure the Non-Linear Animation list items are unchecked (they supercede what's playing in the Animation window

## Importing FBX File
- Materials
	- Material Creation: None
- Model
	- Bake Axis Conversion (if Y Forward, Z Up)