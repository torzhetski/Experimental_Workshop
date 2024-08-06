using Experimental_Workshop.ApplicationResourses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.Model
{
    public class MicroprocessorTypes : ObservableObject
    {
        private int _id;
        private string _name;
        private int _amountOfCores;
        private ObservableCollection<TechnologyCard> _technologyCards;


        public int Id
        { 
            get => _id;
            set => Set(ref _id, value);
        }
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        public int AmountOfCores
        {
            get => _amountOfCores; 
            set => Set(ref _amountOfCores, value);
        }
        public ObservableCollection<TechnologyCard> TechnologyCards 
        {
            get => _technologyCards; 
            set => Set(ref _technologyCards, value);
        }
        public MicroprocessorTypes()
        {
            TechnologyCards = new ObservableCollection<TechnologyCard>();
        }
        public override bool Equals(object? obj)
        {
            return (obj == null || obj is not MicroprocessorTypes) ? false : Id == (obj as MicroprocessorTypes).Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
