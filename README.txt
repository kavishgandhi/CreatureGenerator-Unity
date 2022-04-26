Project 3
Creatures Creation

Usage-
1. Put in the random seed
2. Hit generate to generate 9 different Creatures
3. Hit view to see the animations
4. Hit "Destroy" to destroy all created creatures

Animation- 
1. Wobbling of eyes
2. Lateral Movement of legs

Using Variations in-
1. Eyes (3)
2. Legs (3)
3. Hands (3)

Most of the time you will be able to see 2 different eyes and 3 different legs and 2 different Hands(if not keep on hitting "Generate" variations will be instantiated)
Used catmull clark subdivision to create curved surfaces from a cube (which is defined/hard coded in the code itself)
Used the curved surfaces to create prefabs of different body parts and then place them to get the final creature
Prefabs created and used-
1. Body Prefab - bodyA
2. Mouth Prefab - mouth
3. Legs Prefab - legs
4. Legs Variant Prefab 1 - legs_variant1
5. Legs Variant Prefab 2 - legs_variant2
6. Hands Prefab - hands
7. Hands Variant Prefab 1 - hands_variant1
8. Hands Variant Prefab 2 - hands_variant2
9. Eyes Prefab - eye
10. Eyes Variant Prefab 1 - eye_wobble_variant
11. Eyes Variant Prefab 2 - eye_variant


Subdivider Utility
Used catmull clark subdivision algorithm to subdivide any given surface and create a smooth curved surface
Uses vertices and quads representaiton for mesh

Usage-
1. Select the gameobject you want to subdivide and save (which should be the subdivider (transform))
2. Give the name of the mesh you want to save
3. Click on "Show Cube" to show the mesh, in its og form 
4. Click on "Subdivide" to subdivide the surface
5. Once you are happy, click on "Save" to save the mesh in Meshes Folder
6. You can then drag the mesh and drop it in Prefabs folder to create a prefab of the mesh
7. If you want to restart, click on "Delete & Restart"