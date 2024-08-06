using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Experimental_Workshop.ApplicationResourses.ValidationRules
{
    public class ModelsValidation
    {
        public static bool ValidateEquipment(TechnologyCard card, MainRepositoryService mainRepository)
        {
            var TechnologyCards = mainRepository.TechnologyCardRepository.GetAllTechnologyCards();
            bool flag = false;
            if (card != null)
            {
                foreach (var e in TechnologyCards)
                    foreach (var equip in card.Equipment)
                        if (equip != null && e.Equipment.Contains(equip))
                            if ((card.DateOfStart > e.DateOfStart && card.DateOfStart < e.DateOfEnd) || (card.DateOfEnd > e.DateOfStart && card.DateOfEnd < e.DateOfEnd)
                                && (e.DateOfStart > card.DateOfStart && e.DateOfStart < card.DateOfEnd) || (e.DateOfEnd > card.DateOfStart && e.DateOfEnd < card.DateOfEnd))
                            {
                                MessageBox.Show($"{equip.Name} in {card.Name} is busy on selected dates");
                                flag = true;
                                break;
                            }
            }
            return flag;
        }
        public static bool ValidateEquipment(Equipment equip,TechnologyCard card)
        {
            bool flag = false;
            if (equip != null && card != null)
            {
                foreach(var e in equip.TechnologyCards)
                    if ((card.DateOfStart > e.DateOfStart && card.DateOfStart < e.DateOfEnd) || (card.DateOfEnd > e.DateOfStart && card.DateOfEnd < e.DateOfEnd)
                        && (e.DateOfStart > card.DateOfStart && e.DateOfStart < card.DateOfEnd) || (e.DateOfEnd > card.DateOfStart && e.DateOfEnd < card.DateOfEnd))
                    {
                        MessageBox.Show($"You cant add {card.Name} to {equip.Name} becouse this equip is busy on that dates");
                        flag = true; 
                        break;
                    }

            }
            return flag;
        }

    }
}
