using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

namespace Experimental_Workshop.MVVM.View
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window,INotifyPropertyChanged
    {
        string connectionString = @"Server=(localdb)\mssqllocaldb;Database=ExperimentalWorkshop;Trusted_Connection=True;";
        ObservableCollection<string> tables = new() { "Workers", "Equipment", "JobTitles", "Materials", "MicroprocessorTypes", "TechnologyCards" };
        string tableName;
        ObservableCollection<SqlDbType> dbTypes = new() {SqlDbType.Int,SqlDbType.NVarChar,SqlDbType.Bit};
        
        SqlConnection connection;
        SqlDataAdapter adapter;
        DataTable dataTable;
        DataRowView rowView;
        SqlCommand cmd;
        public delegate void DatabaseUpdatedEventHandler();
        public event DatabaseUpdatedEventHandler? DatabaseUpdated;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> Tables
        {
            get { return tables; }
        }
        public string TableName
        {
            get { return tableName; }
            set
            {
                tableName = value;
                RaisePropertyChanged(nameof(tableName));
            }
        }
        public AdminWindow()
        {
            InitializeComponent();
            tableName = tables[0];
            TablesCombobox.ItemsSource = Tables;
            TablesCombobox.SelectedItem = tableName;
            TypesCombobox.ItemsSource = dbTypes;
            
            Load(); 
        }
        private async void Load()
        {

            connection = new SqlConnection(connectionString);
            dataTable = new DataTable();
            adapter = new SqlDataAdapter();     
            #region select
            string sqlSelect = $"SELECT * FROM {TablesCombobox.SelectedItem}";
            adapter.SelectCommand = new SqlCommand(sqlSelect, connection);
            #endregion
            #region delete
            string deleteString = $"DELETE FROM {TablesCombobox.SelectedItem} Where Id = @Id";
            adapter.DeleteCommand = new SqlCommand(deleteString, connection);
            adapter.DeleteCommand.Parameters.Add("@Id",SqlDbType.Int,4,"Id");
            #endregion
            adapter.Fill(dataTable);

            UpdateParametrSetting();
            InsertParametrSetting();
            
            DataGrid.DataContext = dataTable;
            ObservableCollection<string> list = new();
            foreach (var e in dataTable.Columns)
            {
                list.Add(e.ToString());
            }
            ColumnsCombobox.ItemsSource = list;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            string sqlAddColumns = $"ALTER TABLE {TablesCombobox.SelectedItem} ADD {ColumnNameTextBox.Text} {TypesCombobox.SelectedItem}";
            cmd = new SqlCommand(sqlAddColumns, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            dataTable.Clear();
            Load();

        }


        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            rowView = (DataRowView)DataGrid.SelectedItem;   
            
            
            rowView.BeginEdit();

        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowView == null) return;
            rowView.EndEdit();
            //if (rowView.IsNew)
            //{
            //    dataTable.Rows.Add(rowView);
            //}
            adapter.Update(dataTable);
            DatabaseUpdated?.Invoke();
        }



        private void UpdateParametrSetting()
        {
            string supportUpdate = string.Empty;
            string supportUpdateId = string.Empty;
            int i = 0;
            foreach (var col in dataTable.Columns)
            {

                if (i == 0)
                {
                    supportUpdateId = $" {col} = @{col}";
                    i++;
                }
                else
                    supportUpdate += $" {col} = @{col},";
            }
            supportUpdate = supportUpdate.Remove(supportUpdate.LastIndexOf(','));
            string sqlUpdate = $"UPDATE {TablesCombobox.SelectedItem} SET" + supportUpdate + " WHERE" + supportUpdateId;

            adapter.UpdateCommand = new SqlCommand(sqlUpdate, connection);

            foreach (var col in dataTable.Columns)
            {
                if (col.ToString().Contains("Id")||col.ToString().Contains("Amount") || col.ToString().Contains("Age") || col.ToString().Contains("Power") || col.ToString().Contains("Frequency"))
                {
                    adapter.UpdateCommand.Parameters.Add($"@{col}", SqlDbType.Int, 10, $"{col}");
                }
                else if(col.ToString().Contains("Date"))
                {
                    adapter.UpdateCommand.Parameters.Add($"@{col}", SqlDbType.DateTime2, 20, $"{col}");
                }
                else
                {
                    adapter.UpdateCommand.Parameters.Add($"@{col}", SqlDbType.NVarChar,100, $"{col}");
                }
               
            }
        }
        private void InsertParametrSetting()
        {
            string supportInsertNames = string.Empty;
            string supportInsertValues = string.Empty;
            int i = 0;
            foreach (var col in dataTable.Columns)
            {
                if (col.ToString() == "Id")
                    continue;

                    supportInsertNames += $"{col}, ";
                supportInsertValues += $"@{col}, ";
            }
            supportInsertNames = supportInsertNames.Remove(supportInsertNames.LastIndexOf(','));
            supportInsertValues = supportInsertValues.Remove(supportInsertValues.LastIndexOf(','));
            string sqlInsert = $"INSERT INTO {TablesCombobox.SelectedItem} ({supportInsertNames})" +  $" VALUES ({supportInsertValues});" + " SET @Id = @@IDENTITY;";

            adapter.InsertCommand = new SqlCommand(sqlInsert, connection);

            foreach (var col in dataTable.Columns)
            {
                if (col.ToString().Contains("Id") || col.ToString().Contains("Amount") || col.ToString().Contains("Age") || col.ToString().Contains("Power") || col.ToString().Contains("Frequency"))
                {
                    adapter.InsertCommand.Parameters.Add($"@{col}", SqlDbType.Int, 10, $"{col}");
                }
                else if (col.ToString().Contains("Date"))
                {
                    adapter.InsertCommand.Parameters.Add($"@{col}", SqlDbType.DateTime2, 20, $"{col}");
                }
                else
                {
                    adapter.InsertCommand.Parameters.Add($"@{col}", SqlDbType.NVarChar, 100, $"{col}");
                }

            }
        }
        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete) 
            {
                DatabaseUpdated?.Invoke();
            }
        }

        //private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        //{
        //    e.NewItem = dataTable.NewRow();
        //    DatabaseUpdated?.Invoke();
        //}

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TablesCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Load();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            string sqlAddColumns = $"ALTER TABLE {TablesCombobox.SelectedItem} DROP COLUMN {ColumnsCombobox.SelectedItem}";
            cmd = new SqlCommand(sqlAddColumns, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            DataGrid.Columns.Clear();
            dataTable.Clear();
            Load();
        }
    }
    
}

