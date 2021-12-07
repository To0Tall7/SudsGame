
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class VirusRun : MonoBehaviour
{
    //The anim variable is used to store the reference 
    //to the Animator component of the character. 
    private Animator anim;
    void Start()
    {
        //We get the component and assign it to 
        //the anim variable when the game starts 
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        //Change ->
        if (VirusSpeed > 0)
        {
            /*We cal the SetTrigger() function on the 
            Animator component stored in the anim 
            variable. The function requires one 
            parameter - the name of the trigger 
            parameter set in our Animator Controller 
            ("Wave" in our example). Make sure to match 
            it with the name of the parameter you've 
            created in your Animator Controller*/
            anim.SetTrigger("VirusRun");
        }
    }
}