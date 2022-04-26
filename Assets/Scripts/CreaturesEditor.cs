using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Creatures), true)]
public class CreaturesEditor : Editor
{
    public override void OnInspectorGUI(){
        Creatures creature = (Creatures)target;
        if (DrawDefaultInspector()){
        }
        if(GUILayout.Button ("Generate")){
            if(creature.creaturesList.Count>0){
                creature.destroy();
                creature.selectPrefabs();
            }
            else if(creature.creaturesList.Count==0){
                creature.selectPrefabs();
            }
        }
        if(GUILayout.Button ("Destroy")){
            creature.destroy();
        }
    }
}
