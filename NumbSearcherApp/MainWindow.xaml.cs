using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace NumbSearcherApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private WorkClass worker = null;
        public MainWindow() {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e) {
            int countOfThreads = 0;
            int targetNumber = 0;
            int interval = 0;
            worker = WorkClass.GetWorkClassInstance0();
            switch (btnStart.Content as string) {
                case "Start":       
                    btnStart.Content = "Stop";
                    lbxResults.Items.Clear();
                    try {
                        countOfThreads = Int32.Parse(txbCountOfThreads.Text);
                        targetNumber = Int32.Parse(txbTargetNumber.Text);
                        interval = Int32.Parse(txbInterval.Text);
                    } catch (FormatException ex) {
                        MessageBox.Show(ex.Message);
                    }
                    if (countOfThreads > 0 && -1 < targetNumber && targetNumber < 100 && -1 < interval) {
                        worker.CreateAndStartSearch(countOfThreads, targetNumber, interval);
                    } else MessageBox.Show("Error. Число потоков должно быть положительным. Интервал должен быть неотрицательным. Искомое число должно быть в диапазоне 0 - 99");
                    break;
                case "Stop":
                    // тут вызов метода для остановки работы
                    if(!worker.Equals(null))
                        worker.StopAll();
                    btnStart.Content = "Start";
                    break;
                default:
                    break;
            }            

        }
    }
}
