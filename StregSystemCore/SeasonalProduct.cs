using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    public class SeasonalProduct : BaseProduct {
        IDateProvider _dateProvider { get; set; }
        private bool _active;
        public DateTime SeasonStartDate {  get; set; }
        
        public DateTime SeasonEndDate {  get; set; }

        public override bool Active { 
            get {
                DateTime now = _dateProvider.GetCurrentDateTime();
                return _active && now >= SeasonStartDate && now <= SeasonEndDate;
            } 
            set {
                _active = value;
            } 
        }

        public SeasonalProduct(int id, string name, decimal price, bool active, bool canBeBoughtOnCredit, DateTime seasonStartDate, DateTime seasonEndDate)
            : this(id, name, price, active, canBeBoughtOnCredit, seasonStartDate, seasonEndDate, new SystemDateProvider()) 
        { }

        public SeasonalProduct(int id, string name, decimal price, bool active, bool canBeBoughtOnCredit, DateTime seasonStartDate, DateTime seasonEndDate, IDateProvider dateProvider)
            : base(id, name, price, canBeBoughtOnCredit) {
            SeasonStartDate = seasonStartDate;
            SeasonEndDate = seasonEndDate;
            Active = active;
            _dateProvider = dateProvider;
        }
    }
}
