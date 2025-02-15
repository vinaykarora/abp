﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.TestApp.SecondContext;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Repositories
{
    public class Repository_Queryable_Tests : Repository_Queryable_Tests<AbpEntityFrameworkCoreTestModule>
    {
        private readonly IRepository<BookInSecondDbContext, Guid> _bookRepository;
        private readonly IRepository<PhoneInSecondDbContext> _phoneInSecondDbContextRepository;

        public Repository_Queryable_Tests()
        {
            _bookRepository = ServiceProvider.GetRequiredService<IRepository<BookInSecondDbContext, Guid>>();
            _phoneInSecondDbContextRepository = ServiceProvider.GetRequiredService<IRepository<PhoneInSecondDbContext>>();
        }

        [Fact]
        public async Task GetBookList()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                (await _bookRepository.AnyAsync()).ShouldBeTrue();
                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task GetPhoneInSecondDbContextList()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                (await _phoneInSecondDbContextRepository.AnyAsync()).ShouldBeTrue();
                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task EfCore_Include_Extension()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var person = await (await PersonRepository.GetDbSetAsync()).Include(p => p.Phones).SingleAsync(p => p.Id == TestDataBuilder.UserDouglasId);
                person.Name.ShouldBe("Douglas");
                person.Phones.Count.ShouldBe(2);
                return Task.CompletedTask;
            });
        }
    }
}
