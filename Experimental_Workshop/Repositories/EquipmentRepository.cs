using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.MVVM.ViewModel.ReportsViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.Repositories
{
    public class EquipmentRepository
    {
        private ApplicationContext _dbContext;
        public EquipmentRepository(ApplicationContext context) 
        {
            _dbContext = context;
        }
        public List<Equipment> GetAllEquipment()
        {
            return _dbContext.Equipment
                .Include(w => w.TechnologyCards)
                .ToList();
        }

        public void SaveAllEquipment()
        {
            _dbContext.SaveChanges();
        }
        public void AddEquipment(Equipment equipment)
        {
            _dbContext.Equipment.Add(equipment);
            _dbContext.SaveChanges();
        }

        public void RemoveEquipment(Equipment equipment)
        {
            _dbContext.Equipment.Remove(equipment);
            _dbContext.SaveChanges();
        }
        public List<EquipmentRequestVM> GetEquipmentAndTechCardAmount()
        {
            //to see sql query string
            //string e = _dbContext.Equipment
            //                            .Select(e => new EquipmentRequestVM
            //                            {
            //                                Id = e.Id,
            //                                Name = e.Name,
            //                                TechnologyCardCount = e.TechnologyCards.Count
            //                            })
            //                            .ToQueryString();
            return _dbContext.Equipment
                                        .Select(e => new EquipmentRequestVM 
                                        {
                                            Id = e.Id,
                                            Name = e.Name,
                                            TechnologyCardCount = e.TechnologyCards.Count
                                        })
                                        .ToList();
           
        }
    }
}
