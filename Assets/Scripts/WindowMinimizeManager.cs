using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMinimizeManager : MonoBehaviour
{
	[SerializeField] private EasyOpenVROverlayForUnity OverlaySystem;
	[SerializeField] private PositionManagerScript PositionManager;

	private bool _isActive = false;
	
	public void Open()
	{
		OverlaySystem.show = true;
		PositionManager.setPosition();
		_isActive = true;
	}

	public void Close()
	{
		OverlaySystem.show = false;
		_isActive = false;
	}

	public bool IsActive()
	{
		return _isActive;
	}
}
