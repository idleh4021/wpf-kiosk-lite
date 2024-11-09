using KIOSK_LITE.Pages;
using KIOSK_LITE.Repositories;
using KIOSK_LITE.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KIOSK_LITE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((TabItem)tabMain.SelectedItem == tabKiosk) btnSave.Visibility = btnRefresh.Visibility = Visibility.Collapsed;
            else btnSave.Visibility = btnRefresh.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrameLoad();
        }

        private void MainFrameLoad()
        {
            MenuPage menu = new MenuPage();
            if ((Page)menuFrame.Content == null) menuFrame.Navigate(menu);

            CategoryPage catg = new CategoryPage();
            if ((Page)categoryFrame.Content == null) categoryFrame.Navigate(catg);
            OptionPage option = new OptionPage();
            if ((Page)optionFrame.Content == null) optionFrame.Navigate(option);

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            switch (((TabItem)tabMain.SelectedItem).Name)
            {
                case nameof(tabCategory):
                    ((CategoryPage)categoryFrame.Content).SaveCategory();
                    break;
                case nameof(tabMenu):
                    ((MenuPage)menuFrame.Content).SaveMenu();
                    break;
                case nameof(tabOption):
                    ((OptionPage)optionFrame.Content).SaveOption();
                    break;
            }

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            switch (((TabItem)tabMain.SelectedItem).Name)
            {
                case nameof(tabCategory):
                    categoryFrame.Content = null;
                    CategoryPage catg = new CategoryPage();
                    categoryFrame.Navigate(catg);
                    break;
                case nameof(tabMenu):
                    menuFrame.Content = null;
                    MenuPage menu = new MenuPage();
                    menuFrame.Navigate(menu);
                    break;
                case nameof(tabOption):
                    optionFrame.Content = null;
                    OptionPage option = new OptionPage();
                    optionFrame.Navigate(option);
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OrderView view = new OrderView();
            view.ShowDialog();
        }

        private void btnCreateDefaultData_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("테스트 기초정보를 생성합니다.", "기초정보", MessageBoxButton.YesNo);
            if(dr == MessageBoxResult.Yes)
            {
                CopyTestDate();
                AllRefresh();
            }
        }

        private void AllRefresh()
        {
             
                categoryFrame.Content = null;
                CategoryPage catg = new CategoryPage();
                categoryFrame.Navigate(catg);
             
            
                menuFrame.Content = null;
                MenuPage menu = new MenuPage();
                menuFrame.Navigate(menu);
            
            
                optionFrame.Content = null;
                OptionPage option = new OptionPage();
                optionFrame.Navigate(option);
            
            }

        private void CopyTestDate()
        {
            CsvHelper.CopyTestData();
        }
    }
}