using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadingSceneController : MonoBehaviour {

	[SerializeField] private DataLogger logger;
	[SerializeField] private TextMesh sentence;

	private bool _taskStopped = false;

    // Create a list of sentences.
    //Indented lines have been added for jwalin, commented out lines removed 
    string[] _sentences = {
                                "You can eat a banana.",
                                "A dog can fly.",
                                "Cats have five legs.",
                                //"A shoe goes on your foot.",
                                //"An iPad has a screen.",
                                                "A shirt has collars.",
                                                "A TV has a screen.",

                                "A bird has arms.",
                                "The number '5' is a letter.",
                                "A circle is round.",
                                "Birds can lay eggs.",
                                "People hear with their ears.",
                                "A car floats on water.",
                                "Many children like to play games.",
                                "Sometimes the moon is purple.",
                                "Elephants are big.",
                                //"Poop can be stinky.",
                                "A kitten is a baby horse.",
                                "The sea has a lot of water.",
                                "The letter 'B' is the last letter of the alphabet.",
                                "An aeroplane flies in the sky.",
                                "A spoon can be used for eating",
                                "People can see cartoons on a television.",
                                "A crayon can be used for drawing.",
                                "Ants are big animals.",
                                "A cow has four legs.",
                                "A child can wear shorts.",
                                "Some trees grow fruits.",
                                "People like to write with a spoon.",
                                "Games can be played on a computer.",
                                "October is the month after January.",
                                "A lock can be opened with a key.",
                                "People can turn lights on with a button.",
                                //"Some ships have a sail.", 
                                "A bike has wheels.",
                                "Classrooms are always filled with water.",
                                "A pot can be empty",
                                "People dial a number to phone someone.",
                                "'D' is a letter of the alphabet.",
                                "A cow can eat grass.",
                                //"Nobody wears a coat in cold weather.",
                                                "Nobody uses an umbrella in rainy season",
                                "A glass can be made from shoe laces.",
                                "A child may like chocolates.",
                                "Dogs can walk on the ceiling.",
                                //"Bees eat salmon from the river.",
                                                "A butterfly has wings.",
                                "It can be cold inside a fridge.",
                                "Tuesday is the first month of the year.",
                                //"All crabs have three legs.",
                                "Many eagles wear clothes.",
                                "Bread can be used to make sandwiches.",
                                "A painting can be hung on a wall.",
                                "Most people laugh when they are angry.",
                                //"People should not eat yellow snow.",
                                "Children often go to space.",
                                "Some people write books.",
                                "Most grass is green.",
                                //"A bath can hold water.",
                                                "A jug can hold water.",
                                "Fish tend to live on land.",
                                "All dogs wear socks and shoes.",
                                "Some people wear raincoats in the rain.",
                                "Some children like reading.",
                                "Every person has a different name.",
                                "Most people sleep in trees.",
                                "Most bears ride cycles to work.",
                                "A spoon is needed to tell the time.",
                                //"A giant is much smaller than a dwarf.",
                                "Different plants can grow in a garden.",
                                "Some people like to walk through the garden.",
                                "The number '1' is the biggest number.",
                                "Orange juice can be served in a glass for breakfast.",
                                "A letter with a stamp cannot be posted.",
                                "Cats can eat with their mouths.",
                                "A dog usually spends money on stationary.",
                                "People usually sleep in their cars.",
                                "Ants are bigger than cats.",
                                //"All horse breeds are of the same height and weight.",
                                "Rotis can be served at lunch and at dinner.",
                                "Many trees grow hands on their branches.",
                                "Most schools have several classrooms.",
                                "A chicken meows like a cat.",
                                "Traffic lights always have purple and yellow lights.",
                                "A carpenter can use a saw and a hammer.",
                                "The job of a painter is to help you buy groceries.",
                                "An elephant is very light.",
                                "A sailor works on a boat.",
                                "Some people play games on their mobile phones.",
                                "An electrician can fix a broken doorbell.",
                                //"A light bulb belongs in the toilet bowl.",
                                "A cupboard can be used to house your parents.",
                                "All chocolate tastes very sour.",
                                "A television can be used to watch cartoons.",
                                "Some trees lose their leaves in winter.",
                                "Telephones can be used to cut vegetables.",
                                "Toys fall from the sky on Saturdays.",
                                "A teacher can work at a school.",
                                "Some people enjoy reading an interesting book.",
                                "You can see tigers at some zoos.",
                                "Many women wear saris.",
                                "Magazines are printed on spoons.",
                                "Fishes live under water.",
                                "Horses only ever eat pigeons.",
                                "Cats lay eggs.",
                                "Teachers work in factories.",
                };

    // Start with sentence 0.
    private int _currentSentence = 0;
	// Variable for the sentence TextMash handle.
	private TextMesh _sentenceText;

	// Use this for initialization
	void Start () {

		// Set the first sentence.
		sentence.text = _sentences[_currentSentence];

		// Log a header to the Logger.
		string[] header = {"Time", "Sentence", "Response"};
		logger.LogHeader(header);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Public function to increment the sentence counter and
	// set the text for the next sentence.
	public void NextSentence() {

		// Only do the following if the task is still ongoing.
		if (_taskStopped == false)
		{
			// Increment the counter.
			_currentSentence += 1;

			// Check if the iterator exceeds the number of sentences.
			if (_currentSentence >= _sentences.Length)
			{
				// End the task if all sentences have been presented.
				EndTask();
			}
			// Render the next sentence.
			else 
			{
					// Set the new text.
					sentence.text = _sentences [_currentSentence];
			}
		}
	}
		
	// Public function to signal the end of the task.
	public void TimeUp() {
		// Only do the following once, and ignore further calls.
		if (_taskStopped == false)
		{
			// Set the task stopped Boolean.
			_taskStopped = true;

			// Present an ending text
			// (kinda futile, as the task will end directly after this).
			sentence.text = "The time is up now!";

			// End the task.
			EndTask();
		}
	}

	// Private function to close the logger and load the next scene.
	private void EndTask(){

		// Close the logger.
		logger.Close();

		// Load the scene that follows the cancellation task.
		UnityEngine.SceneManagement.SceneManager.LoadScene("ReadingStop");
	}
}
