using System; 
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Networking;  

using SimpleJSON; 

//Stuff for Alpha file uploader (3rd party, not compliant for actual participant data)



public class DataUploader : MonoBehaviour {

	// SERIALISED FIELDS
	[SerializeField] TextMesh debugFileNameText;
	[SerializeField] TextMesh debugFileContentText;
	[SerializeField] Encryptomatic stringCipher;


	// VARIABLES
	private int _currentIndex = 0;
	private string[] _allFileNames;
	private string thisName;
	private string thisFileName; 
	private string thisString; 
	// TODO: Obtain the password through a prompt.
	private string _passPhrase = "ClownsAreScary";
	private bool _encrypt = true;
	private bool _currentlyUploading = false;


	// Use this for initialization
	void Start () {
		//string directory = string.Format (@"{0}", Application.persistentDataPath);
		DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath);
		// Get all text files in the persistent data path.
		FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
		// Compile a list of all file names.
		List<string> fileList = new List<string>();
		foreach(FileInfo file in Files )
		{
			fileList.Add(file.Name);
		}
		_allFileNames = fileList.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown()
	{
		// DEBUG SINGLE FILE UPLOAD FUNCTION
		//UpdateText ();
		if (_currentlyUploading == false) {
			StartCoroutine (UploadAllTheFiles ());
		}
	}

	private IEnumerator UploadAllTheFiles ()
	{
		// Show a message that reflects what we're currently doing.
		_currentlyUploading = true;
		debugFileContentText.text = "Currently uploading data...";

		// Loop through all the files.
		for (int _currentIndex = 0; _currentIndex < _allFileNames.Length; _currentIndex++)
		{
			// Load the file.
			string filePath = Application.persistentDataPath + "/" + _allFileNames[_currentIndex];
			string text = System.IO.File.ReadAllText(filePath);
			// Upload the file content.
			StartCoroutine(Upload(_allFileNames[_currentIndex], text, _encrypt));

			// Draw the file name on the screen for a cool visual.
			debugFileNameText.text = _allFileNames[_currentIndex];

			// Wait for the next frame to show the file name for at least one frame.
			// (No functional purpose, just cool.)
			yield return new WaitForEndOfFrame();
		}

		// Show a message that reflects what we're currently doing.
		debugFileNameText.text = "All data uploaded!";
		debugFileContentText.text = "All data uploaded!";
		_currentlyUploading = false;

		yield return null;
	}

	private void UpdateText()
	{
		// DEBUG
		// Set the text to reflect all file names.
		debugFileNameText.text = _allFileNames[_currentIndex];

		// Reset the current index if it has reached the length of _fileList
		if (_currentIndex >= _allFileNames.Length) 
		{
			_currentIndex = 0;
		}

		// Load the file.
		string filePath = Application.persistentDataPath + "/" + _allFileNames[_currentIndex];
		Debug.Log(filePath);

		//TextAsset textAsset = Resources.Load(filePath) as TextAsset;
		// Get the content as string.
		//string text = textAsset.text;
		// Get the content as a byte array.
		//byte[] byteText = textAsset.bytes;

		string text = System.IO.File.ReadAllText(filePath);
		thisString = text; 
		//ALPHA Piloting: upload the file to firebase using REST API 
		//First just save as a key value pair, key will be filename and value will be string with csv
		//filename
		thisName = _allFileNames[_currentIndex];
		//public 
		thisFileName =  Application.persistentDataPath + "/" +_allFileNames[_currentIndex];

		//call upload function 
		StartCoroutine(Upload(thisFileName, thisString, true));

		// DEBUG
		// Draw the content on the screen.
		debugFileContentText.text = text;


		// Increment the current index.
		_currentIndex++;
	}

	private IEnumerator Upload(string fileName, string fileContent, bool encrypt) {

		//new 64 instance of class
		String64 toUpload = new String64();

		// Optionally encrypt string. This converts string to Base 64.
		if (encrypt) {
			// Encrypt the string.
			toUpload.file_string = stringCipher.Encrypt(fileContent, _passPhrase);
		} else {
			//convert string to bytes 
			var plainBytes = System.Text.Encoding.UTF8.GetBytes (fileContent);
			toUpload.file_string = Convert.ToBase64String (plainBytes);
		}

		//give the reference filename 
		toUpload.filename = fileName; 

		//change the input text to JSON 
		string thisJSON = JsonUtility.ToJson(toUpload); 

		//get timestamp in URL friendly string, this will act as the key for our entry in the noSQL database
		string tStamp = System.DateTime.Now.ToString("yyyy_MM-dd-HH_mm-ss-fff"); 

		// The name of the file on the database should be the file name without extension.
		string uploadName =  Path.GetFileNameWithoutExtension(fileName);

		Debug.Log (tStamp); 

		Debug.Log (thisJSON); 
		Debug.Log (thisName); 

		//insert this json in unique ID field 
		UnityWebRequest www = UnityWebRequest.Put("https://redapp-alpha.firebaseio.com/.json?auth=Cs6Su1Up797B2LWD7QwrSWF5y7tWtwpr7rbyv0IL".Insert(36, uploadName), thisJSON);

		yield return www.Send();

		if(www.isNetworkError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("Upload complete!");
		}
	}

	//we need to gave a public class for storing the string of text, which we convert to base64 
	// intialize new istance, then add base string to string64.filestring
	[Serializable]
	private class String64
	{
		public string file_string; 
		public string filename;
	}
		
}
