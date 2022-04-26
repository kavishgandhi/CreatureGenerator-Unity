using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creatures : MonoBehaviour
{
    public GameObject bodyPrefab;
    public GameObject mouthPrefab;
    public GameObject legsPrefab;
    public GameObject legsVariantPrefab1;
    public GameObject legsVariantPrefab2;
    public GameObject handsPrefab;
    public GameObject handsVariantPrefab;
    public GameObject handsVariantPrefab2;
    public GameObject eyesPrefab;
    public GameObject eyesVariantPrefab1;
    public GameObject eyesVariantPrefab2;
    public int seed = 12;
    public List<GameObject> creaturesList = new List<GameObject>();
    private GameObject bodyPrefabToUse;
    private GameObject eyesPrefabToUse;
    private GameObject handsPrefabToUse;
    private GameObject legsPrefabToUse;
    private int creatureType;
    private bool eyeFlagV = false;
    private bool eyeFlagW = false;
    private Color color_;
    private float randomness;
    public void selectPrefabs(){
        int position = -4;
        for (int i=0;i<9;i++)
        {
            randomness = Random.Range(0.0f, (float)seed)/(float)seed;
            if(randomness<0.51)
            {
                creatureType = 4;
                if(randomness<0.33){
                    bodyPrefabToUse = bodyPrefab;
                    eyesPrefabToUse = eyesVariantPrefab1;
                    legsPrefabToUse = legsVariantPrefab1;
                    eyeFlagW = true;
                }
                else if(randomness>0.33 && randomness<0.67){
                    bodyPrefabToUse = bodyPrefab;
                    eyesPrefabToUse = eyesVariantPrefab2;
                    legsPrefabToUse = legsVariantPrefab2;
                    eyeFlagV = true;
                }
                else{
                    bodyPrefabToUse = bodyPrefab;
                    eyesPrefabToUse = eyesPrefab;
                    legsPrefabToUse = legsPrefab;
                }
            }
            else
            {
                creatureType = 2;
                if(randomness<0.33){
                    bodyPrefabToUse = bodyPrefab;
                    eyesPrefabToUse = eyesVariantPrefab1;
                    handsPrefabToUse = handsPrefab;
                    legsPrefabToUse = legsVariantPrefab1;
                    eyeFlagW = true;
                }
                else if(randomness>0.33 && randomness<0.67){
                    bodyPrefabToUse = bodyPrefab;
                    eyesPrefabToUse = eyesVariantPrefab2;
                    handsPrefabToUse = handsVariantPrefab;
                    legsPrefabToUse = legsVariantPrefab2;
                    eyeFlagV = true;
                }
                else{
                    bodyPrefabToUse = bodyPrefab;
                    eyesPrefabToUse = eyesPrefab;
                    handsPrefabToUse = handsVariantPrefab2;
                    legsPrefabToUse = legsPrefab;
                }
            }
            color_ = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            generate(position);
            position = position+2;
            eyeFlagV = eyeFlagW = false;
        }
    }
    void generate(int position){
        if(creatureType==4)
        {
            GameObject body = Instantiate(bodyPrefabToUse, new Vector3((float)position,1.0f,(float)position), Quaternion.Euler(0,0,0));
            body.transform.parent = transform;
            body.GetComponent<MeshRenderer>().sharedMaterial.color = color_;
            creaturesList.Add(body);
            if(randomness>0.25){
                GameObject leg1 = Instantiate(legsPrefabToUse, new Vector3((float)position+0.11f,1.0f-0.66f,(float)position+1.45f), Quaternion.Euler(0,0,0));
                leg1.transform.parent = transform;
                leg1.transform.localScale = new Vector3(2.0f,1.0f,2.0f);
                creaturesList.Add(leg1);
            }
            else{
                GameObject leg1 = Instantiate(legsPrefabToUse, new Vector3((float)position+0.18f,1.0f-0.66f,(float)position+1.45f), Quaternion.Euler(0,0,0));
                leg1.transform.parent = transform;
                creaturesList.Add(leg1);
            }
            if(randomness>0.25){
                GameObject leg2 = Instantiate(legsPrefabToUse, new Vector3((float)position+0.11f+0.54f,1.0f-0.66f,(float)position+1.45f), Quaternion.Euler(0,0,0));
                leg2.transform.parent = transform;
                leg2.transform.localScale = new Vector3(2.0f,1.0f,2.0f);
                creaturesList.Add(leg2);
            }
            else{
                GameObject leg2 = Instantiate(legsPrefabToUse, new Vector3((float)position+0.18f+0.54f,1.0f-0.66f,(float)position+1.45f), Quaternion.Euler(0,0,0));
                leg2.transform.parent = transform;
                creaturesList.Add(leg2);
            }
            GameObject leg3 = Instantiate(legsPrefabToUse, new Vector3((float)position+0.18f+0.54f,1.0f-0.66f,(float)position+1.45f-0.54f), Quaternion.Euler(0,0,0));
            leg3.transform.parent = transform;
            creaturesList.Add(leg3);
            GameObject leg4 = Instantiate(legsPrefabToUse, new Vector3((float)position+0.18f,1.0f-0.66f,(float)position+1.45f-0.54f), Quaternion.Euler(0,0,0));
            leg4.transform.parent = transform;
            creaturesList.Add(leg4);
            
            if(eyeFlagW)
            {
                GameObject eyesVariantLeft = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.3f, 1.0f+0.6f, (float)position-0.027f), Quaternion.Euler(0,0,0));
                eyesVariantLeft.transform.parent = transform;
                creaturesList.Add(eyesVariantLeft);
                GameObject eyesVariantMid = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.3f+0.2f, 1.0f+0.6f, (float)position-0.027f-0.2f), Quaternion.Euler(0,0,0));
                eyesVariantMid.transform.parent = transform;
                creaturesList.Add(eyesVariantMid);
                GameObject eyesVariantRight = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.3f+0.4f, 1.0f+0.6f, (float)position-0.027f), Quaternion.Euler(0,0,0));
                eyesVariantRight.transform.parent = transform;
                creaturesList.Add(eyesVariantRight);
            }
            else if(eyeFlagV)
            {
                GameObject eyesVariantMid = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.48f, 1.0f+1.0f, (float)position+0.36f), Quaternion.Euler(90,90,0));
                eyesVariantMid.transform.parent = transform;
                creaturesList.Add(eyesVariantMid);
            }
            else
            {
                GameObject eyesVariantLeft = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.25f, 1.0f+0.66f, (float)position+0.21f), Quaternion.Euler(0,0,0));
                eyesVariantLeft.transform.parent = transform;
                creaturesList.Add(eyesVariantLeft);
                GameObject eyesVariantRight = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.25f+0.4f, 1.0f+0.66f, (float)position+0.21f), Quaternion.Euler(0,0,0));
                eyesVariantRight.transform.parent = transform;
                creaturesList.Add(eyesVariantRight);
            }

            GameObject mouth = Instantiate(mouthPrefab, new Vector3((float)position+0.69f, 1.0f+0.36f, (float)position-0.27f), Quaternion.Euler(0,-75.0f,0));
            mouth.transform.parent = transform;
            creaturesList.Add(mouth);
        }
        else
        {
            GameObject body = Instantiate(bodyPrefabToUse, new Vector3((float)position,1.5f,(float)position), Quaternion.Euler(25.172f,0,0));
            body.transform.parent = transform;
            body.GetComponent<MeshRenderer>().sharedMaterial.color = color_;
            creaturesList.Add(body);
            
            GameObject leg1 = Instantiate(legsPrefabToUse, new Vector3((float)position+0.18f,1.0f-0.66f,(float)position+1.45f), Quaternion.Euler(0,0,0));
            leg1.transform.parent = transform;
            if(randomness>0.5){
                leg1.transform.localScale = new Vector3(2.0f,1.0f,2.0f);
            }
            creaturesList.Add(leg1);
            GameObject leg2 = Instantiate(legsPrefabToUse, new Vector3((float)position+0.18f+0.54f,1.0f-0.66f,(float)position+1.45f), Quaternion.Euler(0,0,0));
            leg2.transform.parent = transform;
            creaturesList.Add(leg2);
            
            GameObject hand1 = Instantiate(handsPrefabToUse, new Vector3((float)position+0.472f,1.5f-0.69f,(float)position+0.23f), Quaternion.Euler(-16.214f,0,0));
            hand1.transform.parent = transform;
            hand1.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            creaturesList.Add(hand1);
            GameObject hand2 = Instantiate(handsPrefabToUse, new Vector3((float)position+0.472f-0.5f,1.5f-0.69f,(float)position+0.23f), Quaternion.Euler(-16.214f,0,0));
            hand2.transform.parent = transform;
            hand2.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            creaturesList.Add(hand2);
            
            if(eyeFlagW)
            {
                GameObject eyesVariantLeft = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.3f, 1.0f+0.6f, (float)position-0.027f), Quaternion.Euler(0,0,0));
                eyesVariantLeft.transform.parent = transform;
                creaturesList.Add(eyesVariantLeft);
                GameObject eyesVariantMid = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.3f+0.2f, 1.0f+0.6f, (float)position-0.027f-0.2f), Quaternion.Euler(0,0,0));
                eyesVariantMid.transform.parent = transform;
                creaturesList.Add(eyesVariantMid);
                GameObject eyesVariantRight = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.3f+0.4f, 1.0f+0.6f, (float)position-0.027f), Quaternion.Euler(0,0,0));
                eyesVariantRight.transform.parent = transform;
                creaturesList.Add(eyesVariantRight);
            }
            else if(eyeFlagV)
            {
                GameObject eyesVariantMid = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.48f, 1.0f+1.243f, (float)position+0.36f), Quaternion.Euler(90,90,0));
                eyesVariantMid.transform.parent = transform;
                creaturesList.Add(eyesVariantMid);
            }
            else
            {
                GameObject eyesVariantLeft = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.25f, 1.0f+0.66f, (float)position+0.21f), Quaternion.Euler(0,0,0));
                eyesVariantLeft.transform.parent = transform;
                creaturesList.Add(eyesVariantLeft);
                GameObject eyesVariantRight = Instantiate(eyesPrefabToUse, new Vector3((float)position+0.25f+0.4f, 1.0f+0.66f, (float)position+0.21f), Quaternion.Euler(0,0,0));
                eyesVariantRight.transform.parent = transform;
                creaturesList.Add(eyesVariantRight);
            }

            GameObject mouth = Instantiate(mouthPrefab, new Vector3((float)position+0.69f, 1.5f-0.01f, (float)position-0.02f), Quaternion.Euler(0,-75.0f,0));
            mouth.transform.parent = transform;
            creaturesList.Add(mouth);
        }   
    }
    public void destroy(){
        for(int i = 0; i <  creaturesList.Count; i++)
        {
            DestroyImmediate(creaturesList[i]);
        }
        creaturesList.Clear();    
    }
    void OnValidate(){
        if(seed<=1){
            seed=1;
        }
    }
}
