﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Xml.Linq;

namespace TaskDeskApp
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<DataModel_temp> _collection;

        public MainWindow()
        {
            InitializeComponent();
            _collection = new ObservableCollection<DataModel_temp>
            {
                new() { Id = 1, EventName = "Событие 1", EventDetail = "" },
                new() { Id = 2, EventName = "Событие 2", EventDetail = "" },
                new() { Id = 3, EventName = "Событие 3", EventDetail = "" }
            };
            PushListViewIntoGrid(2, 2, Calendar, _collection);
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var temp = GetDayofWeek(2021, 10);
            var temp2 = GetColumnInCalendarFirsDayOfMonth(2021, 10);
            MessageBox.Show($"Первый день месяца {temp} // {temp2}");
            CreateTask createTask = new CreateTask();
            createTask.Show();
        }

        public void CalendarReDraw(ObservableCollection<ObservableCollection<DataModel_temp>> eventsMonthCollection,
            int Year, int Month)
        {
            var startColumn = GetColumnInCalendarFirsDayOfMonth(Year, Month);
            var index = 0;
            for (int row = 1; row < 5; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    if (row == 1 && column < startColumn)
                    {
                        break;
                    }
                    else
                    {
                        PushListViewIntoGrid(row, column, Calendar, eventsMonthCollection[index]);
                        index++;
                    }
                }
            }
        }

        private int GetColumnInCalendarFirsDayOfMonth(int Year, int Month)
        {
            var numInWeek = GetDayofWeek(Year, Month);
            switch (numInWeek)
            {
                case int n when (n > 1 && n < 7): return numInWeek - 1;
                case 0: return 6;
                default: return -1;
            }
        }

        private int GetDayofWeek(int Year, int Month)
        {
            DateTime beginningOfMonth = new DateTime(Year, Month, 3);
            return (int)beginningOfMonth.DayOfWeek;
        }

        private void PushListViewIntoGrid(int row, int column, Grid gridname,
            ObservableCollection<DataModel_temp> _OBScollection)
        {
            //var listboxitem = new ListBoxItem().Content = "Событие 1";
            //listView.Items.Add(listboxitem);

            var listView = PushEventIntoListview(_OBScollection);
            RemoveChildElementFromGrid(row, column, gridname);
            Grid.SetRow(listView, row);
            Grid.SetColumn(listView, column);

            gridname.Children.Add(listView);
        }

        private ListView PushEventIntoListview(ObservableCollection<DataModel_temp> _OBScollection)
        {
            var gridColumnID = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("id"),
            };
            var gridColumnName = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("EventName"),
            };

            var gridView = new GridView();
            gridView.Columns.Add(gridColumnID);
            gridView.Columns.Add(gridColumnName);

            ListView listView = new ListView
            {
                View = gridView,
                ItemsSource = _OBScollection,
                //Margin = new Thickness(2),
                //Padding = new Thickness(1),
                // HorizontalAlignment = HorizontalAlignment.Center,
                //VerticalAlignment = VerticalAlignment.Top
            };

            return listView;
        }

        private void RemoveChildElementFromGrid(int row, int column, Grid gridname)
        {
            for (int i = 0; i < gridname.Children.Count; i++)
            {
                if ((Grid.GetRow(gridname.Children[i]) == row) && ((Grid.GetColumn(gridname.Children[i])) == column))
                {
                    gridname.Children.Remove(gridname.Children[i]);
                    break;
                }
            }
        }

        private void RemoveSelectedEventFromCalendar()
        {
            foreach (var obj in Calendar.Children)
            {
                if (obj is ListView list)
                {
                    try
                    {
                        _collection.Remove((DataModel_temp)list.SelectedItem);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить событие?", "Удаление записи", MessageBoxButton.OKCancel,
                MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                RemoveSelectedEventFromCalendar();
            }
        }
    }
}