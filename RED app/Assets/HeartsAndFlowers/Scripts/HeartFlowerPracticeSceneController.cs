using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HeartFlowerPracticeSceneController : MonoBehaviour
{
    [SerializeField] string NextScene;

    [SerializeField] int practiceTrials = 4;
    [SerializeField] int minCorrectPracticeTrials = 3;
    [SerializeField] int testTrials = 12;
    [SerializeField] bool practiceHeart;
    [SerializeField] bool testHeart;
    [SerializeField] bool practiceFlower;
    [SerializeField] bool testFlower;
    [SerializeField] bool playExplanationOnAwake = true;
    [SerializeField] AudioSource explanationAudio;
    [SerializeField] AudioSource correctFeedback;
    [SerializeField] AudioSource incorrectFeedbackHeart;
    [SerializeField] AudioSource incorrectFeedbackFlower;
    [SerializeField] AudioSource heartLeftExplanation;
    [SerializeField] AudioSource heartRightExplanation;
    [SerializeField] AudioSource flowerLeftExplanation;
    [SerializeField] AudioSource flowerRightExplanation;
    [SerializeField] AudioSource practiceStartExplanation;
    [SerializeField] AudioSource testStartExplanation;
    [SerializeField] GameObject highlightLeft;
    [SerializeField] GameObject highlightRight;

    [SerializeField] float stimulusDistance = 5.0f;
    [SerializeField] HeartFlowerFixation fixCross;
    [SerializeField] HeartFlowerStimulus practiceHeartStim;
    [SerializeField] HeartFlowerStimulus testHeartStim;
    [SerializeField] HeartFlowerStimulus practiceFlowerStim;
    [SerializeField] HeartFlowerStimulus testFlowerStim;
    [SerializeField] HeartFlowerPracticeButton leftButton;
    [SerializeField] HeartFlowerPracticeButton rightButton;
    [SerializeField] DataLogger logger;

    // VARIABLES
    // Boolean to indicate whether feedback is currently playing.
    private bool feedbackPlaying = false;
    private bool gamePaused = false;
    // Details on the phases in this practice.
    private List<string> phases = new List<string>();
    private int phaseCounter = 0;
    private string currentPhase;
    // Trial specifics.
    private bool correct;
    private int originalPracticeTrials;
    private int correctCount = 0;
    private int currentTrial = 0;
    private string currentStimulus;
    private string currentStimulusLocation;
    // Place-holders for the stimuli.
    private HeartFlowerFixation fix;
    private HeartFlowerStimulus stim;
    // Create a random number generator with a seed so that all
    // participants will have the same trials.
    private System.Random randomNumber = new System.Random();
    // Floats that keep track of the starting and current time (in seconds),
    // and the onset of the most recent stimulus.
    private float _startTime;
    public float _currentTime;
    private float _mostRecentStimTime;
    private float _respTime;

    // Use this for initialization
    void Start()
    {
        // Set the starting time.
        _startTime = Time.realtimeSinceStartup;

        // Copy the number of practice trials.
        originalPracticeTrials = practiceTrials;

        // Construct the list of phases. Start with the general explanation.
        // Add the practice phases to the list.
        if (practiceHeart & practiceFlower)
        {
            phases.Add("selfMixedPractice");
        }
        else if (practiceHeart)
        {
            phases.Add("explanation");
            phases.Add("rightHeartPractice");
            phases.Add("leftHeartPractice");
            phases.Add("selfHeartPractice");
        }
        else if (practiceFlower)
        {
            phases.Add("explanation");
            phases.Add("rightFlowerPractice");
            phases.Add("leftFlowerPractice");
            phases.Add("selfFlowerPractice");
        }
        // Add the practice phases to the list.
        if (testHeart & testFlower)
        {
            phases.Add("mixedTest");
        }
        else if (testHeart)
        {
            phases.Add("heartTest");
        }
        else if (testFlower)
        {
            phases.Add("flowerTest");
        }

        // Hide the highlight.
        Highlight(false, false);

        // Write a header to the DataLogger.
        string[] header = { "phase", "trial", "stimulus", "stim_location", "pressed", "RT", "correct", "timeout" };
        logger.LogHeader(header);

        // Activate the buttons.
        ActivateButtons(true, true);

        // Start the explanation.
        if (playExplanationOnAwake)
        {
            // Set the current phase.
            currentPhase = phases[phaseCounter];
            // Start with the explanation.
            StartCoroutine(PlayExplanation());
        }
        else {
            // Set the current phase to -1.
            currentPhase = "notStarted";
            phaseCounter = -1;
            // The NextPhase function should increment the phaseCounter from
            // -1 to 0, and thus start from the intended first phase.
            NextPhase();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current time.
        _currentTime = Time.realtimeSinceStartup - _startTime;
    }

    // Function that buttons use to indicate clicks.
    public void OnResponse(string buttonName) {
        // Behaviour depends on the current phase.

        // Explanation before the practice starts.
        if (currentPhase == "explanation")
        {
            NextPhase();
        }

        // Prescriptive practice phase.
        else if (currentPhase == "rightHeartPractice" |
                 currentPhase == "leftHeartPractice" |
                 currentPhase == "leftFlowerPractice" |
                 currentPhase == "rightFlowerPractice")
        {
            // Correct responses.
            if (
                (currentPhase == "rightHeartPractice" & buttonName == "right") |
                (currentPhase == "leftFlowerPractice" & buttonName == "right") |
                (currentPhase == "leftHeartPractice" & buttonName == "left") |
                (currentPhase == "rightFlowerPractice" & buttonName == "left")
            )
            {
                // Play the correct response feedback. This automatically
                // advances to the next phase once the feedback playback is
                // over.
                PlayFeedback(true, currentStimulus);
                // Remove the current stimulus.
                stim.Begone();
            }
            // Incorrect response.
            else
            {
                PlayFeedback(false, currentStimulus);
            }
        }

        // Interactive practice or test phase.
        else if (currentPhase == "selfHeartPractice" |
                 currentPhase == "selfFlowerPractice" |
                 currentPhase == "selfMixedPractice" |
                 currentPhase == "heartTest" |
                 currentPhase == "flowerTest" |
                 currentPhase == "mixedTest")
        {
            // If the game was paused, the first click should unpause it, and
            // start the first trial.
            if (gamePaused)
            {
                // Hide any highlights that might have still been visible.
                Highlight(false, false);
                // Unpause the game.
                gamePaused = false;
                // Start the first trial.
                NextTrial();
            }
            else
            {
                // Only process the trial outcome if this is after a trial has
                // actually started.
                if (currentTrial > 0)
                {
                    // Remove the current stimulus, but only if it still exists.
                    if (stim != null)
                    {
                        stim.Begone();
                    }
                    // Ignore any clicks on buttons while there wasn't a
                    // stimulus anymore.
                    else {
                        return;
                    }
                    // Compute the response time.
                    _respTime = _currentTime - _mostRecentStimTime;
                    // Check if the response is correct.
                    if ((currentStimulus == "heart" & currentStimulusLocation == buttonName) |
                        (currentStimulus == "flower" & currentStimulusLocation != buttonName))
                    {
                        correct = true;
                    }
                    else
                    {
                        correct = false;
                    }
                    // Increment the correct counter.
                    if (correct)
                    {
                        correctCount++;
                    }
                    // Log the response.
                    string[] data = { currentPhase, currentTrial.ToString(), currentStimulus, currentStimulusLocation, buttonName, _respTime.ToString(), correct.ToString(), "False" };
                    logger.LogData(data);

                    // Deal with practice trials in a different way.
                    if (currentPhase == "selfHeartPractice" |
                        currentPhase == "selfFlowerPractice" |
                        currentPhase == "selfMixedPractice")
                    {
                        // If this is the last trial, count the number of correct trials.
                        if (currentTrial >= practiceTrials)
                        {
                            // If the number of correct trials is sufficient, advance to
                            // the next phase.
                            if (correctCount >= minCorrectPracticeTrials)
                            {
                                NextPhase();
                            }
                            // If the number of correct trials is insufficient, either
                            // restart the practice (individual heart or flower 
                            // practice), or end the experiment (mixed practice).
                            else
                            {
                                // In the mixed practice, the experiment ends if a
                                // participant didn't have enough correct responses.
                                if (currentPhase == "selfMixedPractice")
                                {
                                    // Advance to next scene.
                                    LoadNextScene();
                                }
                                // In the heart and flower practices, if the number 
                                // of correct trials is insufficient, the practice
                                // trials are extended.
                                else
                                {
                                    // Extend the practice trials.
                                    practiceTrials += originalPracticeTrials;
                                    // Reset the correct count.
                                    correctCount = 0;
                                }
                            }
                        }
                    }
                    // In test trials, we only have to check whether this might
                    // be the last trial. If it is, we advance to the next phase.
                    else
                    {
                        //Debug.Log("Trial " + currentTrial.ToString() + " / " + testTrials.ToString());
                        if (currentTrial >= testTrials)
                        {
                            NextPhase();
                        }
                    }

                    // If this is not the last trial in this phase, advance to
                    // the next trial.
                    if (
                        (((currentPhase == "selfHeartPractice") | 
                          (currentPhase == "selfFlowerPractice") | 
                          (currentPhase == "selfMixedPractice")) & 
                         (currentTrial < practiceTrials)) | 
                        (currentTrial < testTrials))
                    {
                        NextTrial();
                    }
                }
                // Advance to the next trial when currentTrial==0. This will
                // start the trials on the first click if the trial number is
                // somehow still at 0 (shouldn't happen now that the game is
                // paused directly after a phase transition).
                else
                {
                    NextTrial();
                }
            }
        }
    }

    // Function called by fixation crosses that time out.
    public void OnFixTimeout()
    {
        // Start the drawing and activation routine, and tell it to wait for
        // 0.5 seconds before starting.
        StartCoroutine(WaitThenActivate(0.5f));
    }

    // Function called by stimuli that time out.
    public void OnStimTimeout()
    {
        // De-activate the buttons.
        ActivateButtons(false, false);
        // Log the timeout.
        string[] data = { currentPhase, currentTrial.ToString(), currentStimulus, currentStimulusLocation, "None", "False", "True" };
        logger.LogData(data);
        // Advance to the next trial.
        NextTrial();
    }

    // Function to turn on or off button highlights.
    public void Highlight(bool turnOnLeft, bool turnOnRight) {
        highlightLeft.SetActive(turnOnLeft);
        highlightRight.SetActive(turnOnRight);
    }

    // Convenience function to activate or deactivate the buttons.
    private void ActivateButtons(bool leftActive, bool rightActive) {
        leftButton.ActivationStation(leftActive);
        rightButton.ActivationStation(rightActive);
    }

    private void DrawFixationCross()
    {
        // Initialise a fixation cross.
        fix = Instantiate(fixCross);
        fix.transform.position = new Vector3(0, 0, -10);
        // Pass the handle to this SceneController, so that the fixation cross
        // can report back when a timeout occurs.
        fix.SetSceneController(this);
    }

    private void DrawStimulus()
    {
        // Initialise a stimulus.
        if (currentPhase == "rightHeartPractice" |
            currentPhase == "leftHeartPractice" |
            currentPhase == "selfHeartPractice" |
            currentPhase == "rightFlowerPractice" |
            currentPhase == "leftFlowerPractice" |
            currentPhase == "selfFlowerPractice" |
            currentPhase == "selfMixedPractice") 
        {
            if (currentStimulus == "heart")
            {
                stim = Instantiate(practiceHeartStim);
            }
            else if (currentStimulus == "flower")
            {
                stim = Instantiate(practiceFlowerStim);
            }
        }
        else {
            if (currentStimulus == "heart")
            {
                stim = Instantiate(testHeartStim);
            }
            else if (currentStimulus == "flower")
            {
                stim = Instantiate(testFlowerStim);
            }
        }
        // Pass the handle to this SceneController, so that the stimulus can
        // report back when a timeout occurs.
        stim.SetSceneController(this);
        // Move the stimulus to its intended position.
        float posX = stimulusDistance;
        if (currentStimulusLocation == "left")
            posX *= -1;
        stim.transform.position = new Vector3(posX, 0, -10);

        // Activate the buttons.
        ActivateButtons(true, true);

        // Record the onset time.
        _mostRecentStimTime = _currentTime;
    }

    // Function to deal with phase increments.
    private void NextPhase()
    {
        // Deactivate highlights.
        Highlight(false, false);

        // Increment the phase.
        phaseCounter++;
        // Check whether the phase counter has surpassed the number of phases.
        if (phaseCounter >= phases.Count) {
            LoadNextScene();
        }
        // Update the current phase.
        currentPhase = phases[phaseCounter];

        // Do phase-specific stuff.
        switch (currentPhase)
        {
            case "rightHeartPractice":
                currentStimulus = "heart";
                currentStimulusLocation = "right";
                DrawStimulus();
                StartCoroutine(PlayTrialExplanation(currentStimulus, currentStimulusLocation));
                break;
            case "leftHeartPractice":
                currentStimulus = "heart";
                currentStimulusLocation = "left";
                DrawStimulus();
                StartCoroutine(PlayTrialExplanation(currentStimulus, currentStimulusLocation));
                break;
            case "selfHeartPractice":
                // Pause the game. This will make the next button
                // press start the first trial.
                gamePaused = true;
                // Set the current trial to 0.
                currentTrial = 0;
                // Play the practice start message.
                StartCoroutine(PlayPracticeStart());
                break;
            case "rightFlowerPractice":
                currentStimulus = "flower";
                currentStimulusLocation = "right";
                DrawStimulus();
                StartCoroutine(PlayTrialExplanation(currentStimulus, "left"));
                break;
            case "leftFlowerPractice":
                currentStimulus = "flower";
                currentStimulusLocation = "left";
                DrawStimulus();
                StartCoroutine(PlayTrialExplanation(currentStimulus, "right"));
                break;
            case "selfFlowerPractice":
                // Pause the game. This will make the next button
                // press start the first trial.
                gamePaused = true;
                // Set the current trial to 0.
                currentTrial = 0;
                // Play the practice start message.
                StartCoroutine(PlayPracticeStart());
                break;
            case "selfMixedPractice":
                // Pause the game. This will make the next button
                // press start the first trial.
                gamePaused = true;
                // Set the current trial to 0.
                currentTrial = 0;
                // Play the practice start message.
                StartCoroutine(PlayPracticeStart());
                break;
            case "heartTest":
                // Pause the game. This will make the next button
                // press start the first trial.
                gamePaused = true;
                // Set the current trial to 0.
                currentTrial = 0;
                // Play the test start message.
                StartCoroutine(PlayTestStart());
                break;
            case "flowerTest":
                // Pause the game. This will make the next button
                // press start the first trial.
                gamePaused = true;
                // Set the current trial to 0.
                currentTrial = 0;
                // Play the test start message.
                StartCoroutine(PlayTestStart());
                break;
            case "mixedTest":
                // Pause the game. This will make the next button
                // press start the first trial.
                gamePaused = true;
                // Set the current trial to 0.
                currentTrial = 0;
                // Play the test start message.
                StartCoroutine(PlayTestStart());
                break;
            default:
                Debug.Log("ERROR: Unrecognised phase: " + currentPhase);
                break;
        }
    }

    // Function to draw the next stimulus.
    private void NextTrial() {
        // Only do something if the game isn't paused.
        if (gamePaused == false)
        {
            // Increment the trial number.
            currentTrial++;
            // Choose the new trial parameters.
            if (currentPhase == "selfHeartPractice" |
                currentPhase == "heartTest")
            {
                currentStimulus = "heart";
                if (randomNumber.NextDouble() < 0.5)
                {
                    currentStimulusLocation = "left";
                }
                else
                {
                    currentStimulusLocation = "right";
                }
            }
            else if (currentPhase == "selfFlowerPractice" |
                     currentPhase == "flowerTest")
            {
                currentStimulus = "flower";
                if (randomNumber.NextDouble() < 0.5)
                {
                    currentStimulusLocation = "left";
                }
                else
                {
                    currentStimulusLocation = "right";
                }
            }
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
                if (randomNumber.NextDouble() < 0.5)
                {
                    currentStimulusLocation = "left";
                }
                else
                {
                    currentStimulusLocation = "right";
                }
            }
            // Draw a fixtion cross and wait until it disappears (and then some).
            // This will then draw the new stimulus after it disappears.
            DrawFixationCross();
        }
    }

    // Function to close the current scene and advance to the next.
    private void LoadNextScene() {
        // Close the logger.
        logger.Close();
        // Load the task.
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextScene);
    }

    // Function to play the explanation at the start.
    private IEnumerator PlayExplanation()
    {
        // Deactivate the buttons.
        ActivateButtons(false, false);
        // Play the explanation.
        explanationAudio.Play();
        // Wait while the audio is playing.
        while (explanationAudio.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        // Re-active the buttons.
        ActivateButtons(true, true);
        // Highlight the right button.
        Highlight(false, true);
    }

    // Function to play trial feedback.
    private void PlayFeedback(bool _correct, string stimulusType)
    {
        // Only play feedback if it isn't already playing.
        if (feedbackPlaying == false)
        {
            StartCoroutine(PlayFeedbackCoroutine(_correct, stimulusType, _correct));
        }
    }

    // // // // // //
    // COROUTINES  //
    // // // // // //

    private IEnumerator PlayPracticeStart() 
    {
        // Deactivate the buttons.
        ActivateButtons(false, false);
        // Remove any remaining highlights the right button.
        Highlight(false, false);
        // Play the explanation.
        practiceStartExplanation.Play();
        // Wait while the audio is playing.
        while (practiceStartExplanation.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        // Re-active the buttons.
        ActivateButtons(true, true);
    }

    private IEnumerator PlayTestStart()
    {
        // Deactivate the buttons.
        ActivateButtons(false, false);
        // Remove any remaining highlights the right button.
        Highlight(false, false);
        // Play the explanation.
        testStartExplanation.Play();
        // Wait while the audio is playing.
        while (testStartExplanation.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        // Re-active the buttons.
        ActivateButtons(true, true);
    }

    private IEnumerator PlayTrialExplanation(string stimulusType, string buttonLocation)
    {
        // Deactivate the buttons.
        ActivateButtons(false, false);
        // Play the explanation.
        AudioSource _audio;
        if (stimulusType == "heart" & buttonLocation == "left") {
            _audio = heartLeftExplanation;
        }
        else if (stimulusType == "heart" & buttonLocation == "right")
        {
            _audio = heartRightExplanation;
        }
        else if (stimulusType == "flower" & buttonLocation == "left")
        {
            _audio = flowerLeftExplanation;
        }
        else
        {
            _audio = flowerRightExplanation;
        }
        // Play the feedback.
        _audio.Play();
        // Wait while the audio is playing.
        while (_audio.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        // Highlight the correct button.
        if (buttonLocation == "left")
        {
            Highlight(true, false);
        }
        else {
            Highlight(false, true);
        }
        // Re-active the buttons.
        ActivateButtons(true, true);
    }

    private IEnumerator PlayFeedbackCoroutine(bool _correct, string stimulusType, bool advancePhase)
    {
        // Set the feedbackPlaying bool.
        feedbackPlaying = true;
        // Deactivate the buttons.
        ActivateButtons(false, false);
        // Play the explanation.
        AudioSource _audio;
        if (_correct)
        {
            _audio = correctFeedback;
        }
        else
        {
            if (stimulusType == "heart")
            {
                _audio = incorrectFeedbackHeart;
            }
            else
            {
                _audio = incorrectFeedbackFlower;
            }
        }
        // Play the feedback.
        _audio.Play();
        // Wait while the audio is playing.
        while (_audio.isPlaying) {
            yield return new WaitForSeconds(0.1f);
        }
        // Re-active the buttons.
        ActivateButtons(true, true);
        // Reset the feedbackPlaying bool.
        feedbackPlaying = false;

        // Optionally advance the phase.
        if (advancePhase) {
            NextPhase();
        }
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
        ActivateButtons(true, true);
    }

}
