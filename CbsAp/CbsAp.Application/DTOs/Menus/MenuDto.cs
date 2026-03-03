using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Menus
{
    public class MenuDto
    {
        public string Label { get; set; }
        public string? Icon { get; set; }
        public List<string>? RouterLink { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<MenuItemDto>?  items { get; set; } = new List<MenuItemDto>();
    }
}
