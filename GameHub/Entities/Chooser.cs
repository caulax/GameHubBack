using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Entities
{
    public class Chooser
    {
        public Guid Id { get; set; }

        public Guid SceneId { get; set; }

        public virtual IEnumerable<Choice> Choices { get; set; }
        
        public virtual IEnumerable<MatrixVariant> ChoiceMatrix { get; set; }
    }
}
