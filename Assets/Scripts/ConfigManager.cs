using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class ConfigManager : MonoBehaviour
{
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GetRealTracking RealTracking;
    [SerializeField] private Toggle DialogSaveLoad;
    [SerializeField] private GameObject ModalInfoDialog;
    [SerializeField] private Text ModalInfoDialogText;
    
    const int ConfigVersion = 1;
    private const string SavePath = "config";

    private ConfigData _config = null;

    [Serializable]
    public class ConfigData
    {
        public int ConfVersion;
        public bool isMenuLeftHand = true;
        public bool isMenuRightHand = false;
        public bool isMenuLock = false;
        public ulong MenuLockButton = 0;
        public bool isDialogSaveLoad = true;
    }

    void ShowModalInfoDialog(string Message)
    {
        ModalInfoDialogText.text = Message;
        ModalInfoDialog.SetActive(true);
    }

    public ConfigData GetConfig()
    {
        return _config;
    }

    public void OpenSettingsPanel()
    {
        SettingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        SettingsPanel.SetActive(false);
    }

    public void SetDialogSaveLoad(bool State)
    {
        _config.isDialogSaveLoad = DialogSaveLoad.isOn;
        SaveConfig();
    }

    public void SetMenuLockButton()
    {
        _config.MenuLockButton = RealTracking.GetControllerButton();
        SaveConfig();
        
        if (_config.MenuLockButton != 0)
        {
            ShowModalInfoDialog("Set LockButton : " + _config.MenuLockButton + " ");
        }
        else
        {
            ShowModalInfoDialog("LockButton Disable");
        }
    }

    public void SetMenuLock(bool State)
    {
        _config.isMenuLock = State;
        SaveConfig();
        if (State)
        {
            ShowModalInfoDialog("Set Menu Lock: Enable");
        }
        else
        {
            ShowModalInfoDialog("Set Menu Lock: Disable");
        }
    }

    public void SetMenuHand(int Hand)
    {
        switch (Hand)
        {
            case 0: 
                _config.isMenuLeftHand = true;
                _config.isMenuRightHand = false;
                ShowModalInfoDialog("Set Menu Open Hand: Left");
                break;
            case 1:
                _config.isMenuLeftHand = true;
                _config.isMenuRightHand = true;
                ShowModalInfoDialog("Set Menu Open Hand: Both");
                break;
            case 2:
                _config.isMenuLeftHand = false;
                _config.isMenuRightHand = true;
                ShowModalInfoDialog("Set Menu Open Hand: Right");
                break;
            default:
                _config.isMenuLeftHand = true;
                _config.isMenuRightHand = false;
                ShowModalInfoDialog("Set Menu Open Hand: Left");
                break;
        }
        SaveConfig();
    }

    private void SaveConfig()
    {
        if (_config == null)
        {
            _config = new ConfigData();
        }

        _config.ConfVersion = ConfigVersion;

        var json = JsonUtility.ToJson(_config);

        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        File.WriteAllText(SavePath + "\\config.json", json, new UTF8Encoding(false));
    }

    private void LoadConfig()
    {
        const string file = SavePath + "\\config.json";
        
        _config = null;

        if (!IsConfigDataExists())
        {
            _config = new ConfigData();
            return;
        }

        try
        {
            string jsonStr = File.ReadAllText(file, new UTF8Encoding(false));
            _config = JsonUtility.FromJson<ConfigData>(jsonStr);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            _config = new ConfigData();
        }
    }

    bool IsConfigDataExists()
    {
        const string file = SavePath + "\\config.json";
        
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        
        return File.Exists(file);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        LoadConfig();
        DialogSaveLoad.isOn = _config.isDialogSaveLoad; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
