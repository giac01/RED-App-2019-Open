using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartFlowerSceneController : MonoBehaviour {

    [SerializeField] int conPracticeTrials = 3;
    [SerializeField] int congruentTrials = 12;
    [SerializeField] int inconPracticeTrials = 3;
    [SerializeField] int incongruentTrials = 12;
    [SerializeField] int mixPracticeTrials = 10;
    [SerializeField] int mixTrials = 49;
    [SerializeField] float minPracticeAccuracy = 0.7f;

    [SerializeField] float stimulusDistance = 5.0f;
    [SerializeField] HeartFlowerFixation fixCross;
    [SerializeField] HeartFlowerStimulus heart;
    [SerializeField] HeartFlowerStimulus flower;
    [SerializeField] HeartFlowerButton leftButton;
    [SerializeField] HeartFlowerButton rightButton;
    [SerializeField] DataLogger logger;

    // VARIABLES
    private int minPracticeCorrectTrials;
    private string[] phases = { "conpractice", "congruent", "inconpractice", "incongruent", "mixpractice", "mix" };
    private int[] correctCount = { 0, 0, 0, 0, 0, 0 };
    private int currentTrial = -1;
    private int currentPhase = 0;
    private string currentStimulus;
    private string currentStimulusLocation;
    // Place-holders for the stimuli.
    private HeartFlowerFixation fix;
    private HeartFlowerStimulus stim;
    // Create a random number generator with a seed so that all
    // participants will have the same trials.
    private System.Random randomNumber = new System.Random(100);

    // Use this for initialization
    void Start () {
        // Compute the minimum number of trials in the mixed practice.
        minPracticeCorrectTrials = (int)Mathf.Floor(minPracticeAccuracy * (float)mixPracticeTrials);
        // Write a header to the DataLogger.
        string[] header = { "phase", "trial", "stimulus", "stim_location", "pressed", "RT", "correct", "timeout" };
        logger.LogHeader(header);
        // Start the first trial.
        NextTrial();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void NextTrial()
    {
        // Increment the trial number.
        currentTrial++;
        // Check if the trial number exceeds the maximum number of trials for
        // the current phase.
        if (
            (phases[currentPhase] == "conpractice" & currentTrial >= conPracticeTrials) |
            (phases[currentPhase] == "congruent" & currentTrial >= congruentTrials) |
            (phases[currentPhase] == "inconpractice" & currentTrial >= conPracticeTrials) |
            (phases[currentPhase] == "incongruent" & currentTrial >= incongruentTrials) |
            (phases[currentPhase] == "mixpractice" & currentTrial >= mixPracticeTrials) |
            (phases[currentPhase] == "mix" & currentTrial >= mixTrials)
        ){
            // In the mixed trial practice, the minPracticeAccuracy needs to be
            // met. Thus, we should check if there were enough correct trials.
            // We should only do this if the currentPhase counter hasn't
            // exceeded the total number of phases.
            if (phases[currentPhase] == "mixpractice" & correctCount[currentPhase] < minPracticeCorrectTrials)
            {
                // Increment the current phase by one more to skip over the
                // mixed trial round.
                currentPhase++;
            }
            // Reset the current trial number, and increment the current phase.
            currentTrial = 0;
            currentPhase++;
        }
        // Check if the current phase exceeds the number of phases in the
        // experiment.
        if (currentPhase >= phases.Length)
        {
            // End the experiment if the phases ran out.
            EndExperiment();
        }
        // Choose the next target.
        else
        {
            // If this is the first half of the practice or the congruent phase,
            // choose the heart.
            if (
                (phases[currentPhase] == "mixpractice" & currentTrial < mixPracticeTrials / 2) |
                (phases[currentPhase] == "conpractice") |
                (phases[currentPhase] == "congruent")
            )
            {
                currentStimulus = "heart";
            }
            // If this is the second half of the practice or the incongruent
            // phase, choose the flower.
            else if (
                (phases[currentPhase] == "inconpractice" & currentTrial >= mixPracticeTrials / 2) |
                (phases[currentPhase] == "inconpractice") |
                (phases[currentPhase] == "incongruent")
            )
            {
                currentStimulus = "flower";
            }
            // If this is any other phase (i.e. mix or mix practice), randomly
            // choose between the stimuli.
            else
            {
                if (randomNumber.NextDouble() < 0.5)
                {
                    currentStimulus = "heart";
                }
                else
                {
                    currentStimulus = "flower";
                }
            }

            // Randomly choose a side for the target to appear on.
            if (randomNumber.NextDouble() < 0.5)
            {
                currentStimulusLocation = "left";
            }
            else
            {
                currentStimulusLocation = "right";
            }
        }

        // Draw a fixation cross.
        DrawFixationCross();
    }

    private void DrawFixationCross() {
        // Initialise a fixation cross.
        fix = Instantiate(fixCross);
        fix.transform.position = new Vector3(0, 0, -10);
        // Pass the handle to this SceneController, so that the fixation cross
        // can report back when a timeout occurs.
//        fix.SetSceneController(this);
    }

    private void DrawStimulus() {
        // Initialise a stimulus.
        if (currentStimulus == "heart") {
            stim = Instantiate(heart);
        }
        else if (currentStimulus == "flower") {
            stim = Instantiate(flower);
        }
        // Pass the handle to this SceneController, so that the stimulus can
        // report back when a timeout occurs.
//        stim.SetSceneController(this);
        // Move the stimulus to its intended position.
        float posX = stimulusDistance;
        if (currentStimulusLocation == "left")
            posX *= -1;
        stim.transform.position = new Vector3(posX, 0, -10);
    }

    private void EndExperiment() {
        // Close the logger.
        logger.Close();
        // Advance to the next scene.
        UnityEngine.SceneManagement.SceneManager.LoadScene("HeartsAndFlowersEnd");
    }

    // Function called by fixation crosses that time out.
    public void OnFixTimeout()
    {
        // Start the drawing and activation routine, and tell it to wait for
        // 0.5 seconds before starting.
        StartCoroutine(WaitThenActivate(0.5f));
    }

    // Function called by stimuli that time out.
    public void OnStimTimeout(){
        // Log the timeout.
        string[] data = { phases[currentPhase], currentTrial.ToString(), currentStimulus, currentStimulusLocation, "None", "False", "True" };
        logger.LogData(data);
        // Deactivate the buttons.
        leftButton.ActivationStation(false);
        rightButton.ActivationStation(false);
        // Advance to the next trial.
        NextTrial();
    }

    // Function called by the response buttons.
    public void OnResponse(string button)
    {
        // Log the response.
        bool correct;
        if ((currentStimulus == "heart" & currentStimulusLocation == button) |
            (currentStimulus == "flower" & currentStimulusLocation != button))
        {
            correct = true;
        }
        else {
            correct = false;
        }
        string[] data = { phases[currentPhase], currentTrial.ToString(), currentStimulus, currentStimulusLocation, button, correct.ToString(), "False" };
        logger.LogData(data);
        // Add one to the corrent count.
        if (correct) {
            correctCount[currentPhase] += 1;
        }
        // Remove the stimulus.
        stim.Begone();
        // Deactivate the buttons.
        leftButton.ActivationStation(false);
        rightButton.ActivationStation(false);
        // Advance to the next trial.
        NextTrial();
    }

    // Coroutine to wait for a bit, and then draw a stimulus and activate the
    // response buttons.
    private IEnumerator WaitThenActivate(float delaySeconds)
    {
        // Wait for the requested amount of seconds.
        yield return new WaitForSeconds(delaySeconds);
        // Draw the stimulus.
        DrawStimulus();
        // Activate the buttons.
        leftButton.ActivationStation(true);
        rightButton.ActivationStation(true);
    }

}
