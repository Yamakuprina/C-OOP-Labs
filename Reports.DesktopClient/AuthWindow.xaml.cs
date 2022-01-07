using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Reports.Models.Model;
using Reports.Server;

namespace Reports.DesktopClient
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            string guid = TextBoxLogin.Text;
            Employee user = Identify(guid);
            if (user == null)
            {
                TextBoxLogin.ToolTip = "Wrong ID";
                TextBoxLogin.Background = Brushes.DarkRed;
            }
            else
            {
                TextBoxLogin.Background = Brushes.Transparent;
                App.User = user;
                Tabs tabs = new Tabs();
                tabs.Show();
                this.Hide();
            }
        }
        
        private static Employee Identify(string guid)
        {
            WebRequest request = HttpWebRequest.Create($"https://localhost:5001/employees/?guid={guid}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new ReportsException("No response from the server");
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();

                Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
                if (employee == null)
                {
                    throw new ReportsException("Object cant be deserialized");
                }
                return employee;
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
    
    
}