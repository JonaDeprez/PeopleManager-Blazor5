using Microsoft.EntityFrameworkCore;
using PeopleManager.Core;
using PeopleManager.Dto.Filters;
using PeopleManager.Dto.Requests;
using PeopleManager.Dto.Results;
using PeopleManager.Model;
using PeopleManager.Services.Extensions;
using PeopleManager.Services.Extensions.Filters;
using Vives.Services.Model;
using Vives.Services.Model.Extensions;

namespace PeopleManager.Services
{
    public class PersonService
    {
        private readonly PeopleManagerDbContext _dbContext;

        public PersonService(PeopleManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Find
        public async Task<PagedServiceResult<PersonResult, PersonFilter>> FindAsync(Paging paging, PersonFilter? filter)
        {
            var totalCount = await _dbContext.People
                .ApplyFilter(filter)
                .CountAsync();

            var people = await _dbContext.People
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .ApplyFilter(filter)
                .ProjectToResults()
                .AddPaging(paging)
                .ToListAsync();

            return new PagedServiceResult<PersonResult, PersonFilter>
            {
                Data = people, 
                Paging = paging, 
                TotalCount = totalCount,
                Filter = filter
            };
        }

        //Get by id
        public async Task<PersonResult?> GetAsync(int id)
        {
            return await _dbContext.People
                .ProjectToResults()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        

        //Create
        public async Task<ServiceResult<PersonResult?>> CreateAsync(PersonRequest request)
        {
            if (request.FirstName == "Droid")
            {
                return new ServiceResult<PersonResult?>
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "NoDroids",
                            Title = "We don't serve your kind here!",
                            Type = ServiceMessageType.Error,
                            PropertyName = nameof(request.FirstName)
                        }
                    }
                };
            }

            var person = new Person
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Description = request.Description
            };
            _dbContext.Add(person);

            await _dbContext.SaveChangesAsync();

            var personResult = await GetAsync(person.Id);

            return new ServiceResult<PersonResult?>(personResult);
        }

        //Update
        public async Task<ServiceResult<PersonResult?>> UpdateAsync(int id, PersonRequest person)
        {
            var dbPerson = await _dbContext.People.FindAsync(id);
            if (dbPerson is null)
            {
                return new ServiceResult<PersonResult?>().NotFound("person");
            }

            dbPerson.FirstName = person.FirstName;
            dbPerson.LastName = person.LastName;
            dbPerson.Email = person.Email;
            dbPerson.Description = person.Description;

            await _dbContext.SaveChangesAsync();

            var personResult = await GetAsync(id);
            return new ServiceResult<PersonResult?>(personResult);
        }

        //Delete
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var person = await _dbContext.People.FindAsync(id);
            if (person is null)
            {
                return new ServiceResult().NotFound("person");
            }

            _dbContext.People.Remove(person);

            await _dbContext.SaveChangesAsync();

            return new ServiceResult();
        }
    }
}
