using System.Windows;

namespace Reports.DesktopClient.TasksServices
{
    public partial class UpdateTask : Window
    {
        public UpdateTask()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            TasksService tasksService = new TasksService();
            tasksService.Show();
            Hide();
        }
        private void ChangeState(object sender, RoutedEventArgs e)
        {
            ChangeTaskState changeTaskState = new ChangeTaskState();
            changeTaskState.Show();
            Hide();
        }
        private void AddComment(object sender, RoutedEventArgs e)
        {
            AddTaskComment addTaskComment = new AddTaskComment();
            addTaskComment.Show();
            Hide();
        }
        private void ChangeAssignedEmployee(object sender, RoutedEventArgs e)
        {
            ChangeTaskAssignedEmployee changeTaskAssignedEmployee = new ChangeTaskAssignedEmployee();
            changeTaskAssignedEmployee.Show();
            Hide();
        }
        
        private void ChangeDescription(object sender, RoutedEventArgs e)
        {
            ChangeTaskDescription changeTaskDescription = new ChangeTaskDescription();
            changeTaskDescription.Show();
            Hide();
        }
    }
}