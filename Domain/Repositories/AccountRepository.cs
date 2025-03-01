using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace Domain.Repositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<Account> GetByEmailAsync(string email);
    }
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        private readonly AppDbContext _context;
        public AccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            var result = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            return result;
        }
    }
}