using Experimental_Workshop.ApplicationResourses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.Model
{
    public class JobTitle : ObservableObject
    {
        private int _id;
        private string _title;
        private int? _amountOfWorkers;
        private ObservableCollection<Worker> _workers;

        public int Id 
        { 
            get => _id; 
            set => Set(ref _id, value); 
        }
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        public int? AmountOfWorkers
        {
            get => _amountOfWorkers;
            set => Set(ref _amountOfWorkers, value);
        }
        public ObservableCollection<Worker> Workers
        {
            get => _workers; 
            set => Set(ref _workers, value);
        }
        public override bool Equals(object? obj)
        {
            return (obj==null || obj is not JobTitle)? false : Id == (obj as JobTitle).Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
