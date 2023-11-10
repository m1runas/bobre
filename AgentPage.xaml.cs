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


namespace чзхноваялаба

{ 
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;

        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
        public AgentPage()
        {
            InitializeComponent();
            var currentAgent = ivanov_GlazkiEntities.GetContext().Agent.ToList();

            ServiceListView.ItemsSource = currentAgent;
            ComboType.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;
        }

        public void UpdateAgent()
        {
            var currentAgent = ivanov_GlazkiEntities.GetContext().Agent.ToList();
            if (ComboType.SelectedIndex == 0)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID >= 1 && p.AgentTypeID <= 6)).ToList();
            }
            if (ComboType.SelectedIndex == 1)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 1)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 2)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 3)).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 4)).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 5)).ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 6)).ToList();
            }



            if (ComboSort.SelectedIndex == 0)//все
            {
            }
            if (ComboSort.SelectedIndex == 1)//наименование(А до Я) 1
            {
                currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
            }
            if (ComboSort.SelectedIndex == 2)//наименование(Я до А) 2
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
            }
            if (ComboSort.SelectedIndex == 3)//размер скидки(по возрастанию) 3
            {
            }
            if (ComboSort.SelectedIndex == 4)//размер скидки(по убыванию) 4 
            {
            }
            if (ComboSort.SelectedIndex == 5)//приоритет агента(по возрастанию) 5
            {
                currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();
            }
            if (ComboSort.SelectedIndex == 6) //приоритет агента(по убыванию) 6
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();
            }                           
               
            currentAgent = currentAgent.Where(p =>
            p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())
            || p.Phone.Replace("+7", "8").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "")
            .Contains(TBoxSearch.Text.Replace("+7", "8").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""))
            || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();


            TableList = currentAgent;
            ServiceListView.ItemsSource = currentAgent;
            ChangePage(0, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            manager.MainFrame.GoBack();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();
        }

        

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;
            if (CountRecords % 10 > 0) CountPage = CountRecords / 10 + 1;
            else CountPage = CountRecords / 10;

            Boolean Ifupdate = true;

            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++) CurrentPageList.Add(TableList[i]);
                }

            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++) CurrentPageList.Add(TableList[i]);
                        }
                        else Ifupdate = false;
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++) CurrentPageList.Add(TableList[i]);
                        }
                        else Ifupdate = false;
                        break;
                }

            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;

                ServiceListView.ItemsSource = CurrentPageList;
                ServiceListView.Items.Refresh();
            }
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString())-1);
        }

        private void AddEditButton_Click(object sender, RoutedEventArgs e)
        {
            manager.MainFrame.Navigate(new AddEditAgent((sender as Button).DataContext as Agent));            
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            manager.MainFrame.Navigate(new AddEditAgent());
        }

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateAgent();
        }
    }
}
