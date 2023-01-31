using System;
using System.Collections.Generic;
using System.Text;

namespace RandomBugleDB.Models.Comments
{
    public class MainComment :Comment 
    {
        public List<SubComment> SubComments { get; set; }
    }
}
