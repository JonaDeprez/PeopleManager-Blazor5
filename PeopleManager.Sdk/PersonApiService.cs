using System.Net.Http.Json;
using PeopleManager.Dto.Filters;
using PeopleManager.Dto.Requests;
using PeopleManager.Dto.Results;
using PeopleManager.Sdk.Extensions;
using Vives.Services.Model;

namespace PeopleManager.Sdk
{
    public class PersonApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PersonApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PagedServiceResult<PersonResult, PersonFilter>> FindAsync(Paging? paging = null, PersonFilter? filter = null)
        {
            if (paging is null)
            {
                paging = new Paging { PageSize = 100 };
            }

            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            //var route = "/api/people";
            var route = $"/api/people?startIndex={paging.StartIndex}&pageSize={paging.PageSize}";
            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var people = await httpResponse.Content.ReadFromJsonAsync<PagedServiceResult<PersonResult, PersonFilter>>();

            return people ?? new PagedServiceResult<PersonResult, PersonFilter>{Paging = paging};
        }

        public async Task<PersonResult?> GetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = $"/api/people/{id}";
            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            return await httpResponse.Content.ReadFromJsonAsync<PersonResult>();
        }

        public async Task<ServiceResult<PersonResult?>> CreateAsync(PersonRequest person)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = "/api/people";
            var httpResponse = await httpClient.PostAsJsonAsync(route, person);

            httpResponse.EnsureSuccessStatusCode();

            var serviceResult = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<PersonResult?>>();
            if (serviceResult is null)
            {
                return new ServiceResult<PersonResult?>().ApiError();
            }
            return serviceResult;
        }

        public async Task<ServiceResult<PersonResult?>> UpdateAsync(int id, PersonRequest person)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = $"/api/people/{id}";
            var httpResponse = await httpClient.PutAsJsonAsync(route, person);

            httpResponse.EnsureSuccessStatusCode();

            var serviceResult = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<PersonResult?>>();
            if (serviceResult is null)
            {
                return new ServiceResult<PersonResult?>().ApiError();
            }
            return serviceResult;
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = $"/api/people/{id}";
            var httpResponse = await httpClient.DeleteAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var serviceResult = await httpResponse.Content.ReadFromJsonAsync<ServiceResult>();
            if (serviceResult is null)
            {
                return new ServiceResult().ApiError();
            }
            return serviceResult;
        }
    }
}
