using Experimental_Workshop.ApplicationResourses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Experimental_Workshop.MVVM.Model
{
    public class Worker : ObservableObject
    {
        public event Action<int,int>? onJobTitleIdChanged;


        private int _id;
        private string _name;
        private string _secondName;
        private DateTime _dateOfBirth;
        private string _email;
        private ObservableCollection<JobTitle> _allJobTitles;
        private ObservableCollection<TechnologyCard> _restTechnologyCards;

        private int _jobTitleId;
        private JobTitle? _jobTitle;
        private ObservableCollection<TechnologyCard> _technologyCards;

        public int Id {
            get => _id;
            set => Set(ref _id, value); 
        }
        public string Name 
        { 
            get => _name; 
            set => Set(ref _name, value);
        }
        public string SecondName
        {
            get => _secondName;
            set => Set(ref _secondName, value);
        }
        public DateTime DateOfbirth
        {
            get => _dateOfBirth;
            set => Set(ref _dateOfBirth, value);
        }
        public string Email
        {
            get => _email; 
            set => Set(ref _email, value);
        }
        [NotMapped]
        public ObservableCollection<JobTitle> AllJobTitles
        {
            get => _allJobTitles;
            set => Set(ref _allJobTitles, value);
        }
        [NotMapped]
        public ObservableCollection<TechnologyCard> RestTechnologyCards
        {
            get => _restTechnologyCards;
            set => Set(ref _restTechnologyCards, value);
        }
        public int JobTitleId
        {
            get => _jobTitleId;
            set => Set(ref _jobTitleId, value); 

        }
        public JobTitle? JobTitle
        {
            get => _jobTitle;
            set 
            {
                onJobTitleIdChanged?.Invoke(_jobTitle.Id, value.Id);
                Set(ref _jobTitle, value); 
            }
        }
        public ObservableCollection<TechnologyCard> TechnologyCards
        {
            get => _technologyCards;
            set => Set(ref _technologyCards, value);
        }
        public Worker()
        {
            TechnologyCards = new ObservableCollection<TechnologyCard>();
            DateOfbirth = DateTime.Now;
        }
        public override bool Equals(object? obj)
        {
            return (obj == null || obj is not Worker) ? false : Id == (obj as Worker).Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
