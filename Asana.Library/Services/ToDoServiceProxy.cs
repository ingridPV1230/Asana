using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asana.Library.Models;

namespace Asana.Library.Services
{
    public class ToDoServiceProxy
    {
        private List<Project> _projects;

        public ToDoServiceProxy()
        {
            _projects = new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Name = "Project 1",
                    Description = "Description for Project 1",
                    ToDos = new List<ToDo>
                    {
                        new ToDo { Id = 1, Name = "Task 1", Description = "Description for Task 1", IsCompleted = false, ProjectId = 1 },
                        new ToDo { Id = 2, Name = "Task 2", Description = "Description for Task 2", IsCompleted = true, ProjectId = 1 }
                    },
                    CompletePercent = 50
                },
                new Project
                {
                    Id = 2,
                    Name = "Project 2",
                    Description = "Description for Project 2",
                    ToDos = new List<ToDo>
                    {
                        new ToDo { Id = 3, Name = "Task 3", Description = "Description for Task 3", IsCompleted = false, ProjectId = 2}
                    },
                    CompletePercent = 0
                }
            };
        }
        public List<Project> GetProjects()
        {
            return _projects;
        }

        public List<ToDo> GetAllToDos()
        {
            return _projects.SelectMany(p => p.ToDos).ToList();
        }

    }
}
