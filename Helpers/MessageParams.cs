using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    public class MessageParams
    {
        private const int Max_Page_Size = 50;
        public int PageNumber { get; set; }
        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            // no more than 50 if requested 60 it is 50 if 10 its less so 10
            set { pageSize = (value > Max_Page_Size) ? Max_Page_Size : value; }
        }

        public int UserId { get; set; }
        public string MessageContainer { get; set; } = "Unread";

    }
}
