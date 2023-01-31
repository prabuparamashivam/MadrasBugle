using System;
using System.Collections.Generic;
using System.Text;

namespace RandomBugleDB.Models.Comments
{
    public class SubComment :Comment
    {
        public int MainCommentId { get; set; }
    }
}
