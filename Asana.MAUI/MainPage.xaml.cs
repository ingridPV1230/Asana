using Asana.Library.Models;
using System.Text.Json;

namespace Asana.MAUI
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        private async void OnAddProjectClicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("New Project", "Enter project name:");

            if (string.IsNullOrWhiteSpace(name)) return;

            string description = await DisplayPromptAsync("New Project", "Enter project description:");

            if (BindingContext is MainPageViewModel vm)
            {
                vm.AddProject(name.Trim(), description?.Trim() ?? "");
            }
        }


        private void OnAddToDoClicked(object sender, EventArgs e)
        {
            if (sender is Button button &&
                button.BindingContext is Project project)
            {
                var layout = (button.Parent as HorizontalStackLayout);
                if (layout != null)
                {
                    var entry = layout.Children
                        .OfType<Entry>()
                        .FirstOrDefault();

                    if (entry != null &&
                        !string.IsNullOrWhiteSpace(entry.Text) &&
                        BindingContext is MainPageViewModel vm &&
                        vm.SelectedUser != null)
                    {
                        vm.AddToDoToProject(project.Id, entry.Text, vm.SelectedUser.Id.ToString());
                        entry.Text = string.Empty;
                    }
                }
            }
        }

        private void OnDeleteProjectClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Project project)
            {
                if (BindingContext is MainPageViewModel vm)
                {
                    vm.DeleteProject(project.Id);
                }
            }
        }

        private void OnDeleteToDoClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ToDo todo)
            {
                if (BindingContext is MainPageViewModel vm)
                {
                    var project = vm.Projects.FirstOrDefault(p => p.ToDos.Any(t => t.Id == todo.Id));
                    if (project != null)
                    {
                        vm.DeleteToDo(project.Id, todo.Id);
                    }
                }
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (BindingContext is MainPageViewModel vm)
            {
                vm.SearchText = e.NewTextValue;
            }
        }

        private void OnSortChanged(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel vm && sender is Picker picker)
            {
                vm.SortIndex = picker.SelectedIndex;
            }
        }

        private async void OnExportClicked(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel vm)
            {
                var json = JsonSerializer.Serialize(vm.Projects);
                var filePath = Path.Combine(FileSystem.AppDataDirectory, "projects_export.json");
                File.WriteAllText(filePath, json);
                await DisplayAlert("Export Successful", $"Projects exported to {filePath}", "OK");
            }
        }

        private async void OnImportClicked(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel vm)
            {
                try
                {
                    var filePath = Path.Combine(FileSystem.AppDataDirectory, "projects_export.json");
                    if (!File.Exists(filePath))
                    {
                        await DisplayAlert("Error", "Export file not found.", "OK");
                        return;
                    }

                    var json = File.ReadAllText(filePath);
                    var importedProjects = JsonSerializer.Deserialize<List<Project>>(json);

                    if (importedProjects != null && vm.SelectedUser != null)
                    {
                        foreach (var project in importedProjects)
                        {
                            foreach (var todo in project.ToDos)
                            {
                                todo.AssignedUserId = vm.SelectedUser.Username;
                            }
                            project.InitializeListeners();
                            project.UpdateCompletion();

                            vm.SelectedUser.Projects.Add(project);
                            vm.Projects.Add(project);
                        }

                        //vm.Projects.Clear();
                        foreach (var p in importedProjects)
                        {
                            vm.Projects.Add(p);
                        }

                        vm.ApplyFilter();
                    }

                    await DisplayAlert("Import Successful", "Projects imported successfully.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to import projects: {ex.Message}", "OK");
                }
            }
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Add User", "Enter username:");

            if (!string.IsNullOrWhiteSpace(result) && BindingContext is MainPageViewModel vm)
            {
                vm.AddUser(result.Trim());
            }
        }
    }
}
