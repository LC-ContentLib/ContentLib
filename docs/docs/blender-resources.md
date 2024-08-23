# 3D Modeling With Blender

This guide provides a comprehensive collection of resources to help you start creating 3D models in Blender, and details how to import these models into Unity.

> [!TIP]  
> **Reference Project:**  
> Check the ExampleEnemy Blender project included in the example enemy repository for reference. It can be found under [AssetSources/Blender](https://github.com/Hamunii/LC-ExampleEnemy/tree/main/AssetSources/Blender).

## Blender Basics

> [!TIP]  
> **Keyboard Shortcuts:**  
> Avoid pressing random keys, as Blender has numerous keyboard shortcuts that could lead to unexpected actions. However, mastering these shortcuts can greatly accelerate your workflow. Refer to this [Blender 4.1 Shortcuts Cheat Sheet](https://surf-visualization.github.io/blender-course/references/cheat-sheet-4.1.pdf) by SURF HPCV for quick reference.

If you are completely new to Blender, the following tutorial series will guide you through the basics:

### Blender 4.0 Beginner Donut Tutorial - Blender Guru (Playlist)

- [Part 1: Introduction](https://youtu.be/B0J27sf9N1Y?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) - Covers the very basics of Blender.
- [Part 2: Basic Modeling](https://youtu.be/tBpnKTAc5Eo?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) - Learn to model a donut.
- [Part 3: Modeling the Icing](https://youtu.be/AqJx5TJyhes?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z&t=42) - Introduces advanced modeling techniques for detailing.
- [Part 4: Sculpting](https://youtu.be/--GVNZnSROc?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) - Learn sculpting, especially useful for organic models.
- [Part 5: Shading](https://youtu.be/fsLO1F5x7yM?list=PLjEaoINr3zgEPv5y--4MKpciLaoQYZB1Z) - Covers materials, texturing, and UV unwrapping.

## Modeling

Here are some valuable tutorials to improve your modeling skills:

- [**Fast Character Modeling with the Skin Modifier in Blender**](https://youtu.be/DAAwy_l4jw4) - Joey Carlino  
  Learn a simple yet effective modeling technique, perfect for creating basic initial meshes for characters or creatures.
  
- [**2D Drawing to 3D Model Part 1 (Modeling the Shape)**](https://www.youtube.com/watch?v=AlPPYkZg9D4) - Eve Sculpts  
  This tutorial walks you through transforming a simple drawing into a 3D model.

### Modeling - Common Issues

**Q: My mesh looks inverted in Blender or when imported into Unity.**  

> **Solution:**  
> This is likely due to inverted normals. In Blender, select your mesh in Edit Mode, press `A` to select everything, and press `Shift+N` to recalculate your normals (ensure "Inside" is not selected).  
> If the problem persists after importing to Unity, it could be due to resizing your object negatively. To fix this, switch to Object Mode, select your object, press `Ctrl+A`, and choose "Apply Scale." Then, recalculate normals again in Blender.

## Materials, Texturing & UV Unwrapping

> [!NOTE]  
> **Shader Compatibility:**  
> Unity does not natively understand Blender's shader node system, except for the Principled BSDF. If you use other nodes, you must bake your material as a texture for it to work in Unity.  
> Note that Lethal Company automatically adds its own "style" to assets, so elaborate texturing might not be necessary.

### Modelling:Recommended Tutorials

- [**2D Drawing to 3D Model Part 2 (UVs and Textures)**](https://www.youtube.com/watch?v=GLxMYj-pBQs) - Eve Sculpts  
- [**Blender 4.0 - Texture Painting Quick Start Guide**](https://youtu.be/iwWoXMWzC_c) - Jamie Dunbar  
  A beginner-friendly introduction to texture painting.
- [**Blender 4.0: How to UV Unwrap Anything**](https://youtu.be/XleO7DBm1Us) - On Mars 3D  
  Learn the essentials of UV unwrapping.

## Rigging:Recommended Tutorials

Rigging is essential for creating movable, articulated models. The following tutorials will help you get started:

- [**Rigging for Beginners**](https://www.youtube.com/watch?v=mYgznqvbisM) - Ryan King Art  
  A detailed yet easy-to-follow tutorial on rigging humanoid models, applicable to any model type.
  
- [**Tutorial: My New Rigging Workflow in Blender**](https://youtu.be/BiPoPMnU2VI) - Polyfjord  
  Learn about inverse kinematics, particularly useful for rigging legs.
  
- [**Rigging for Impatient People - Blender Tutorial**](https://youtu.be/DDeB4tDVCGY) - Joey Carlino  
  A fast-paced but informative video, ideal after you've grasped the basics.

- [**How to Rig and Animate in Blender!**](https://youtu.be/1khSuB6sER0) - ProductionCrate  
  Covers rigging a humanoid character, resolving issues with automatic weights, and using inverse kinematics.

## Animation & NLA (Nonlinear Animation) Editor

> [!NOTE]  
> **NLA Editor Use:**  
> It's important to place individual animations in the NLA Editor so they can be used separately in Unity. The length of an animation in Unity is determined by the length set in the NLA Editor.

### Animating:Recommended Tutorials

- [**The Nuts and Bolts of Blender's Animation System**](https://youtu.be/p3m57yAcsi0) - CGDive  
  Provides an in-depth introduction to Blender's animation tools: Timeline, Dope Sheet, Graph Editor, NLA Editor, and Actions.
  
- [**Un-confusing the NLA Editor (Nonlinear Animation)**](https://youtu.be/tAo7HxxxA08) - CGDive  
  A more detailed guide to the NLA Editor, focusing on basic usage for Unity compatibility.

- [**Become a PRO at Animation in 25 Minutes | Blender Tutorial**](https://youtu.be/_C2ClFO3FAY) - CG Geek  
  Learn to animate a walk cycle using Timeline, Dope Sheet, and Graph Editor.

- [**Character Animation for Impatient People - Blender Tutorial**](https://youtu.be/GAIZkIfXXjQ) - Joey Carlino  
  A quick guide for those who want to animate without creating their own rigs.

### Animation - Common Issues

**Q: Objects in my model appear in different places in Unity compared to Blender.**  

> **Solution:**  
> This may occur if you have animated an object directly instead of using an armature. Try parenting your object to an armature and re-create the animation.

**Q: Some animations do nothing in Unity.**  

> **Solution:**  
> This could be due to having animations with identical names in the NLA Editor. Ensure all your animations have unique names.

## Exporting Assets For Unity

### Basic Export Instructions

1. Go to `File` -> `Export` -> `FBX (.fbx)`.
2. In the FBX export window, ensure that under the "Bake Animation" dropdown, `All Actions` is disabled if you have placed your animations in the NLA Editor.
3. The most crucial part is setting the correct transform options to ensure your model appears correctly in Unity:

    - Set `Forward` to `-Z Forward`.
    - Set `Up` to `Y Up`.

This aligns Blender's coordinate system with Unity's, ensuring your model is oriented correctly.

### Exporting an Updated Version of Your Model for Unity

> [!CAUTION]  
> **Backup Your Unity Project:**  
> Always back up your Unity project before importing updated models.

1. When making changes to your model in Blender, re-export the model by overwriting the previous FBX file.
2. Do **not** delete the previous model version or its `.meta` file.
3. Avoid overwriting the model directly within Unity to maintain all references. Instead, overwrite the FBX file from outside Unity.

---

### Additional Suggestions

1. **Images and Diagrams:** Ensure all images referenced in the guide are correctly linked and displayed. Diagrams explaining coordinate systems and export settings are particularly helpful.
2. **External Resources:** Consider including a brief description or key takeaways from each linked video tutorial, so users know what to expect before watching.
3. **Glossary or FAQ:** Consider adding a glossary of key terms or an FAQ section at the end to address common questions and clarify terminology.
