using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Shared.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
