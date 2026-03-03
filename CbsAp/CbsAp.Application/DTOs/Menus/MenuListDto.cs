using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Menus
{
    public class MenuListDto
    {
        public List<MenuDto> items { get; set; } = new List<MenuDto>();
    }
}
