using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaaradhiPay.Data
{
    public class UPIAddress
    {
        [Key]
        public int UPIAddressId { get; set; } 
        public string Address { get; set; } 
        public string UpiUserName { get; set; } 
        public bool IsActive { get; set; } = true; 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


        [Required]
        public string UserId { get; set; } 

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } 

        

    }
}
