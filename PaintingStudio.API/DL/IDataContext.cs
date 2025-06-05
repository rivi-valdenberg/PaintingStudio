using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DL
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Image> Images { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}