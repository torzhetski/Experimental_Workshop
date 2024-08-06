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
    public class Equipment : ObservableObject
    {
        private int _id;
        private string _name;
        private DateTime _expluatationStartDate;
        private ObservableCollection<TechnologyCard> _technologyCards;
        private ObservableCollection<TechnologyCard> _restTechnologycards;
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
        public DateTime ExpluatationStartDate
        {
            get => _expluatationStartDate;
            set => Set(ref _expluatationStartDate, value);
        }
        public ObservableCollection<TechnologyCard> TechnologyCards
        {
            get => _technologyCards;
            set => Set (ref _technologyCards, value);
        }
        [NotMapped]
        public ObservableCollection<TechnologyCard> RestTechnologyCards
        {
            get => _restTechnologycards;
            set => Set(ref _restTechnologycards, value);
        }
        public Equipment()
        {
            ExpluatationStartDate = DateTime.Now;
            TechnologyCards = new ObservableCollection<TechnologyCard>();
        }
        public override bool Equals(object? obj)
        {
            return (obj == null || obj is not Equipment) ? false : Id == (obj as Equipment).Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
