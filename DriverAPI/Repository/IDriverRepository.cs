using DriverAPI.Models;

namespace DriverAPI.Repository
{
    public interface IDriverRepository
    {
        List<Driver> GetAllDrivers();
        int InsertDriver(Driver driver);
        int DeleteDriver(int Id);
        int UpdateDriver(Driver driver);
        Driver GetDriverByID(int Id);
    }
}
