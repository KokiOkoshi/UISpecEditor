using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Windows;

namespace UISpecEditor;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    string _fileName = "無題";

    [ObservableProperty]
    UISpec _uiSpec = new();

    [RelayCommand]
    void New()
    {
        this.UiSpec = new();
    }

    [RelayCommand]
    void SaveAs()
    {
        var dialog = new SaveFileDialog()
        {
            FileName = this.FileName,
            Filter = "設定ファイル|*.uispec",
        };
        if (dialog.ShowDialog() ?? false)
        {
            try
            {
                File.WriteAllText(
                    dialog.FileName,
                    JsonSerializer.Serialize(
                        this.UiSpec,
                        new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                        }));
                this.FileName = Path.GetFileName(dialog.FileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(messageBoxText: e.ToString(), caption: Assembly.GetEntryAssembly()?.FullName, button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }
    }

    [RelayCommand]
    void Open()
    {
        var dialog = new OpenFileDialog()
        {
            Filter = "uispecファイル(.uispec)|*.uispec",
        };
        if (dialog.ShowDialog() ?? false)
        {
            try
            {
                this.UiSpec = JsonSerializer.Deserialize<UISpec>(File.ReadAllText(dialog.FileName)) ?? this.UiSpec;
                this.FileName = Path.GetFileName(dialog.FileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(messageBoxText: e.ToString(), caption: Assembly.GetEntryAssembly()?.FullName, button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }
    }
}

public partial class UISpec : ObservableObject
{
    [ObservableProperty]
    Version _formatVersion = new(1, 0);

    [ObservableProperty]
    ParameterDefinition _parameterDefinition = new();

    [ObservableProperty]
    ViewSpec _viewSpec = new();
}

public partial class ParameterDefinition : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<State> _states = new();

    [ObservableProperty]
    ObservableCollection<TriggerCategory> _triggerCategories = new();
}

public partial class State : ObservableObject
{
    [ObservableProperty]
    string _name = string.Empty;

    [ObservableProperty]
    string _note = string.Empty;
}

public partial class TriggerCategory : ObservableObject
{
    [ObservableProperty]
    string _name = string.Empty;

    [ObservableProperty]
    ObservableCollection<Trigger> _triggers = new();
}

public partial class Trigger : ObservableObject
{
    [ObservableProperty]
    string _name = string.Empty;

    [ObservableProperty]
    string _note = string.Empty;
}

public partial class ViewSpec : ObservableObject
{
    [ObservableProperty]
    string _name = "無題";

    [ObservableProperty]
    string _note = string.Empty;

    [ObservableProperty]
    ObservableCollection<StateSpec> _stateSpecs = new() { new(), };

    [ObservableProperty]
    ObservableCollection<ViewSpec> _viewSpecs = new();

    [RelayCommand]
    [property:JsonIgnore]
    void AddStateSpec() => this.StateSpecs.Add(new());

    [RelayCommand]
    [property: JsonIgnore]
    void AddViewSpec() => this.ViewSpecs.Add(new());
}

public partial class StateSpec : ObservableObject
{
    [ObservableProperty]
    string _name = "無題";

    [ObservableProperty]
    string _note = string.Empty;

    [ObservableProperty]
    ObservableCollection<DesignSpec> _designSpecs = new();

    [ObservableProperty]
    ObservableCollection<BehaviorSpec> _behaviorSpecs = new();
}

public partial class DesignSpec : ObservableObject
{
    [ObservableProperty]
    string _name = string.Empty;

    [ObservableProperty]
    string _parameter = string.Empty;

    [ObservableProperty]
    string _note = string.Empty;
}

public partial class BehaviorSpec : ObservableObject
{
    [ObservableProperty]
    string _category = string.Empty;

    [ObservableProperty]
    string _trigger = string.Empty;

    [ObservableProperty]
    string _term = string.Empty;

    [ObservableProperty]
    string _action = string.Empty;
}
