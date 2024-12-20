using Microsoft.AspNetCore.Components;

namespace VaaradhiPay.DTOs
{
    public class TabItemDTO
    {
        public int TabId { get; set; }
        public string Name { get; set; } = string.Empty;
        public RenderFragment Content { get; set; }
    }
}
