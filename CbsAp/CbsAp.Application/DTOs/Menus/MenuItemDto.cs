using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Menus
{
    public class MenuItemDto
    {
        public string Label { get; set; }
        public string? Icon { get; set; }
        public List<string>? RouterLink { get; set; }
    }
}