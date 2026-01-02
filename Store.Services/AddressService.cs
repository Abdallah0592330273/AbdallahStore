using Store.Core.Entities;
using Store.Core.Interfaces;
using Store.Core.Specifications;

namespace Store.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Address?> GetUserAddressAsync(string email)
        {
            var spec = new UserWithAddressSpecification(email);
            var user = await _unitOfWork.Repository<ApplicationUser>().GetEntityWithSpec(spec);
            return user?.Address;
        }

        public async Task<Address?> UpdateUserAddressAsync(string email, Address address)
        {
            var spec = new UserWithAddressSpecification(email);
            var user = await _unitOfWork.Repository<ApplicationUser>().GetEntityWithSpec(spec);

            if (user == null) return null;

            user.Address = address;
            _unitOfWork.Repository<ApplicationUser>().Update(user);
            
            var result = await _unitOfWork.Complete();
            return result > 0 ? user.Address : null;
        }
    }

    public class UserWithAddressSpecification : BaseSpecification<ApplicationUser>
    {
        public UserWithAddressSpecification(string email) : base(x => x.Email == email)
        {
            AddInclude(x => x.Address!);
        }
    }
}
