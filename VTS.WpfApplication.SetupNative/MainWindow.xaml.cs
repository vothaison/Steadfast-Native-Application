using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using Newtonsoft.Json;
using Utility.ModifyRegistry;

namespace VTS.WpfApplication.SetupNative
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            textboxId.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            this.Save();

        }

        private void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(new
                {
                    name = "com.example.native",
                    description = "Native support for Chrome Extension",
                    path = "VTS.ChromeNativeMessaging.exe",
                    type = "stdio",
                    allowed_origins = new List<string>()
                    {
                        String.Format("chrome-extension://{0}/", textboxId.Text)
                    }
                });


                File.WriteAllText("manifest.json", json);
                this.WriteRegistry();
                this.Close();
            }
            catch (Exception e)
            {
                File.WriteAllText(@"D:\VTS.WpfApplication.SetupNative_error.txt", e.Message);
                throw;
            }
        }

        private void WriteRegistry()
        {
            RegistryKey myKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Google\Chrome", true);
            if (myKey != null)
            {
                RegistryKey nativeMessagingHostsKey = myKey.CreateSubKey("NativeMessagingHosts");

                if (nativeMessagingHostsKey != null)
                {
                    RegistryKey nativeClientKey = nativeMessagingHostsKey.CreateSubKey("com.example.native");

                    if (nativeClientKey != null)
                    {
                        nativeClientKey.SetValue("", System.IO.Path.Combine(Environment.CurrentDirectory, "manifest.json"),
                            RegistryValueKind.String);

                        //string value = myKey.GetValue("(Default)").ToString();
                        //myKey.DeleteValue("(Default)");
                        //myKey.SetValue("(Default)", "asdfsa", RegistryValueKind.String);
                        nativeClientKey.Close();
                    }
                    nativeMessagingHostsKey.Close();
                }

                myKey.Close();
            }
        }

        private void buttonUninstall_Click(object sender, RoutedEventArgs e)
        {
            Uninstall();
        }

        private void Uninstall()
        {
            RegistryKey myKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Google\Chrome\NativeMessagingHosts", true);
            if (myKey != null)
            {
                myKey.DeleteSubKey("com.example.native");
                myKey.Close();
            }
        }
    }
}
