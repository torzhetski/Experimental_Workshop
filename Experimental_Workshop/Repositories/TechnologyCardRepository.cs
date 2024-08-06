using Experimental_Workshop.MVVM.Model;
using Microsoft.EntityFrameworkCore;

namespace Experimental_Workshop.Repositories
{
    public class TechnologyCardRepository
    {

        private readonly ApplicationContext _dbContext;

        public TechnologyCardRepository(ApplicationContext context)
        {
            _dbContext = context;
        }
        public List<TechnologyCard> GetAllTechnologyCards()
        {
            return _dbContext.TechnologyCards
                .Include(w => w.Materials)
                .Include(w => w.Equipment)
                .Include(w => w.Workers)
                .Include(w => w.MicroprocessorType)
                .ToList();
        }

        public void SaveAllTechnologyCards()
        {
            _dbContext.SaveChanges();
        }
        public void AddTechnologyCard(TechnologyCard TechnologyCard)
        {
            _dbContext.TechnologyCards.Add(TechnologyCard);
            _dbContext.SaveChanges();
        }

        public void RemoveTechnologyCard(TechnologyCard technologyCard)
        {
            _dbContext.TechnologyCards.Remove(technologyCard);
            _dbContext.SaveChanges();
        }
        public List<TechnologyCard> GetExpluatationTechnologyCards()
        {
            //to see sql query string
            //string e = _dbContext.TechnologyCards
            //   .Where(w => w.IsInExpluatation == true)
            //   .ToQueryString();

            return _dbContext.TechnologyCards
                .Where(w => w.IsInExpluatation == true)
                .ToList();
        }
        public List<TechnologyCard> GetTechnologyCardsBetweenToDates(DateTime dateStart, DateTime dateEnd)
        {
            return _dbContext.TechnologyCards
                .Include(w => w.Materials)
                .Include(w => w.Equipment)
                .Include(w => w.Workers)
                .Include(w => w.MicroprocessorType)
                .Where(w => (w.DateOfStart >= dateStart && w.DateOfStart <= dateEnd) || (w.DateOfEnd >= dateStart && w.DateOfEnd <= dateEnd))
                .ToList();
        }
        public List<TechnologyCard> GetTechnologyCardsWithGivenParametrs(int power, int frequency)
        {
          //to see sql query string
          //string e = _dbContext.TechnologyCards
          //      .Where(w => (w.Frequency == frequency) || (w.Power == power))
          //      .ToQueryString();

            return _dbContext.TechnologyCards
                .Where(w => (w.Frequency == frequency) || (w.Power == power))
                .ToList();
        }
    }
}
