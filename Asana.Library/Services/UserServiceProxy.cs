using System.Collections.Generic;
using Asana.Library.Models;

namespace Asana.Library.Services
{
    public class UserServiceProxy
    {
        private List<User> _users;

        public UserServiceProxy()
        {
            _users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "Ingrid",
                    Projects = new List<Project>
                    {
                        new Project
                        {
                            Id = 1,
                            Name = "Meal Prep",
                            Description = "Prepare the meal prep for this week",
                            CompletePercent = 0,
                            ToDos = new List<ToDo>
                            {
                                new ToDo { Id = 1, Name = "Grocery list", Description = "Write down all items needed", IsCompleted = false, ProjectId = 1, AssignedUserId = "Ingrid"},
                                new ToDo { Id = 2, Name = "Cook", Description = "Cook all proteins and prep veggies", IsCompleted = false, ProjectId = 1, AssignedUserId = "Ingrid" },
                                new ToDo { Id = 3, Name = "Package", Description = "Pack up all 5 meals", IsCompleted = false, ProjectId = 1, AssignedUserId = "Ingrid" }
                            }
                        }
                    }
                },
                new User
                {
                    Id = 2,
                    Username = "Alex",
                    Projects = new List<Project>
                    {
                        new Project
                        {
                            Id = 2,
                            Name = "Study Plan for Midterms",
                            Description = "Follow study plan for midterms",
                            CompletePercent = 0,
                            ToDos = new List<ToDo>
                            {
                                new ToDo { Id = 4, Name = "Write down dates", Description = "Gather dates of exams and assignments", IsCompleted = false, ProjectId = 2, AssignedUserId = "Alex" },
                                new ToDo { Id = 5, Name = "Create study schedule", Description = "Create a schedule for studying", IsCompleted = false, ProjectId = 2, AssignedUserId = "Alex" },
                                new ToDo { Id = 6, Name = "Study", Description = "Study for each exam", IsCompleted = false, ProjectId = 2, AssignedUserId = "Alex" }
                            }
                        }
                    }
                }
            };
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public User? GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
