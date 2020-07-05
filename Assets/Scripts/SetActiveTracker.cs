using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uOSC;

public class SetActiveTracker : MonoBehaviour
{
	[SerializeField] private GameObject T_Chest;
	[SerializeField] private GameObject T_Foot_L;
	[SerializeField] private GameObject T_Foot_R;
	
	// Start is called before the first frame update
	void Start()
	{
		var server = GetComponent<uOscServer>();
		server.onDataReceived.AddListener(OnDataReceived);
	}

	private void OnDataReceived(Message message)
	{
		var msg = message.address;

		if (msg == "/vTPM/Off")
		{
			TrackerDisable();
		}
		else if (msg == "/vTPM/On")
		{
			TrackerEnable();
		}
		else
		{
			Debug.Log(msg);
		}
	}

	public void TrackerEnable()
	{
		T_Chest.GetComponent<VMTTransmitter>().isEnable = 1;
		T_Foot_L.GetComponent<VMTTransmitter>().isEnable = 1;
		T_Foot_R.GetComponent<VMTTransmitter>().isEnable = 1;
	}

	public void TrackerDisable()
	{
		T_Chest.GetComponent<VMTTransmitter>().isEnable = 0;
		T_Foot_L.GetComponent<VMTTransmitter>().isEnable = 0;
		T_Foot_R.GetComponent<VMTTransmitter>().isEnable = 0;
	}

	public void SetTrackerState(bool isEnable)
	{
		if (isEnable)
		{
			TrackerEnable();
		} 
		else
		{
			TrackerDisable();
		}
	}
}
