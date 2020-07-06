using System;
using System.IO;
using System.Text;
using UnityEngine;

public class QuickSaveManager : MonoBehaviour
{
	private const int ConfigVersion = 1;
	private const string SavePath = "config";

	[SerializeField] private GameObject V_Chest;
	[SerializeField] private GameObject V_Foot_L;
	[SerializeField] private GameObject V_Foot_R;

	[SerializeField] private GameObject Modal_QS_Slot1;
	[SerializeField] private GameObject Modal_QS_Slot2;
	[SerializeField] private GameObject Modal_QS_Slot3;

	[SerializeField] private GameObject Modal_QL_Slot1;
	[SerializeField] private GameObject Modal_QL_Slot2;
	[SerializeField] private GameObject Modal_QL_Slot3;
	
	[SerializeField] private ConfigManager configManager;
	
	private TrackerTransform Slot1;
	private TrackerTransform Slot2;
	private TrackerTransform Slot3;
	
	[Serializable]
	private class TrackerTransform
	{
		public int ConfVersion;
		public Vector3 Chest_Position;
		public Quaternion Chest_Rotation;
		public Vector3 Foot_L_Position;
		public Quaternion Foot_L_Rotation;
		public Vector3 Foot_R_Position;
		public Quaternion Foot_R_Rotation;
	}

	public void QuickSaveWithDialog(int slotNum)
	{
		if (!IsSaveDataExists(slotNum) || !configManager.GetConfig().isDialogSaveLoad)
		{
			QuickSave(slotNum);
			return;
		}

		switch (slotNum)
		{
			case 1:
				Modal_QS_Slot1.SetActive(true);
				break;
			case 2:
				Modal_QS_Slot2.SetActive(true);
				break;
			case 3:
				Modal_QS_Slot3.SetActive(true);
				break;
			default:
				Debug.Log("Error: QuickSave: not Exist Slot: " + slotNum);
				break;
		}
	}

	public void QuickLoadWithDialog(int slotNum)
	{
		if (!IsSaveDataExists(slotNum))
		{
			return;
		}

		if (!configManager.GetConfig().isDialogSaveLoad)
		{
			QuickLoad(slotNum);
			return;
		}

		switch (slotNum)
		{
			case 1:
				Modal_QL_Slot1.SetActive(true);
				break;
			case 2:
				Modal_QL_Slot2.SetActive(true);
				break;
			case 3:
				Modal_QL_Slot3.SetActive(true);
				break;
			default:
				Debug.Log("Error: QuickSave: not Exist Slot: " + slotNum);
				break;
		}
	}
	
	public void QuickSave(int slotNum)
	{
		switch (slotNum)
		{
			case 1:
				SaveTransform(Slot1, slotNum);
				Modal_QS_Slot1.SetActive(false);
				break;
			case 2:
				SaveTransform(Slot2, slotNum);
				Modal_QS_Slot2.SetActive(false);
				break;
			case 3:
				SaveTransform(Slot3, slotNum);
				Modal_QS_Slot3.SetActive(false);
				break;
			default:
				Debug.Log("Error: QuickSave: not Exist Slot: " + slotNum);
				break;
		}
	}

	public void QuickLoad(int slotNum)
	{
		switch (slotNum)
		{
			case 1:
				LoadTransform(slotNum);
				Modal_QL_Slot1.SetActive(false);
				break;
			case 2:
				LoadTransform(slotNum);
				Modal_QL_Slot2.SetActive(false);
				break;
			case 3:
				LoadTransform(slotNum);
				Modal_QL_Slot3.SetActive(false);
				break;
			default:
				Debug.Log("Error: QuickLoad: not Exist Slot: " + slotNum);
				break;
		}
	}

	private void SaveTransform(TrackerTransform slot, int slotNum)
	{
		if (slot == null)
		{
			slot = new TrackerTransform();
		}

		slot.ConfVersion = ConfigVersion;
		slot.Chest_Position = V_Chest.transform.position;
		slot.Chest_Rotation = V_Chest.transform.rotation;
		slot.Foot_L_Position = V_Foot_L.transform.position;
		slot.Foot_L_Rotation = V_Foot_L.transform.rotation;
		slot.Foot_R_Position = V_Foot_R.transform.position;
		slot.Foot_R_Rotation = V_Foot_R.transform.rotation;

		var json = JsonUtility.ToJson(slot, true);

		if (!Directory.Exists(SavePath))
		{
			Directory.CreateDirectory(SavePath);
		}
		File.WriteAllText(SavePath + "\\Slot" + slotNum + ".json", json, new UTF8Encoding(false));
	}

	private void LoadTransform(int slotNum)
	{
		var file = SavePath + "\\Slot" + slotNum + ".json";
		
		if (!IsSaveDataExists(slotNum))
		{
			return;
		}

		try
		{
			var jsonStr = File.ReadAllText(file, new UTF8Encoding(false));
			var slot = JsonUtility.FromJson<TrackerTransform>(jsonStr);

			V_Chest.transform.SetPositionAndRotation(slot.Chest_Position, slot.Chest_Rotation);
			V_Foot_L.transform.SetPositionAndRotation(slot.Foot_L_Position, slot.Foot_L_Rotation);
			V_Foot_R.transform.SetPositionAndRotation(slot.Foot_R_Position, slot.Foot_R_Rotation);
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
	}

	private static bool IsSaveDataExists(int slotNum)
	{
		var file = SavePath + "\\Slot" + slotNum + ".json";
		
		if (!Directory.Exists(SavePath))
		{
			Directory.CreateDirectory(SavePath);
		}
		
		if (File.Exists(file))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
