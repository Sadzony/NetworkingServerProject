using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Threading;
using System.Windows.Threading;

namespace ClientProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Client client)
        {
            InitializeComponent();
        }
        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (messageText.Text == "")
            {
                MessageBox.Show("No Message in text box!", "Warning");
            }
            else
            {
                string message = messageText.Text;
                messageText.Text = "";
                if (localName.Text == "")
                {
                    MessageBox.Show("Please enter a local name!", "Warning");
                    messageText.Text = message;
                }
                else
                {
                    string name = localName.Text;
                    message = name + " says: " + message + "\n";
                    ThreadStart ButtonThread = delegate ()
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action<string>(SendMessage), message);
                    };
                    new Thread(ButtonThread).Start();

                }
            }
        }
        private void SendMessage(string status)
        {
            chatbox.Text = status;
        }
    }
}
