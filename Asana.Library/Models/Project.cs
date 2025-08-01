using System.Collections.Generic;
using System.ComponentModel;

namespace Asana.Library.Models
{
    public class Project : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<ToDo> ToDos { get; set; } = new List<ToDo>();

        private double _completePercent;
        public double CompletePercent
        {
            get => _completePercent;
            set
            {
                if (_completePercent != value)
                {
                    _completePercent = value;
                    OnPropertyChanged(nameof(CompletePercent));
                }
            }
        }

        public Project()
        {
            ToDos = new List<ToDo>();
        }

        public void InitializeListeners()
        {
            foreach (var todo in ToDos)
            {
                todo.PropertyChanged += ToDo_PropertyChanged;
            }
        }

        private void ToDo_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ToDo.IsCompleted))
            {
                UpdateCompletion();
            }
        }

        public void UpdateCompletion()
        {
            if (ToDos.Count == 0)
            {
                CompletePercent = 0;
            }
            else
            {
                int completedCount = ToDos.Count(t => t.IsCompleted);
                CompletePercent = (double)completedCount / ToDos.Count * 100;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}