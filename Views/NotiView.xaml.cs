using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KIOSK_LITE.Views
{
    /// <summary>
    /// NotiView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NotiView : Window ,INotifyPropertyChanged
    {
        private int _timeCnt = 5;
        public int TimeCnt { get => _timeCnt; set { _timeCnt = value; OnPropertyChanged(nameof(TimeCnt)); } }

        private string _caption;
        public string Caption { get => _caption; set { _caption = value;OnPropertyChanged(nameof(Caption)); } }
        private string _mainText;
        public string MainText { get => _mainText; set { _mainText = value; OnPropertyChanged(nameof(MainText)); } }

        private string _subText;
        public string SubText { get => _subText; set { _subText = value; OnPropertyChanged(nameof(SubText)); } }

        private string _btnCaption;
        public string btnCaption { get => _btnCaption; set { _btnCaption = value; OnPropertyChanged(nameof(btnCaption)); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        DispatcherTimer timer = new DispatcherTimer();

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
       // public NotiView()
       // {
       //     InitializeComponent();
       //     DataContext = this;
       // }

        public NotiView(string Caption,string MainText,string SubText)
        {
            InitializeComponent();
            DataContext = this;
            this.Caption = Caption;
            this.MainText = MainText;
            this.SubText = SubText;
            btnCaption = $"확인 ({TimeCnt})";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop(); Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (TimeCnt == 0) { timer.Stop(); Close(); return; }
            btnCaption = $"확인 ({--TimeCnt})";
        }
    }
}
