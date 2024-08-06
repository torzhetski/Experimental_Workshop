using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Experimental_Workshop.MVVM.ViewModel.DataTableViewModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Experimental_Workshop.MVVM.View.DataTableView
{
    /// <summary>
    /// Interaction logic for WorkersDataTableView.xaml
    /// </summary>
    public partial class WorkersDataTableView : UserControl
    {
        public WorkersDataTableView()
        {
            InitializeComponent();
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {

            }
        }

    }
}
