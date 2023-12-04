using Microsoft.EntityFrameworkCore;
using PeopleManager.Core;
using PeopleManager.Dto.Filters;
using PeopleManager.Dto.Requests;
using PeopleManager.Dto.Results;
using PeopleManager.Model;
using PeopleManager.Services.Extensions;
using Vives.Services.Model;
using Vives.Services.Model.Extensions;

namespace PeopleManager.Services
{
    public class VehicleService
    {
        private readonly PeopleManagerDbContext _dbContext;

        public VehicleService(PeopleManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Find
        public async Task<PagedServiceResult<VehicleResult, VehicleFilter>> FindAsync(Paging paging, VehicleFilter filter)
        {
            var totalCount = await _dbContext.Vehicles
                .CountAsync();

            var vehicles = await _dbContext.Vehicles
                .ProjectToResults()
                .AddPaging(paging)
                .ToListAsync();

            return new PagedServiceResult<VehicleResult, VehicleFilter>
            {
                Data = vehicles,
                Paging = paging,
                TotalCount = totalCount,
                Filter = filter
            };
        }

        //Get by id
        public async Task<VehicleResult?> GetAsync(int id)
        {
            return await _dbContext.Vehicles
                .ProjectToResults()
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        //Create
        public async Task<ServiceResult<VehicleResult?>> CreateAsync(VehicleRequest request)
        {
            var vehicle = new Vehicle
            {
                LicensePlate = request.LicensePlate,
                Brand = request.Brand,
                Type = request.Type,
                ResponsiblePersonId = request.ResponsiblePersonId
            };

            _dbContext.Add(vehicle);
            await _dbContext.SaveChangesAsync();

            var vehicleResult = await GetAsync(vehicle.Id);
            
            return new ServiceResult<VehicleResult?>(vehicleResult);
        }

        //Update
        public async Task<ServiceResult<VehicleResult?>> UpdateAsync(int id, VehicleRequest vehicle)
        {
            var dbVehicle = await _dbContext.Vehicles.FindAsync(id);
            if (dbVehicle is null)
            {
                return new ServiceResult<VehicleResult?>().NotFound("vehicle");
            }

            dbVehicle.LicensePlate = vehicle.LicensePlate;
            dbVehicle.Brand = vehicle.Brand;
            dbVehicle.Type = vehicle.Type;
            dbVehicle.ResponsiblePersonId = vehicle.ResponsiblePersonId;

            await _dbContext.SaveChangesAsync();

            var vehicleResult = await GetAsync(id);
            return new ServiceResult<VehicleResult?>(vehicleResult);
        }

        //Delete
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(id);
            if (vehicle is null)
            {
                return new ServiceResult().NotFound("vehicle");
            }

            _dbContext.Vehicles.Remove(vehicle);

            await _dbContext.SaveChangesAsync();

            return new ServiceResult();
        }
    }
}
