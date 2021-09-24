﻿#nullable enable

using DevTools.Common;
using DevTools.Core;
using DevTools.Core.Settings;
using DevTools.Core.Theme;
using DevTools.Impl.Views.Settings;
using DevTools.Providers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Reflection;
using System.Threading.Tasks;

namespace DevTools.Impl.ViewModels.Settings
{
    [Export(typeof(SettingsToolViewModel))]
    public sealed class SettingsToolViewModel : ObservableRecipient, IToolViewModel
    {
        private readonly IWindowManager _windowManager;
        private readonly ISettingsProvider _settingsProvider;

        public Type View { get; } = typeof(SettingsToolPage);

        internal SettingsStrings Strings => LanguageManager.Instance.Settings;

        internal List<LanguageDefinition> AvailableLanguages => LanguageManager.Instance.AvailableLanguages;

        internal string Language
        {
            get => _settingsProvider.GetSetting(PredefinedSettings.Language);
            set => _settingsProvider.SetSetting(PredefinedSettings.Language, value);
        }

        internal AppTheme Theme
        {
            get => _settingsProvider.GetSetting(PredefinedSettings.Theme);
            set => _settingsProvider.SetSetting(PredefinedSettings.Theme, value);
        }

        internal bool SmartDetection
        {
            get => _settingsProvider.GetSetting(PredefinedSettings.SmartDetection);
            set => _settingsProvider.SetSetting(PredefinedSettings.SmartDetection, value);
        }

        internal string TextEditorFont
        {
            get => _settingsProvider.GetSetting(PredefinedSettings.TextEditorFont);
            set => _settingsProvider.SetSetting(PredefinedSettings.TextEditorFont, value);
        }

        internal bool TextEditorTextWrapping
        {
            get => _settingsProvider.GetSetting(PredefinedSettings.TextEditorTextWrapping);
            set => _settingsProvider.SetSetting(PredefinedSettings.TextEditorTextWrapping, value);
        }

        internal bool TextEditorLineNumbers
        {
            get => _settingsProvider.GetSetting(PredefinedSettings.TextEditorLineNumbers);
            set => _settingsProvider.SetSetting(PredefinedSettings.TextEditorLineNumbers, value);
        }

        internal bool TextEditorHighlightCurrentLine
        {
            get => _settingsProvider.GetSetting(PredefinedSettings.TextEditorHighlightCurrentLine);
            set => _settingsProvider.SetSetting(PredefinedSettings.TextEditorHighlightCurrentLine, value);
        }

        /// <summary>
        /// Gets the version of the application.
        /// </summary>
        internal string Version => Strings.GetFormattedVersion(typeof(SettingsToolViewModel).GetTypeInfo().Assembly.GetName().Version.ToString());

        [ImportingConstructor]
        public SettingsToolViewModel(
            IWindowManager windowManager,
            ISettingsProvider settingsProvider)
        {
            _windowManager = windowManager;
            _settingsProvider = settingsProvider;

            PrivacyPolicyCommand = new AsyncRelayCommand(ExecutePrivacyPolicyCommandAsync);
            ThirdPartyNoticesCommand = new AsyncRelayCommand(ExecuteThirdPartyNoticesCommandAsync);
            LicenseCommand = new AsyncRelayCommand(ExecuteLicenseCommandAsync);
        }

        #region PrivacyPolicyCommand

        internal IAsyncRelayCommand PrivacyPolicyCommand { get; }

        private async Task ExecutePrivacyPolicyCommandAsync()
        {
            await _windowManager.ShowContentDialogAsync(
                new MarkdownContentDialog(
                    await AssetsHelper.GetPrivacyStatementAsync()),
                Strings.Close,
                title: Strings.PrivacyPolicy);
        }

        #endregion

        #region ThirdPartyNoticesCommand

        internal IAsyncRelayCommand ThirdPartyNoticesCommand { get; }

        private async Task ExecuteThirdPartyNoticesCommandAsync()
        {
            await _windowManager.ShowContentDialogAsync(
                new MarkdownContentDialog(
                    await AssetsHelper.GetThirdPartyNoticesAsync()),
                Strings.Close,
                title: Strings.ThirdPartyNotices);
        }

        #endregion

        #region LicenseCommand

        internal IAsyncRelayCommand LicenseCommand { get; }

        private async Task ExecuteLicenseCommandAsync()
        {
            await _windowManager.ShowContentDialogAsync(
                new MarkdownContentDialog(
                    await AssetsHelper.GetLicenseAsync()),
                Strings.Close,
                title: Strings.License);
        }

        #endregion
    }
}