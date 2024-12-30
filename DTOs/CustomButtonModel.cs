namespace VaaradhiPay.DTOs
{
    public class CustomButtonModel
    {
        public string DisplayText { get; set; } = string.Empty; // Button text
        public string NavigationLink { get; set; } = string.Empty; // Navigation link
        public string Icon { get; set; } = string.Empty; // Icon associated with the button
        public bool IsButtonDisabled { get; set; } = false; // Flag to disable navigation link
        public bool? IsButtonActive { get; set; }
    }
}
