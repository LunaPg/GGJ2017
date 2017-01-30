using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	
	void Start () {
		instance = this;
		GameObject.Find("Parley/Character Creation").GetComponent<Dialog>().TriggerDialog();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	public void RandomRoll () {
		int roll = Random.Range(0, 100);
		GetComponent<ParleyInvironmentInfoDictionary>().SetEnviromentInfo("random", roll);
	}

	public void Push () {
		//GetComponent<ParleyInvironmentInfoDictionary>().SetEnviromentInfo("checkpointdialog", Parley.GetInstance().GetCurrentDialog());
		//int iD = Parley.GetInstance().GetCurrentDialog().GetConversations()[Parley.GetInstance().GetCurrentDialog().GetConversationIndex()].options[0].destinationId;
		//GetComponent<ParleyInvironmentInfoDictionary>().SetEnviromentInfo("checkpointconversation", iD);

		int destinationID = Parley.GetInstance().GetCurrentDialog().GetConversations()[Parley.GetInstance().GetCurrentDialog().GetConversationIndex()].options[0].destinationId;
		string destinationName = Parley.GetInstance().GetCurrentDialog().GetConversations()[Parley.GetInstance().GetCurrentDialog().GetConversationIndex()].options[0].destinationDialogName;
		GetComponent<ParleyInvironmentInfoDictionary>().SetEnviromentInfo("checkpointdialog", destinationName);
		GetComponent<ParleyInvironmentInfoDictionary>().SetEnviromentInfo("checkpointconversation", destinationID);

		int returnID = Parley.GetInstance().GetCurrentDialog().GetConversations()[Parley.GetInstance().GetCurrentDialog().GetConversationIndex()].returnId;
		string returnName = Parley.GetInstance().GetCurrentDialog().GetConversations()[Parley.GetInstance().GetCurrentDialog().GetConversationIndex()].returnDialogName;
		FindObjectOfType<DialogGuiAbstract>().GotoDialogue(returnName, returnID);
		//Parley.GetInstance().GetCurrentDialog().SetConversationIndex(Parley.GetInstance().GetCurrentDialog().GetConversations()[Parley.GetInstance().GetCurrentDialog().GetConversationIndex()].returnId);
		Debug.Log("Push");
	}

	public void Pop () {
		//Dialog dialog = GetComponent<ParleyInvironmentInfoDictionary>().GetEnviromentInfo("checkpointdialog") as Dialog;
		//Parley.GetInstance().GetCurrentDialog().TriggerDialogEnd();
		//dialog.TriggerDialog((int)GetComponent<ParleyInvironmentInfoDictionary>().GetEnviromentInfo("checkpointconversation"));

		FindObjectOfType<DialogGuiAbstract>().GotoDialogue((string)GetComponent<ParleyInvironmentInfoDictionary>().GetEnviromentInfo("checkpointdialog"), (int)GetComponent<ParleyInvironmentInfoDictionary>().GetEnviromentInfo("checkpointconversation"));

		Debug.Log("Pop");
	}
}