using Experimental_Workshop.MVVM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.Repositories
{
    public class MicroprocessorTypesRepository
    {
        private readonly ApplicationContext _dbContext;

        public MicroprocessorTypesRepository(ApplicationContext context)
        {
            _dbContext = context;
        }
        public List<MicroprocessorTypes> GetAllMicroprocessorTypes()
        {
            return _dbContext.MicroprocessorTypes.Include(w => w.TechnologyCards).ToList();
        }

        public void SaveAllMicroprocessorTypes()
        {
            _dbContext.SaveChanges();
        }
        public void AddMicroprocessorType(MicroprocessorTypes microprocessorTypes)
        {
            _dbContext.MicroprocessorTypes.Add(microprocessorTypes);
            _dbContext.SaveChanges();
        }

        public void RemoveMicroprocessorType(MicroprocessorTypes microprocessorTypes)
        {
            _dbContext.MicroprocessorTypes.Remove(microprocessorTypes);
            _dbContext.SaveChanges();
        }
    }
}
