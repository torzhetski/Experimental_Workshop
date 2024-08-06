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
    public class Materials : ObservableObject
    {
        private int _id;
        private string _name;
        private int _amount;
        private ObservableCollection<TechnologyCard> _technologyCards;
        private ObservableCollection<TechnologyCard> _restTechnologyCards;

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
        public int Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }
        public ObservableCollection<TechnologyCard> TechnologyCards
        {
            get => _technologyCards;
            set => Set(ref _technologyCards, value);
        }
        [NotMapped]
        public ObservableCollection<TechnologyCard> RestTechnologyCards
        {
            get => _restTechnologyCards;
            set => Set(ref _restTechnologyCards, value);
        }
        public Materials()
        {
            TechnologyCards = new ObservableCollection<TechnologyCard>();
        }
        public override bool Equals(object? obj)
        {
            return (obj == null || obj is not Materials) ? false : Id == (obj as Materials).Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
