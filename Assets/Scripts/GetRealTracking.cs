using UnityEngine;
using EasyLazyLibrary;
using UnityEngine.Animations;
using UnityEngine.UI;
using uOSC;

public class GetRealTracking : MonoBehaviour
{
	[SerializeField] private GameObject HMD;
	[SerializeField] private GameObject LeftHand;
	[SerializeField] private GameObject RightHand;
	
	[SerializeField] private GameObject HMD_withOffset;
	[SerializeField] private GameObject LeftHand_withOffset;
	[SerializeField] private GameObject RightHand_withOffset;
	
	[SerializeField] private GameObject V_Chest;
	[SerializeField] private GameObject V_Foot_L;
	[SerializeField] private GameObject V_Foot_R;
	[SerializeField] private GameObject V_Offset;

	[SerializeField] private WindowMinimizeManager MenuManager;

	[SerializeField] private Toggle HandToggle_R;

	[SerializeField] private ConfigManager configManager;

	private EasyOpenVRUtil eou;

	private EasyOpenVRUtil.Transform HMDTrans;
	private EasyOpenVRUtil.Transform LeftHandTrans;
	private EasyOpenVRUtil.Transform RightHandTrans;
	
	private EasyOpenVRUtil.Transform HMD_Offset;
	private EasyOpenVRUtil.Transform LeftHand_Offset;
	private EasyOpenVRUtil.Transform RightHand_Offset;
	
	private ConstraintSource FollowTarget;
	private bool isFollowRightHand = true;

	private ConfigManager.ConfigData _config = null;
	
	void Start()
	{
		eou = new EasyOpenVRUtil();
		var server = GetComponent<uOscServer>();
		server.onDataReceived.AddListener(OnDataReceived);
	}

	void Update()
	{
		if (!eou.IsReady())
		{
			eou.StartOpenVR();
		}

		if (HMD_Offset == null || LeftHand_Offset == null || RightHand_Offset == null)
		{
			HMD_Offset = eou.GetHMDTransform();
			LeftHand_Offset = eou.GetLeftControllerTransform();
			RightHand_Offset = eou.GetRightControllerTransform();
		}

		eou.AutoExitOnQuit();

		_config = configManager.GetConfig();
		
		HMDTrans = eou.GetHMDTransform();
		LeftHandTrans = eou.GetLeftControllerTransform();
		RightHandTrans = eou.GetRightControllerTransform();
		
		eou.SetGameObjectTransform(ref HMD, HMDTrans);
		eou.SetGameObjectTransform(ref LeftHand, LeftHandTrans);
		eou.SetGameObjectTransform(ref RightHand, RightHandTrans);

		eou.SetGameObjectTransformWithOffset(ref HMD_withOffset, HMDTrans, HMD_Offset);
		eou.SetGameObjectTransformWithOffset(ref LeftHand_withOffset, LeftHandTrans, LeftHand_Offset);
		eou.SetGameObjectTransformWithOffset(ref RightHand_withOffset, RightHandTrans, RightHand_Offset);
		
		ulong button;

		if (isFollowRightHand)
		{
			eou.GetControllerButtonPressed(eou.GetRightControllerIndex(), out button);
		}
		else
		{
			eou.GetControllerButtonPressed(eou.GetLeftControllerIndex(), out button);
		}
		if (button != 0)
		{
			V_Chest.SetActive(false);
			V_Foot_L.SetActive(false);
			V_Foot_R.SetActive(false);
			V_Offset.SetActive(false);
		}
		
		if (_config.isMenuLeftHand && LeftHandTrans != null && LeftHandTrans.velocity.y < -3.5f || _config.isMenuRightHand && RightHandTrans != null && RightHandTrans.velocity.y < -3.5f)
		{
			if (!MenuManager.IsActive() && (!_config.isMenuLock || _config.MenuLockButton == 0 || _config.MenuLockButton == button))
			{
				MenuManager.Open();
			}
		}else if (_config.isMenuLeftHand && LeftHandTrans != null && LeftHandTrans.velocity.y > 3.5f || _config.isMenuRightHand && RightHandTrans != null && RightHandTrans.velocity.y > 3.5f)
		{
			if (MenuManager.IsActive() && (!_config.isMenuLock || _config.MenuLockButton == 0 || _config.MenuLockButton == button))
			{
				MenuManager.Close();
			}			
		}




	}
	void OnDataReceived(Message message)
	{
		var msg = message.address;

		switch (msg)
		{
			case "/vTPM/Chest":
				MoveChest();
				break;
			case "/vTPM/Foot_L":
				MoveFootL();
				break;
			case "/vTPM/Foot_R":
				MoveFootR();
				break;
			case "/vTPM/Offset":
				MoveOffset();
				break;
			default:
				Debug.Log(msg);
				break;
		}
	}

	public ulong GetControllerButton()
	{
		ulong button;

		eou.GetControllerButtonPressed(eou.GetRightControllerIndex(), out button);
		if (button == 0)
		{
			eou.GetControllerButtonPressed(eou.GetLeftControllerIndex(), out button);	
		}

		return button;
	}

	public void MoveChest()
	{
		V_Chest.SetActive(true);
	}

	public void MoveFootL()
	{
		V_Foot_L.SetActive(true);
	}

	public void MoveFootR()
	{
		V_Foot_R.SetActive(true);
	}

	public void MoveOffset()
	{
		HMD_Offset = eou.GetHMDTransform();
		LeftHand_Offset = eou.GetLeftControllerTransform();
		RightHand_Offset = eou.GetRightControllerTransform();
		V_Offset.SetActive(true);
	}

	public void ChangeFollowHand(bool _)
	{
		FollowTarget.weight = 1.0f;
		
		if (HandToggle_R.isOn)
		{
			FollowTarget.sourceTransform = RightHand.transform;
			isFollowRightHand = true;
		}
		else
		{
			FollowTarget.sourceTransform = LeftHand.transform;
			isFollowRightHand = false;
		}

		V_Chest.GetComponent<ParentConstraint>().SetSource(0, FollowTarget);
		V_Foot_L.GetComponent<ParentConstraint>().SetSource(0, FollowTarget);
		V_Foot_R.GetComponent<ParentConstraint>().SetSource(0, FollowTarget);
	}

}
