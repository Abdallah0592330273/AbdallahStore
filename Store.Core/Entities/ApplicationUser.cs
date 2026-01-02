using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Store.Core.Entities
{
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        public ApplicationUser()
        {
            CreatedDate = DateTimeOffset.UtcNow;
            IsDeleted = false;
        }

        public string DisplayName { get; set; } = string.Empty;
        public string? PictureUrl { get; set; }
        public string? PicturePublicId { get; set; }
        
        [InverseProperty("ApplicationUser")]
        public Address? Address { get; set; }
        
        public Wishlist? Wishlist { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
