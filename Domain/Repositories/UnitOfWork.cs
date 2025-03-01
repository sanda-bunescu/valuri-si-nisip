using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUnitOfWork
    {
        AppDbContext Context { get; }
        IAccountRepository AccountRepository { get; }
        IBeachRepository BeachRepository { get; }
        IReviewRepository ReviewRepository { get; }
        Task<bool> SaveChangesAsync();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public AppDbContext Context => _context;
        public IAccountRepository AccountRepository { get; }
        public IBeachRepository BeachRepository { get; }
        public IReviewRepository ReviewRepository { get; }

        public UnitOfWork(
            AppDbContext context, 
            IAccountRepository accountRepository, 
            IBeachRepository beachRepository,
            IReviewRepository reviewRepository
            )
        {
            _context = context;
            this.AccountRepository = accountRepository;
            this.BeachRepository = beachRepository;
            this.ReviewRepository = reviewRepository;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}