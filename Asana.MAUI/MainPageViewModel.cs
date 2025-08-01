using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.MAUI
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly ToDoServiceProxy _service;
        private readonly UserServiceProxy _userService;

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Project> FilteredProjects { get; set; }
        public ObservableCollection<User> Users { get; set; } = new();

        public void AddProject(string name, string description)
        {
            if (SelectedUser != null)
            {
                var newProject = new Project
                {
                    Id = Projects.Any() ? Projects.Max(p => p.Id) + 1 : 1,
                    Name = name,
                    Description = description,
                    CompletePercent = 0,
                    ToDos = new List<ToDo>()
                };
                newProject.InitializeListeners();
                SelectedUser.Projects.Add(newProject);
                Projects.Add(newProject);
                ApplyFilter();
            }
        }

        private void LoadProjectsForSelectedUser()
        {
            if (SelectedUser != null)
            {
                foreach (var project in SelectedUser.Projects)
                {
                    project.InitializeListeners();
                    project.UpdateCompletion(); // recalculate initial %
                }

                Projects = new ObservableCollection<Project>(SelectedUser.Projects);
                ApplyFilter();
                OnPropertyChanged(nameof(Projects));
            }
        }


        public void AddToDoToProject(int projectId, string todoName, string assignedUserId)
        {
            var project = Projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                var newToDo = new ToDo
                {
                    Id = project.ToDos.Any() ? project.ToDos.Max(t => t.Id) + 1 : 1,
                    Name = todoName,
                    Description = string.Empty,
                    IsCompleted = false,
                    ProjectId = projectId,
                    AssignedUserId = assignedUserId
                };

                project.ToDos.Add(newToDo);
                UpdateProjectCompletion(project);
                ApplyFilter();
            }
        }


        public void DeleteToDo(int projectId, int todoId)
        {
            var project = Projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                var todo = project.ToDos.FirstOrDefault(t => t.Id == todoId);
                if (todo != null)
                {
                    project.ToDos.Remove(todo);
                    UpdateProjectCompletion(project);
                    ApplyFilter();
                }
            }
        }

        public void DeleteProject(int projectId)
        {
            var project = Projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                SelectedUser?.Projects.Remove(project);
                Projects.Remove(project);
                ApplyFilter();
            }
        }

        public void ToggleToDoCompleted(int projectId, int todoId)
        {
            var project = Projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                var todo = project.ToDos.FirstOrDefault(t => t.Id == todoId);
                if (todo != null)
                {
                    todo.IsCompleted = !todo.IsCompleted;
                    UpdateProjectCompletion(project!);
                    ApplyFilter();
                }
            }
        }

        private void UpdateProjectCompletion(Project project)
        {
            if (project.ToDos.Count == 0)
            {
                project.CompletePercent = 0;
            }
            else
            {
                int completedCount = project.ToDos.Count(t => t.IsCompleted);
                project.CompletePercent = (double)completedCount / project.ToDos.Count * 100;
            }
            OnPropertyChanged(nameof(Projects));
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                    LoadProjectsForSelectedUser();
                }
            }
        }



        private Project _selectedProject;
        public Project SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }


        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    ApplyFilter();
                }
            }
        }

        public MainPageViewModel()
        {
            _service = new ToDoServiceProxy();
            _userService = new UserServiceProxy();

            Users = new ObservableCollection<User>(_userService.GetAllUsers());

            // Select first user by default
            SelectedUser = Users.FirstOrDefault();
        }

        public void ApplyFilter()
        {
            IEnumerable<Project> filtered;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = Projects;
            }
            else
            {
                string query = SearchText.ToLower();

                filtered = Projects
                    .Where(p =>
                        (p.Name?.ToLower().Contains(query) == true)
                        || p.ToDos.Any(t => t.Name?.ToLower().Contains(query) == true));
            }

            var filteredList = filtered.ToList();

            switch (SortIndex)
            {
                case 0: // Recently Added
                    filteredList = filteredList.OrderByDescending(p => p.Id).ToList();
                    break;
                case 1: // Completion %
                    filteredList = filteredList.OrderByDescending(p => p.CompletePercent).ToList();
                    break;
                case 2: // Name A-Z
                    filteredList = filteredList.OrderBy(p => p.Name).ToList();
                    break;
            }

            FilteredProjects = new ObservableCollection<Project>(filteredList);
            OnPropertyChanged(nameof(FilteredProjects));
        }

        public void AddUser(string username)
        {
            if (!string.IsNullOrWhiteSpace(username) && !Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                var newUser = new User
                {
                    Id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1,
                    Username = username,
                    Projects = new List<Project>()
                };
                Users.Add(newUser);
                SelectedUser = newUser;
            }
        }

        private int _sortIndex;
        public int SortIndex
        {
            get => _sortIndex;
            set
            {
                if (_sortIndex != value)
                {
                    _sortIndex = value;
                    OnPropertyChanged(nameof(SortIndex));
                    ApplyFilter();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
