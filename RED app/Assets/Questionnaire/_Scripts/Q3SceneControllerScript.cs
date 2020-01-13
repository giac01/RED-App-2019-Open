using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Q3SceneControllerScript : QQSceneManagerClass
{

    void Awake()
    {
		questiontext = new List<string> (new string[] {
			//"On a normal school day, how many hours do you spend: \nUsing a computer, tablet, or smartphone.",


			"Do you like to come to school?",
			"Do you wish you could stay home from school?",

			"On a normal school day, how many hours do you spend: \nreading alone at home?",
			"On a normal school day, how many hours do you spend: \nreading with a parent at home?",


			"How many children's books do you have at home?",

			"On a normal school day, how many hours do you spend: \nUsing a computer, tablet, or smartphone.",
			"On a normal school day, how many hours do you spend: \nWatching videos on the TV or internet (like Youtube, NetFlix, CBBC, or DVDs).",
			"On a normal school day, how many hours do you spend: \nChatting or interacting with friends on the internet (like on Facebook, Club Penguin, WhatsApp, or SnapChat).",
			"On a normal school day, how many hours do you spend: \nUsing a computer, laptop or tablet for doing schoolwork and other learning activities?",
			"On a normal school day, how many hours do you spend: \nPlaying games on a game console, computer, tablet, or phone.",
			"On a normal school day, how many hours do you spend: \nMaking things on a computer, tablet or smartphone. This can include: pictures, videos, podcasts, music, and writing (stories, blogs or poems).",

			"Do you have your own personal mobile phone?",

			"Do you use any of the following devices in your\nbedroom?",
			//"Click the food items that you are allergic to, or select 'I have no allergies' at the bottom. Click next when your done."
			"If you have any allergies, click on the ones you have in the list below, or select 'I Have No Allergies.'"
		});

        sliderselect = new int[] { //5,
								   4, 4,
                                   5, 5, 
								   6,
                                   5, 5, 5, 5, 5, 5,
                                   0,
								   7,8}; //this variable selects the type of slider (from sliderlist) for each trial - e.g. binary slider, continuous slider, etc.

        AudioTag = "c.q"; //YOU NEED TO TURN AUDIOLISTENER BACK ON FOR THIS QUSTION WHEN RECORDINGS READY
    }
}