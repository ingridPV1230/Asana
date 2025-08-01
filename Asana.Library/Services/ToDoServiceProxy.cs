using Asana.Library.Models;
using Asana.Library.Services;

public class ToDoServiceProxy
{
    private UserServiceProxy _userService;

    public ToDoServiceProxy()
    {
        _userService = new UserServiceProxy();
    }

    public List<Project> GetProjectsForUser(string username)
    {
        var user = _userService.GetUserByUsername(username);
        return user?.Projects ?? new List<Project>();
    }

    public List<ToDo> GetAllToDosForUser(string username)
    {
        var projects = GetProjectsForUser(username);
        return projects.SelectMany(p => p.ToDos).ToList();
    }
}
