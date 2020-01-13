using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancellationSceneController : MonoBehaviour {

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
    private int[] xPixelJitter = {66, 54, 25, 35, 32, 7, 7, 55, 8, 30, 54, 40,
        36, 24, 49, 20, 16, 62, 16, 68, 10, 66, 23, 29, 56, 47, 34, 73, 72, 23,
        41, 46, 42, 55, 10, 37, 69, 38, 45, 10, 47, 4, 8, 20, 2, 73, 50, 71, 4,
        43, 7, 0, 66, 34, 31, 4, 5, 0, 55, 70, 74, 37, 22, 2, 15, 68, 26, 39,
        42, 40, 37, 14, 68, 23, 41, 53, 63, 25, 37, 48};
    private int[] yPixelJitter = {70, 67, 62, 70, 52, 61, 29, 20, 21, 34, 4,
        60, 34, 4, 40, 27, 60, 29, 51, 74, 40, 31, 41, 61, 39, 55, 52, 13, 69,
        73, 33, 25, 28, 11, 2, 57, 38, 28, 35, 3, 45, 55, 53, 4, 21, 31, 22,
        49, 47, 3, 46, 30, 43, 38, 74, 0, 25, 59, 24, 48, 37, 24, 62, 14, 48,
        45, 52, 60, 34, 5, 35, 70, 25, 20, 74, 35, 40, 44, 66, 62};
    // The stimulus types are also hard-coded, and (as the other hard-coded
    // variables) generated elsewhere. The aim is to make sure that each column
    // has an equal amount of targets, so that they are equally spaced along
    // the x-axis.
    private string[] stimulusTypes = {
        "target", "distractor", "distractor", "target", "distractor", "target", "distractor", "target",

        "distractor", "distractor", "target", "target", "distractor", "target", "distractor", "target",

        "distractor", "target", "target", "target", "target", "distractor", "distractor", "distractor",

        "distractor", "target", "distractor", "distractor", "target", "target", "target", "distractor",

        "target", "distractor", "target", "distractor", "distractor", "target", "distractor", "target",

        "distractor", "target", "distractor", "target", "target", "distractor", "target", "distractor",

        "target", "target", "distractor", "distractor", "target", "distractor", "target", "distractor",

        "distractor", "distractor", "target", "target", "distractor", "distractor", "target", "target",

        "target", "distractor", "distractor", "target", "target", "target", "distractor", "distractor",

        "target", "distractor", "target", "distractor", "distractor", "target", "distractor", "target"};

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
