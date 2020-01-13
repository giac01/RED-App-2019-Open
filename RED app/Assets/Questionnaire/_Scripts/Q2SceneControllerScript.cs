using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Q2SceneControllerScript : QQSceneManagerClass
{



    void Awake(){

    //Trial Questionnnaire Info
     questiontext = new List<string>(new string[] {
            "I finish all my class work.",
            "When I get a bad grade, I try even harder the next time.",
            "I keep working and working until I get my schoolwork right. ",
            "I do my class assignments even when they are really hard for me.", //4

            "I feel that nothing is much fun anymore",
            "I feel sad or empty",
            "I feel very tired",
            "I feel like I don’t want to move",
            "I have problems with my appetite", //9

            "I worry about bad things happening to me",
            "I am worried that something bad will happen to myself ",
            "I worry about what will happen",
            "I am worried that something awful will happen to my family",
            "I think about death", //14

			"You can always become more clever",
			"You have a certain amount of talent, and you can't do much to change it" //16


            });


     sliderselect = new int[] { 3, 3, 3, 3,
                                4, 4, 4, 4, 4,
                                4, 4, 4, 4, 4,
								9, 9

								}; //this variable selects the type of slider (from sliderlist) for each trial - e.g. binary slider, continuous slider, etc.
     AudioTag = "b.q";

    }


}
