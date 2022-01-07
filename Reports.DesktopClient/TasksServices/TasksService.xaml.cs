using System.Windows;

namespace Reports.DesktopClient.TasksServices
{
    public partial class TasksService : Window
    {
        public TasksService()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            Tabs tabs = new Tabs();
            tabs.Show();
            Hide();
        }
        private void Get_All(object sender, RoutedEventArgs e)
        {
            GetAll getAll = new GetAll();
            getAll.Show();
            Hide();
        }
        
        private void Find_task(object sender, RoutedEventArgs e)
        {
            FindTask findTask = new FindTask();
            findTask.Show();
            Hide();
        }
        
        private void Update_task(object sender, RoutedEventArgs e)
        {
            UpdateTask updateTask = new UpdateTask();
            updateTask.Show();
            Hide();
        }
        
        private void Create_task(object sender, RoutedEventArgs e)
        {
            CreateTask createTask = new CreateTask();
            createTask.Show();
            Hide();
        }
    }
}