using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NumbSearcherApp {
    class WorkClass {
        private static WorkClass instance;
        private static object syncRoot = new Object();

        private Random random = new Random();
        private DateTime localDate;

        private List<Thread> threads;
        private int countOfThreads;
        private int targetNumber;
        private int interval;
        bool b;
        ListBox lb = Application.Current.MainWindow.FindName("lbxResults") as ListBox;

        private WorkClass() { }
        public static WorkClass GetWorkClassInstance0() {
            if (instance == null) {
                lock (syncRoot) {
                    if (instance == null) instance = new WorkClass();
                }
            }
            return instance;
        }
        public void CreateAndStartSearch(int _countOfThreads, int _targetNumber, int _interval) {
            countOfThreads = _countOfThreads;
            targetNumber = _targetNumber;
            interval = _interval;
            threads = new List<Thread>(countOfThreads);
            b = true;
            for (int i = 0; i < countOfThreads; i++) {
                threads.Add(new Thread(new ThreadStart(DoSearch)));
                threads[i].Name = string.Format("Thread #{0}", i);
                threads[i].IsBackground = true;
            }
            foreach (Thread t in threads) {
                if (!t.Equals(null)) {
                    t.Start();
                }
            }
        }
        // main method to search targetNumber
        private void DoSearch() {
            int currentNumber = -1;
            Action<bool, string> a = (i, s) => {
                ListBoxItem item = new ListBoxItem();
                item.IsSelected = true;
                item.Content = s;
                if (i) item.Background = new LinearGradientBrush(Colors.ForestGreen, Colors.LightGreen, 90);
                else item.Background = new LinearGradientBrush(Colors.OrangeRed, Colors.Orange, 90);
                item.FontFamily = new FontFamily("Tahoma");
                lb.Items.Add(item);
            };

            while (b) {
                lock (syncRoot) {
                    if (b == false) {
                        break;
                    }
                    currentNumber = random.Next(100);
                    localDate = DateTime.Now;
                    string threadInfo = string.Format("{0}\n" + Thread.CurrentThread.Name +
                        " - " + localDate.ToString(new CultureInfo("ru-RU")), currentNumber);

                    //Console.WriteLine(threadInfo);    // write to console
                    if (currentNumber != targetNumber) {
                        if (currentNumber < targetNumber) {
                            lb.Dispatcher.Invoke(a, true, threadInfo);
                        } else lb.Dispatcher.Invoke(a, false, threadInfo);
                        Thread.Sleep(interval);
                    } else if (currentNumber == targetNumber) {
                        lb.Dispatcher.Invoke(a, true, threadInfo);
                        MessageBox.Show("I found your number!\n" + threadInfo);
                        //Console.WriteLine("I found your number!");
                        b = false;
                        StopAll();  // stop all threads
                    }
                }
            }
            Console.WriteLine("end of function"); 
        }

        internal void StopAll() {
            foreach (Thread t in threads) {
                t.Abort();
            }
            threads = null;
        }
    }
}

