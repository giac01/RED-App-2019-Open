using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancellationSceneControllerInvisible: MonoBehaviour {

	/// <summary>
	/// SceneController draws all targets and distractors.
	/// </summary>

	// SERIALIZED FIELDS
	[SerializeField] private Sprite target;
	[SerializeField] private Sprite cancelledTarget;
	[SerializeField] private Sprite[] distractors;
	[SerializeField] private Sprite[] cancelledDistractors;
	[SerializeField] private CancellationStimulus originalStimulus;
	[SerializeField] private DataLogger logger;
	[SerializeField] bool isVisible;

	// CONSTANTS
	// The screen dimensions in pixels. Note that these must be made to
	// correspond with the settings manually!
	private int screenPixelWidth = 2048;
	private int screenPixelHeight = 1536;
	// The horizontal (x) and vertical (y) positions of the stimuli have been
	// hard-coded to make sure that each participant performs the same task.
	private int[] xPixelPos = {186, 372, 558, 744, 930, 1116, 1302, 1488, 1674,
		1860};
	private int[] yPixelPos = { 259, 418, 577, 736, 895, 1054, 1213, 1372 };

	// Horizontal and vertical jitter is applied to each of the stimulus
	// positions to make sure that the stimuli do not appear to grid-like. The
	// values are again hard-coded to make sure that each participant performs
	// the exact same task.
	private int[] xPixelJitter = {73, 36, 21, 73, 71, 29, 16, 3, 50, 74, 61,
		14, 5, 0, 41, 61, 43, 34, 63, 55, 74, 35, 26, 66, 46, 43, 30, 28, 22,
		19, 30, 15, 10, 72, 49, 6, 8, 40, 5, 60, 69, 68, 27, 29, 6, 62, 42, 40,
		48, 17, 69, 13, 13, 72, 36, 16, 66, 5, 18, 25, 52, 25, 27, 22, 6, 38,
		24, 34, 52, 18, 43, 47, 55, 1, 68, 53, 54, 42, 48, 27};
	private int[] yPixelJitter = {9, 70, 23, 28, 62, 25, 29, 53, 70, 69, 54,
		23, 27, 56, 19, 15, 62, 26, 71, 74, 56, 32, 63, 67, 41, 72, 52, 63, 10,
		29, 59, 23, 0, 62, 66, 18, 25, 13, 45, 5, 23, 53, 41, 61, 68, 8, 61, 0,
		37, 60, 40, 59, 15, 74, 0, 34, 23, 48, 61, 25, 49, 58, 48, 34, 13, 67,
		52, 54, 38, 55, 53, 42, 10, 28, 9, 23, 47, 41, 18, 14};
	// The stimulus types are also hard-coded, and (as the other hard-coded
	// variables) generated elsewhere. The aim is to make sure that each column
	// has an equal amount of targets, so that they are equally spaced along
	// the x-axis.
	private string[] stimulusTypes = {"distractor", "target", "target",
		"target", "distractor", "distractor", "distractor", "target",
		"distractor", "distractor", "target", "target", "target", "distractor",
		"distractor", "target", "distractor", "target", "distractor", "target",
		"target", "distractor", "target", "distractor", "distractor",
		"distractor", "distractor", "target", "target", "target", "target",
		"distractor", "distractor", "target", "target", "target", "distractor",
		"target", "distractor", "distractor", "distractor", "distractor",
		"distractor", "target", "target", "target", "distractor", "target",
		"distractor", "distractor", "target", "distractor", "target", "target",
		"distractor", "target", "distractor", "target", "distractor", "target",
		"target", "distractor", "distractor", "target", "distractor", "target",
		"distractor", "distractor", "target", "target", "target", "distractor",
		"target", "target", "target", "distractor", "distractor", "distractor",
		"distractor", "target"};

	// The depth is hard-coded, and determines what other information elements
	// can obscure the stimuli.
	private int posZ = -10;


	// VARIABLES
	// The stimulus number is used as a counter to keep track of the current
	// stimulus number, and is used as an index for x/y PixelJitter.
	private int stimNumber = 0;


	// FUNCTIONS

	// Use this for initialization
	void Start()
	{
		// TODO: Draw the task grid.
		for (int i = 0; i < xPixelPos.Length; i++)
		{
			for (int j = 0; j < yPixelPos.Length; j++)
			{
				// Generate a local variable for the current stimulus.
				CancellationStimulus stim;
				// Get the original stimulus on the first iteration.
				if (i == 0 && j == 0)
				{
					stim = originalStimulus;
				}
				// Initialise a new stimulus on all other iterations.
				else
				{
					stim = Instantiate(originalStimulus) as CancellationStimulus;
				}

				// Get the stimulus type.
				string stimulusType = stimulusTypes[stimNumber];

				// Set the stimulus' position and sprites.
				if (stimulusType == "target")
				{
					stim.SetStimulus(
						xPixelPos[i] + xPixelJitter[stimNumber],
						yPixelPos[j] + yPixelJitter[stimNumber],
						stimulusType,
						target,
						cancelledTarget);
				}
				else if (stimulusType == "distractor")
				{
					int index = Random.Range(0, distractors.Length);
					stim.SetStimulus(
						xPixelPos[i] + xPixelJitter[stimNumber],
						yPixelPos[j] + yPixelJitter[stimNumber],
						stimulusType, 
						distractors[index],
						cancelledDistractors[index]
					);
				}

				// Determine the stimulus position. This needs to be recomputed
				// from a position in pixels with (0,0) as the top left to a
				// position in the Unity coordinate system with (0,0) in the
				// centre, and 100 pixels per unit of measurement. Recomputing
				// entails dividing the pixel distances by 100, and subtracting
				// half the screen size from the top-left-origin coordinates.
				float posX = ((xPixelPos[i] + xPixelJitter[stimNumber]) / 
					100.0f) - (screenPixelWidth / 200.0f);
				float posY = ((yPixelPos[j] + yPixelJitter[stimNumber]) / 
					100.0f) - (screenPixelHeight / 200.0f);
				// Invert the y-axis, because Unity counts in the opposite
				// direction from the coordinate system we're working in.
				posY *= -1;
				// Move the stimulus to its new position.
				stim.transform.position = new Vector3(posX, posY, posZ);

				// Update the stimulus counter.
				stimNumber++;
			}
		}
		// Write header to the DataLogger.
		string[] header = {"time", "x", "y", "stimulus"};
		logger.LogHeader(header);
	}

	// Update is called once per frame
	void Update ()
	{

	}

}
