using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asana.Library.Models;
using Asana.Library.Services;

namespace Asana.MAUI
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Project> Projects { get; set; }

        public MainPageViewModel()
        {
            var service = new ToDoServiceProxy();
            Projects = new ObservableCollection<Project>(service.GetProjects());
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
