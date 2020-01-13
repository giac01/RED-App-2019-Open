using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Video;

using System.IO;
using SimpleJSON; 
using System; 

//Stuff for Alpha file uploader (3rd party, not compliant for actual participant data)

using System.Linq;
[Serializable]


public class EvidJSON : MonoBehaviour
{
	public string Trial;
	public string Type;
	public string Dirs;
	public string Resp;
	public string Accu;
	public string RT;
	public string initRT;
}

//public class EvidVecJSON : MonoBehaviour
//{
//	public List<string> Xvect;
//	public List<string> Yvect;
//	public List<string> CohVect;
//
//}