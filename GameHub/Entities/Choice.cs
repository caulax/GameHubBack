using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Entities
{
    public class Choice
    {
        [Key]
        public Guid ChoiceId { get; set; }

        public int Id { get; set; }

        public Guid ChooserId { get; set; }

        public string Caption { get; set; }
    }
}
