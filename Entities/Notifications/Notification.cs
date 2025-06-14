using Entities.Common;
using Entities.Estates;
using Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Notifications
{
    public class Notification :BaseEntity<Guid>
    {
 

        /// <summary> متن نوتیفیکیشن </summary>
        public string Message { get; set; }

        /// <summary> تاریخ و زمان ارسال نوتیفیکیشن </summary>
        public DateTime SentAt { get; set; }

        /// <summary> تاریخ و زمان خوانده شدن نوتیفیکیشن (nullable) </summary>
        public DateTime? ReadAt { get; set; }

        /// <summary> وضعیت خوانده شدن نوتیفیکیشن </summary>
        public bool IsRead { get; set; }

        /// <summary> شناسه یکتای شخص دریافت‌کننده نوتیفیکیشن </summary>
        public Guid RecipientUserId { get; set; }

        /// <summary> شناسه یکتای ملک مربوط به نوتیفیکیشن </summary>
        public Guid RelatedEstateId { get; set; }

        public SmartRealEstatePricing RelatedEstate { get; set; }




        public User RecipientUser  { get; set; }

    }
}
