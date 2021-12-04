using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    public class SeasonalProduct : BaseProduct {
        public DateTime SeasonStartDate {  get; set; }
        
        public DateTime SeasonEndDate {  get; set; }

        private bool _active;

        //TODO: Det her giver ikke helt mening og det virker i det hele taget som to forskellige værdier
        //hvor den ene er om vi er inde for sæsonen og den anden er om den aktivt bliver solgt men hvis
        //den bliver solgt så burde den også være inde i sæsonen
        public override bool Active { 
            get {
                DateTime now = DateTime.Now;
                return _active && now >= SeasonStartDate && now <= SeasonEndDate;
            } 
            set {
                _active = value;
            } 
        }

        public SeasonalProduct(int id, string name, decimal price, bool active, bool canBeBoughtOnCredit, DateTime seasonStartDate, DateTime seasonEndDate)
            : base(id, name, price, canBeBoughtOnCredit) {
            SeasonStartDate = seasonStartDate;
            SeasonEndDate = seasonEndDate;
            Active = active;
        }
    }
}
