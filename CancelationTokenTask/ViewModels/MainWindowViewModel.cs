using CancelationTokenTask.Command;
using CancelationTokenTask.Encrypttion;
using CancelationTokenTask.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace CancelationTokenTask.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindow MainWindow { get; set; }
        public RelayCommand FileBtnCommand { get; set; }
        public RelayCommand StratBtnCommand { get; set; }
        public RelayCommand CancelBtnCommand { get; set; }
        bool cancel = false;

        string text = string.Empty;
        string path = string.Empty;

        double p = 0.0;
        double pMax = 0.0;
        double pMin = 0.0;
        long length = 0;
        bool hasEncrypt = false;
        bool hasDecrypt = false;
        string passwordtext = string.Empty;
        bool hash = false;
        int count = 100;
        int currentcount = 0;
        int count2 = 50;

        private double value;
        public double Value { get { return value; } set { this.value = value; OnPropertyChanged(); } }

        public MainWindowViewModel(MainWindow mainWindow)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(4);
            timer.Tick += Timer_Tick;
            timer.Start();
            //MainWindow = new MainWindow();
            MainWindow = mainWindow;

            FileBtnCommand = new RelayCommand((sender) =>
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.InitialDirectory = Path.GetFullPath(Environment.CurrentDirectory + @"../../../../");
                openFile.Filter = "Text files (*.txt)|*.txt";

                if (openFile.ShowDialog() == true)
                {
                    mainWindow.fileTxtbx.Text = openFile.FileName;
                    path = openFile.FileName;
                }

            });

            StratBtnCommand = new RelayCommand((sender) =>
            {

                timer.Start();
                try
                {

                    if (mainWindow.Passwordtxtbox.Password != string.Empty)
                    {


                        if (cancellationTokenSource != null)
                        {
                            ThreadPool.QueueUserWorkItem((j) => { ED(mainWindow.Passwordtxtbox.Password, Document.GetText(), hash, cancellationTokenSource.Token); });
                            if (mainWindow.encyrptBtn.IsChecked == true)
                            {
                                mainWindow.Passwordtxtbox.IsEnabled = false;
                                hasEncrypt = true;


                            }
                            else if (mainWindow.decyrptBtn.IsChecked == true)
                            {

                                mainWindow.Passwordtxtbox.IsEnabled = false;
                                hasDecrypt = true;
                            }
                        }


                    }
                    else
                    {
                        MessageBox.Show("Input password: ");
                    }



                    cancel = true;

                }
                catch (Exception)
                {


                }



            });

            CancelBtnCommand = new RelayCommand((sender) =>
            {

                try
                {
                    if (cancel == true)
                    {
                        if (cancellationTokenSource != null)
                        {
                            ThreadPool.QueueUserWorkItem((i) => { ED(passwordtext, Document.GetText(), hash, cancellationTokenSource.Token); });


                            cancellationTokenSource.Cancel();

                        }
                    }
                }
                catch (Exception)
                {


                }


            });



        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MainWindow.startBtn.Dispatcher.BeginInvoke(new Action(() =>
            {

                path = MainWindow.fileTxtbx.Text;
                p = MainWindow.mainProgressBar.Value;
                pMax = MainWindow.mainProgressBar.Maximum;
                pMin = MainWindow.mainProgressBar.Minimum;
                hasEncrypt = (bool)MainWindow.encyrptBtn.IsChecked;
                hasDecrypt = (bool)MainWindow.decyrptBtn.IsChecked;
                passwordtext = MainWindow.Passwordtxtbox.Password;


            }));
        }

        private void ED(string passwordtext, string text, bool useHashing, CancellationToken token)
        {

            if (path != string.Empty)
            {
                length = new System.IO.FileInfo(path).Length;

                string w = string.Empty;
                string w2 = string.Empty;

                if (!token.IsCancellationRequested)
                {
                    if (hasEncrypt == true)
                    {

                        for (int i = 0; i < count; i++)
                        {
                            Thread.Sleep(count2);
                            if (i == count - 1)
                            {

                                for (int j = 0; j < 101; j++)
                                {
                                    Thread.Sleep(500);


                                    Value = j;

                                    if (Value == 100)
                                    {
                                        try
                                        {
                                            w = EncryptDecrypt.EString(passwordtext, Document.GetText());

                                            EncryptDecrypt.FileStreamWrite(w, path);
                                            EncryptDecrypt.FileStreamRead(w, path);

                                            Thread.Sleep(500);

                                            Value = 0;

                                        }
                                        catch (Exception)
                                        {

                                        }
                                    }

                                    if (Value != 100)
                                    {
                                        try
                                        {

                                            w = EncryptDecrypt.EString(passwordtext, Document.GetText().Substring(0, Convert.ToInt32(100 * (j++))));

                                            EncryptDecrypt.FileStreamWrite(w, path);
                                            EncryptDecrypt.FileStreamRead(w, path);


                                            Thread.Sleep(400);


                                        }
                                        catch (Exception)
                                        {


                                        }

                                    }

                                    currentcount = int.Parse(Value.ToString());



                                }
                            }

                        }
                        hasEncrypt = false;

                    }

                    if (hasDecrypt == true)
                    {

                        File.WriteAllText(path, string.Empty);

                        for (int i = 0; i < count; i++)
                        {
                            Thread.Sleep(count2);
                            if (i == count - 1)
                            {

                                for (int j = 0; j < 101; j++)
                                {
                                    Thread.Sleep(50);
                                    Value = j;


                                    if (Value == 100)
                                    {
                                        try
                                        {
                                            w2 = EncryptDecrypt.DString(passwordtext, EncryptDecrypt.EString(passwordtext, Document.GetText()));

                                            EncryptDecrypt.FileStreamWrite(w2, path);
                                            EncryptDecrypt.FileStreamRead(w2, path);



                                            Thread.Sleep(500);

                                            Value = 0;

                                        }
                                        catch (Exception)
                                        {

                                        }
                                    }

                                    if (Value != 100)
                                    {
                                        try
                                        {

                                            w2 = EncryptDecrypt.DString(passwordtext, EncryptDecrypt.EString(passwordtext, Document.GetText().Substring(0, Convert.ToInt32(100 * (j++)))));

                                            EncryptDecrypt.FileStreamWrite(w2, path);
                                            EncryptDecrypt.FileStreamRead(w2, path);



                                            Thread.Sleep(500);

                                            
                                        }
                                        catch (Exception)
                                        {


                                        }

                                    }

                                    currentcount = int.Parse(Value.ToString());

                                }

                            }
                        }


                    }
                    hasDecrypt = false;

                }

                if (token.IsCancellationRequested)
                {

                    if (File.Exists(path))
                    {
                        File.WriteAllText(path, string.Empty);
                    }

                    for (int j = currentcount; j >= 0; j--)
                    {
                        Thread.Sleep(50);
                        Value = j;
                    }

                    MessageBox.Show($"Process canceled.");


                }
            }



        }




    }
}
