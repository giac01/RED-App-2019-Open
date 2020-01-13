using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Q1SceneControllerScript : QQSceneManagerClass
{

    void Awake()
    {
        questiontext = new List<string>(new string[] {
			"I have a regular bedtime routine.",
            "You can’t hear yourself think in our home.",
            "There is usually a television turned on somewhere in our home.",
            "The atmosphere in our house is calm.",

            "How many computers (PCs, Macs or laptops) does your family own?",
            "Does your family own a car, van or truck?",
            "Do you have your own bedroom for yourself?",
            "Does your home have a dishwasher?",

            "How many times did you travel abroad for holiday last year?",
            "How many bathrooms (room with a bath) are in your home?",
            "How many grownups (adults) live in your home?",
            "How many brothers and sisters do you have?",

			"When I finish my homework, I check it many times to see if I did it correctly",
			"I play only when I finished my homework",
			"I get easily distracted during class-time",
			"I keep my bedroom tidy",

			"Our home is messy.",
			"Our home is a good place to relax.",
			"Our home is quiet."
		
		
		});

        sliderselect = new int[] { 1, 1, 1, 1,
                                   2, 0, 0, 0,
                                   2, 2, 2, 2,
								   1, 1, 1, 1,
								   1, 1, 1

									
		
		
		}; //this variable selects the type of slider (from sliderlist) for each trial - e.g. binary slider, continuous slider, etc.

        AudioTag = "a.q";
    }


}
