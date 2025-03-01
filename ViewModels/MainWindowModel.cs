﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RawInput;
using System.Collections.ObjectModel;

namespace ScreTran;

public partial class MainWindowModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly IWindowService _windowService;
    private readonly IExecutionService _executionService;
    private readonly IInputService _inputSevice;

    /// <summary>
    /// App parameters.
    /// </summary>
    [ObservableProperty]
    private IParametersService _parameters;

    /// <summary>
    /// Is key sets in current moment.
    /// </summary>
    [ObservableProperty]
    private bool _isKeySetting;

    /// <summary>
    /// App settings.
    /// </summary>
    [ObservableProperty]
    private SettingsModel _settings;

    /// <summary>
    /// List of translators.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Enumerations.Translator> _translators;

    /// <summary>
    /// List of models.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<Enumerations.Model> _models;

    /// <summary>
    /// Start execution.
    /// </summary>
    [RelayCommand]
    private void Start()
    {
        _windowService.SetWindowClickThru("TranslationWindow");
        _windowService.SetWindowClickThru("SelectionWindow");
        _executionService.Start();
        Parameters.IsStarted = true;
    }

    /// <summary>
    /// Stop execution.
    /// </summary>
    [RelayCommand]
    private void Stop()
    {
        _windowService.SetWindowClickable("TranslationWindow");
        _windowService.SetWindowClickable("SelectionWindow");
        _executionService.Stop();
        Parameters.IsStarted = false;
    }

    /// <summary>
    /// App close event.
    /// </summary>
    [RelayCommand]
    private void Close()
    {
        _settingsService.Save(Settings);
        _windowService.CloseAll();
    }

    /// <summary>
    /// Clear hotkey.
    /// </summary>
    [RelayCommand]
    private void ClearKey()
    {
        Settings.Key = new Key();
    }

    /// <summary>
    /// Set hotkey.
    /// </summary>
    [RelayCommand]
    private void SetKey()
    {
        IsKeySetting = true;
    }

    /// <summary>
    /// Resets windows position.
    /// </summary>
    [RelayCommand]
    private void ResetWindowsPosition()
    {
        Settings.ResetWindowPositions();
    }

    /// <summary>
    /// Reset settings to default.
    /// </summary>
    [RelayCommand]
    private void ResetSettingsToDefault()
    {
        Settings.ResetToDefault();
    }

    public MainWindowModel(ISettingsService settingsService, IParametersService parametersService, IWindowService windowService, IExecutionService executionService, IInputService inputService)
    {
        _settingsService = settingsService;
        Parameters = parametersService;

        Settings = _settingsService.Settings;

        _translators =
        [
            Enumerations.Translator.Google,
            Enumerations.Translator.Yandex,
            Enumerations.Translator.Bing,
        ];

        _models =
        [
            Enumerations.Model.English,
            Enumerations.Model.Korean,
            Enumerations.Model.Chinese,
            Enumerations.Model.Japanese,
        ];

        IsKeySetting = false;

        _executionService = executionService;

        _windowService = windowService;
        _windowService.Show("SelectionWindow");
        _windowService.Show("TranslationWindow");
        _windowService.ExcludeFromCapture("SelectionWindow");
        _windowService.ExcludeFromCapture("TranslationWindow");

        _inputSevice = inputService;
        _inputSevice.KeyDown += InputSevice_KeyDown;
    }

    /// <summary>
    /// Key down event processing.
    /// </summary>
    private void InputSevice_KeyDown(HookEventArgs e)
    {
        if (IsKeySetting)
        {
            if (e.Key.Code != KeyCodes.Esc)
                Settings.Key = e.Key;

            IsKeySetting = false;
            return;
        }

        if (Equals(e.Key, Settings.Key))
        {
            if (Parameters.IsStarted)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }
    }
}
