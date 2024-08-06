using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace Experimental_Workshop.MVVM.View.ReportsView
{
    /// <summary>
    /// Interaction logic for CrossJoinView.xaml
    /// </summary>
    public partial class CrossJoinView : UserControl
    {
        string connectionString = @"Server=(localdb)\mssqllocaldb;Database=ExperimentalWorkshop;Trusted_Connection=True;";

        //SqlConnection connection;
        SqlDataAdapter adapter;
        DataTable dataTable;
        SqlCommand cmd;
        public CrossJoinView()
        {
            InitializeComponent();

            Load();
        }

        private void Load()
        {
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataTable dataTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter();

                // Запрос для получения списка всех годов
                string sqlGetYears = @"SELECT DISTINCT YEAR(DateOfStart) AS Year FROM TechnologyCards";

                // Выполнение запроса и получение результата
                adapter.SelectCommand = new SqlCommand(sqlGetYears, connection);
                adapter.Fill(dataTable);

                List<string> years = new List<string>();

                // Извлечение годов из результата запроса
                foreach (DataRow row in dataTable.Rows)
                {
                    years.Add(row["Year"].ToString());
                }
                dataTable.Reset();

                // Формирование строки для динамического SQL-запроса с учетом всех годов
                StringBuilder dynamicSqlSelect = new StringBuilder();
                dynamicSqlSelect.Append("SELECT ");
                dynamicSqlSelect.Append("Name, ");

                // Добавление столбцов для каждого года
                foreach (string year in years)
                {
                    dynamicSqlSelect.Append($"ISNULL([{year}], 0) AS [{year}], ");
                }

                // Удаление последней запятой
                dynamicSqlSelect.Length -= 2;

                dynamicSqlSelect.Append(" FROM (SELECT tc.Name, " +
                                        "YEAR(tc.DateOfStart) AS Year, " +
                                        "COUNT(e.Name) AS EquipmentCount " +
                                        "FROM TechnologyCards tc " +
                                        "INNER JOIN EquipmentTechnologyCard tce ON tc.Id = tce.TechnologyCardsId " +
                                        "INNER JOIN Equipment e ON tce.EquipmentId = e.Id " +
                                        "GROUP BY tc.Name, YEAR(tc.DateOfStart)) AS SourceTable " +
                                        "PIVOT " +
                                        "( " +
                                        "SUM(EquipmentCount) " +
                                        "FOR Year IN (");

                // Добавление динамически формируемых столбцов для годов
                foreach (string year in years)
                {
                    dynamicSqlSelect.Append($"[{year}], ");
                }

                // Удаление последней запятой
                dynamicSqlSelect.Length -= 2;

                dynamicSqlSelect.Append(")) AS PivotTable");

                // Выполнение динамического запроса
                adapter.SelectCommand = new SqlCommand(dynamicSqlSelect.ToString(), connection);
                adapter.Fill(dataTable);

                // Привязка данных к вашему DataGrid
                pivotedDataGrid.DataContext = dataTable;
            }
        }
    }
}