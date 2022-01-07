using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient.EmployeeServices
{
    public partial class DeleteEmployee : Window
    {
        public DeleteEmployee()
        {
            InitializeComponent();
        }
        private void Go_back(object sender, RoutedEventArgs e)
        {
            EmployeeService employeeService = new EmployeeService();
            employeeService.Show();
            Hide();
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            string name = TextBoxName.Text;
            HttpStatusCode statusCode = DeleteEmployeeByName(name);
            if (statusCode == HttpStatusCode.OK)
            {
                H1Deleted.Text = "Successfully deleted employee ";
            }
            else
            {
                H1Deleted.Text = "Cant delete employee ";
            }
        }
        
        private static HttpStatusCode DeleteEmployeeByName(string name)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/delete/?name={name}");
            request.Method = WebRequestMethods.Http.Post;
            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new ReportsException("No response from the server");
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();

            var status = JsonConvert.DeserializeObject<HttpStatusCode>(responseString);
            return status;
        }
    }
}