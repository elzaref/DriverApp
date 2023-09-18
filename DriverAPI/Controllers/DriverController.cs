using DriverAPI.Models;
using DriverAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace DriverAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class DriverController : Controller
    {
        private readonly IDriverRepository _driverRepository;
        public DriverController(IDriverRepository driverRepository)
        {
                _driverRepository= driverRepository;
        }
        [HttpGet]
        public ActionResult<List<Driver>> GetAllAlphabetized()
        {
            try
            {
                var response = _driverRepository.GetAllDrivers();
                return Ok(new ApiResponse<List<Driver>>() { Success = true, Data = response });
            }
            catch (Exception exception)
            {
                return Ok(new ApiResponse<List<Driver>>() { Success = false, Data = null,Message=exception.Message + exception.InnerException?.Message });
            }
        }
        //name alphabetized 
        [HttpGet]
        public ActionResult<List<string>> GetAllNamesAlphabetized()
        {
            try
            {
                var all = _driverRepository.GetAllDrivers();
                var res = NamesAlphabetized(all);
                return Ok(new ApiResponse<List<string>>() { Success = true, Data = res });
            }
            catch (Exception exception)
            {
                return Ok(new ApiResponse<List<string>>() { Success = false, Data = null, Message = exception.Message + exception.InnerException?.Message });
            }
        }
        [HttpPost]
        public ActionResult<ApiResponse<int>> Create(Driver input)
        {
            try
            {
                var isValid = Validate(input);
                if (!string.IsNullOrWhiteSpace(isValid))
                {
                    return Ok(new ApiResponse<int>() { Success = false, Data = input.Id, Message = isValid });
                }
                var response = _driverRepository.InsertDriver(input);
                return Ok(new ApiResponse<int>() { Success = true, Data = response  });

            }
            catch (Exception exception)
            {
                return Ok(new ApiResponse<int>() { Success = false, Data = 0, Message = exception.Message + exception.InnerException?.Message });
            }
        }
        [HttpGet]
        public ActionResult<Driver> GetById(int id)
        {
            try
            {
                var response = _driverRepository.GetDriverByID(id);
                return Ok(new ApiResponse<Driver>() { Success = true, Data = response });
            }
            catch (System.Exception ex)
            {
                return Ok(new ApiResponse<Driver>() { Success = false, Data = null, Message = ex.Message + ex.InnerException?.Message });
            }
        }


        [HttpPut]
        [Route("api/[controller]")]
        public ActionResult<ApiResponse<int>> Update(Driver input)
        {
            try
            {
                var old= _driverRepository.GetDriverByID(input.Id);
                if(old==null)
                    return Ok(new ApiResponse<int>() { Success = false, Data = input.Id, Message = "No Driver with this Id" });

                var isValid = Validate(input);
                if (!string.IsNullOrWhiteSpace(isValid))
                {
                    return Ok(new ApiResponse<int>() { Success = false, Data = input.Id, Message = isValid });
                }
                int id = _driverRepository.UpdateDriver(input);
                return Ok(new ApiResponse<int>() { Success = true, Data = input.Id });

            }
            catch (System.Exception ex)
            {
                return Ok(new ApiResponse<int>() { Success = false, Data = input.Id, Message = ex.Message + ex.InnerException?.Message });
            }
        }


        [HttpDelete]
        [Route("api/[controller]/[Action]/{id}")]
        public ActionResult<ApiResponse<int>> Delete(int id)
        {
            try
            {
                var old = _driverRepository.GetDriverByID(id);
                if (old == null)
                    return Ok(new ApiResponse<int>() { Success = false, Data = id, Message = "No Driver with this Id" });
                _driverRepository.DeleteDriver(id);
                return Ok(new ApiResponse<int>() { Success = true, Data = id });
            }
            catch (System.Exception ex)
            {
                return Ok(new ApiResponse<int>() { Success = false, Data = id, Message = ex.Message + ex.InnerException?.Message });
            }
        }
        [HttpPost]
        public ActionResult<ApiResponse<bool>> InsertSomeData()
        {
            try
            {
                List<Driver> lst = new List<Driver>()
                {
                    new Driver(){FirstName="Oliver",LastName="Johnson",Email="Oliver@gmail.com",PhoneNumber="+201013816994"},
                    new Driver(){FirstName="Tom",LastName="Hanks",Email="Tom@gmail.com",PhoneNumber="+201013816994"},
                    new Driver(){FirstName="Morgan",LastName="Freeman",Email="Morgan@gmail.com",PhoneNumber="+201013816994"},
                    new Driver(){FirstName="teto",LastName="tat",Email="teto@gmail.com",PhoneNumber="+201013816994"},
                    new Driver(){FirstName="meo",LastName="cat",Email="meo@gmail.com",PhoneNumber="+201013816994"},
                    new Driver(){FirstName="hoho",LastName="dog",Email="hoho@gmail.com",PhoneNumber="+201013816994"},
                    new Driver(){FirstName="zekzek",LastName="rat",Email="zekzek@gmail.com",PhoneNumber="+201013816994"},
                    new Driver(){FirstName="pol",LastName="hersh",Email="pol@gmail.com",PhoneNumber="+201013816994"},
                    new Driver(){FirstName="rampo",LastName="gam",Email="rampo@gmail.com",PhoneNumber="+201013816994"},
                    new Driver(){FirstName="Robert",LastName="Downey",Email="Robert@gmail.com",PhoneNumber="+201013816994"},
                    new Driver(){FirstName="mostafa",LastName="elzaref",Email="mostafaelzaref3@gmail.com",PhoneNumber="+201013816994"}
                };
                foreach(Driver driver in lst)
                {
                    var response = _driverRepository.InsertDriver(driver);
                }
                return Ok(new ApiResponse<bool>() { Success = true, Data = true });

            }
            catch (Exception exception)
            {
                return Ok(new ApiResponse<bool>() { Success = false, Data =true, Message = exception.Message + exception.InnerException?.Message });
            }
        }
        private List<string> NamesAlphabetized(List<Driver> drivers)
        {
            List<string> names = new List<string>();
            foreach(var driver in drivers)
            {
                string fName = driver.FirstName.ToLower();
                string lName = driver.LastName.ToLower();

                var fullName = $"{String.Concat(fName.OrderBy(c => c))} {String.Concat(lName.OrderBy(c => c))}";
                names.Add(fullName);
            }
            return names;
        }
        private string Validate(Driver driver)
        {
            // I used manual validation but we can use data annotation or action filter to validate these data
            if (!Regex.Match(driver.FirstName, @"^([a-zA-Z]{3,30})$").Success)
            {
                return "Invalid First Name";
            }
            if (!Regex.Match(driver.LastName, @"^([a-zA-Z]{3,30})$").Success)
            {
                return "Invalid Last Name";
            }
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            //var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (!Regex.Match(driver.Email, pattern).Success)
            {
                return "Invalid Email";
            }

            //+201013816994
            //12 digit in addition "+"
            if (!Regex.Match(driver.PhoneNumber, @"^(\+[0-9]{12})$").Success)
            {
                return "Invalid PhoneNumber";
            }
            return string.Empty;
        }


    }
}
