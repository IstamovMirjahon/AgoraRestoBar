using Agora.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.Domain.Entities
{
    public class MenuTranslation : BaseParams
    {
        public Guid MenuId { get; set; }
        public Menu Menu { get; set; } = default!;

        public string LanguageCode { get; set; } = default!; // 'uz', 'ru', 'en'
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
