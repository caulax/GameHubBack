using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHub.Models.ViewModels
{
    public class CreateGameRequestViewModel
    {
        /// <summary>
        /// Название игры
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор сценария
        /// </summary>
        public Guid ScenarioId { get; set; }
    }
}
