﻿using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Infrastructure.Data
{
    public abstract class AppDbContext : DbContext
    {
        private readonly IMediatorHandler _mediatorHandler = null;

        protected AppDbContext() : base() { }

        protected AppDbContext(DbContextOptions options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();

            if (result > 0)
            {
                PublishEntityEvents();
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                PublishEntityEvents();
            }

            return result;
        }

        /// <summary>
        /// Source: https://github.com/ardalis/CleanArchitecture/blob/master/src/CleanArchitecture.Infrastructure/Data/AppDbContext.cs
        /// </summary>
        private void PublishEntityEvents()
        {
            var aggregators = ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .Where(e => e.GetUncommittedEvents().Count > 0);

            foreach (var aggregator in aggregators)
            {
                var @events = aggregator.GetUncommittedEvents();

                foreach (var @event in @events)
                {
                    _mediatorHandler.PublishEvent(@event);
                }

                aggregator.ClearUncommittedEvents();
            }
        }
    }

}
