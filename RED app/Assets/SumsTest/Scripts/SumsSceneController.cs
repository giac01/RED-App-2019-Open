using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumsSceneController : MonoBehaviour {

	[SerializeField] private DataLogger logger;
	[SerializeField] private SumsTimer timer;
	[SerializeField] private TextMesh sum;

	private bool _taskStopped = false;
	private string _currentAnswer = "";

	// Create a list of sums.
	string[] _sums = {
		"1 + 7",
		"5 - 2",
		"8 + 1",
		"5 - 0",
		"2 + 7",
		"1 + 5",
		"1 - 1",
		"1 + 4",
		"2 - 2",
		"3 - 1",
		"4 - 3",
		"1 + 1",
		"6 - 3",
		"3 + 0",
		"3 - 2",
		"4 - 4",
		"1 + 2",
		"6 - 1",
		"0 + 0",
		"4 + 2",
		"3 - 3",
		"6 + 1",
		"5 + 3",
		"3 - 0",
		"4 - 2",
		"4 + 3",
		"2 - 1",
		"2 + 2",
		"4 - 1",
		"0 + 5",
		"9 - 3",
		"7 + 7",
		"10 - 6",
		"3 + 9",
		"6 + 2",
		"7 - 4",
		"5 + 5",
		"8 - 3",
		"0 + 8",
		"5 - 1",
		"8 + 6",
		"9 - 4",
		"8 + 2",
		"7 - 2",
		"10 - 2",
		"4 + 4",
		"5 - 3",
		"4 + 8",
		"8 - 1",
		"6 + 6",
		"1 + 3",
		"10 - 10",
		"5 - 4",
		"5 + 7",
		"6 - 2",
		"8 - 6",
		"9 + 1",
		"6 - 6",
		"3 + 3",
		"5 + 2",
		"7 + 3",
		"1 x 3",
		"2 x 2",
		"10 - 5",
		"8 - 8",
		"4 x 1",
		"7 + 8",
		"1 x 5",
		"0 x 1",
		"6 - 5",
		"3 x 0",
		"6 + 4",
		"9 - 2", 
		"2 x 1", 
		"3 x 2", 
		"5 + 9", 
		"0 x 4", 
		"9 - 9", 
		"6 + 3", 
		"1 x 1", 
		"9 + 7", 
		"6 x 0", 
		"10 - 0", 
		"4 x 3", 
		"1 x 7", 
		"9 - 5", 
		"2 x 4", 
		"5 + 6", 
		"3 x 5", 
		"8 - 7", 
		"5 x 2", 
		"9 - 6", 
		"1 x 8", 
		"2 + 3", 
		"7 - 7", 
		"9 x 1", 
		"5 + 4", 
		"3 x 3", 
		"10 - 7", 
		"6 x 2", 
		"7 x 4", 
		"5 + 8", 
		"10 - 1", 
		"9 x 3", 
		"8 - 2", 
		"6 + 9", 
		"4 x 4", 
		"10 - 3", 
		"5 x 8", 
		"1 x 6", 
		"7 + 6", 
		"9 x 2", 
		"7 x 5", 
		"8 - 4", 
		"9 x 5", 
		"8 x 9", 
		"7 - 0", 
		"6 x 4", 
		"9 + 2", 
		"10 - 8", 
		"6 x 7", 
		"10 - 4", 
		"9 x 9", 
		"7 - 1", 
		"5 x 6", 
		"9 - 7", 
		"8 x 2", 
		"5 - 5", 
		"6 x 8", 
		"4 + 9", 
		"10 - 9", 
		"8 x 4", 
		"4 - 0", 
		"6 x 9", 
		"8 + 8", 
		"7 - 6", 
		"3 x 8", 
		"9 x 4", 
		"8 + 3", 
		"7 x 7", 
		"7 - 3", 
		"8 x 0", 
		"3 x 6", 
		"9 + 9", 
		"8 x 7", 
		"4 x 5", 
		"9 - 8", 
		"6 x 6", 
		"0 + 4", 
		"9 x 7", 
		"4 + 7", 
		"5 x 5", 
		"7 x 3", 
		"6 - 4", 
		"7 - 5", 
		"8 + 9", 
		"7 x 2", 
		"9 - 1", 
		"8 x 8", 
	};
	// Start with sum 0.
	private int _currentSum = 0;
	// Variable for the sum TextMesh handle.
	private TextMesh _sumText;

	// Use this for initialization
	void Start () {

		// Set the first sentence.
		sum.text = _sums[_currentSum] + " = ";

		// Log a header to the Logger.
		string[] header = {"Time", "Sum", "Response"};
		logger.LogHeader(header);

	}
	
	// Update is called once per frame
	void Update () {

	}

	// Public function to add a number to the answer.
	public void AddAnswerNumber(string number) {

		// Add the number to the answer.
		_currentAnswer = _currentAnswer + number;

		// Update the text.
		sum.text = _sums[_currentSum] + " = " + _currentAnswer;
	}

	// Public function to reset the answer.
	public void RemoveAnswer() {

		// Reset the answer.
		_currentAnswer = "";

		// Update the text.
		sum.text = _sums[_currentSum] + " = " + _currentAnswer;
	}

	// Public function to increment the sentence counter and
	// set the text for the next sentence.
	public void NextSum() {

		// Only do the following if the task is still ongoing AND if the current answer
		// is not empty.
		if ((_taskStopped == false) & (_currentAnswer != ""))
		{
			// Get the time, and convert it to milliseconds.
			int _time = (int)(timer.getCurrentTime() * 1000.0f);

			// Log the answer.
			string[] data = {_time.ToString(), _sums[_currentSum], _currentAnswer};
			logger.LogData(data);
			Debug.Log (data);

			// Reset the answer.
			_currentAnswer = "";

			// Increment the counter.
			_currentSum += 1;

			// Check if the iterator exceeds the number of sums.
			if (_currentSum >= _sums.Length)
			{
				// End the task if all sums have been presented.
				EndTask();
			}
			// Render the next sum.
			else 
			{
					// Set the new text.
					sum.text = _sums [_currentSum] + " = ";
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
			sum.text = "The time is up now!";

			// End the task.
			EndTask();
		}
	}

	// Private function to close the logger and load the next scene.
	private void EndTask(){

		// Close the logger.
		logger.Close();

		// Load the ending scene that follows.
		UnityEngine.SceneManagement.SceneManager.LoadScene("SumsStop");
	}
}
