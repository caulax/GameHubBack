using System.Collections.Generic;

namespace GameHub.Models.ParseModels
{
    public class ChooserModel
    {
        public IEnumerable<ChoiceModel> Choices { get; set; }

        public IEnumerable<ChoiceMatrixModel> ChoiceMatrix { get; set; }
    }
}
