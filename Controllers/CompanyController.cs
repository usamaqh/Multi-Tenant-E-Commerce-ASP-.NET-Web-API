using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;
using Multi_Tenant_E_Commerce_API.Helpers;
using Multi_Tenant_E_Commerce_API.Services.CompanyService;

namespace Multi_Tenant_E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ICompanyService _ICompanyService;

        public CompanyController(ICompanyService iCompanyService)
        {
            _ICompanyService = iCompanyService;
        }

        [HttpGet]
        [Route("get_all")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole)]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                List<CompanyResponse> result = await _ICompanyService.GetAllCompanies();

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("get_by_id/{companyId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole)]
        public async Task<IActionResult> GetCompanyById(Guid companyId)
        {
            try
            {
                CompanyResponse? result = await _ICompanyService.GetCompanyById(companyId);

                if (result == null)
                    return NotFound("No data present!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("get_by_name/{companyName}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole)]
        public async Task<IActionResult> GetCompanyByName(string companyName)
        {
            try
            {
                CompanyResponse? result = await _ICompanyService.GetCompanyByName(companyName);

                if (result == null)
                    return NotFound("No data present!");

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("get_by_adminid/{adminId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole)]
        public async Task<IActionResult> GetCompanyByAdminId(Guid adminId)
        {
            try
            {
                List<CompanyResponse> result = await _ICompanyService.GetCompanyByAdminId(adminId);

                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole)]
        public async Task<IActionResult> AddCompany([FromForm] CompanyRequest newCompanyRequest)
        {
            try
            {
                Guid userIdClaim = User.GetLoggedInUserId();

                if (userIdClaim == Guid.Empty)
                    return Unauthorized();

                CompanyResponse? result = await _ICompanyService.AddCompany(newCompanyRequest, userIdClaim);

                if (result == null)
                    return BadRequest("Could not add data!");

                return CreatedAtAction(nameof(AddCompany), result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("update/{companyId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole)]
        public async Task<IActionResult> UpdateCompany(Guid companyId, [FromForm] CompanyRequest updateCompanyRequest)
        {
            try
            {
                if (!await _ICompanyService.UpdateCompany(companyId, updateCompanyRequest))
                    return BadRequest("Could not update data!");

                return Ok("Data updated successfully!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


        [HttpDelete]
        [Route("delete/{companyId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole)]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            try
            {
                if (!await _ICompanyService.DeleteCompany(companyId))
                    return BadRequest("Could not delete data!");

                return Ok("Data deleted successfully!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("undelete/{companyId}")]
        [Authorize(Roles = UserRolesHelper.SuperAdminRole)]
        public async Task<IActionResult> UnDeleteCompany(Guid companyId)
        {
            try
            {

                if (!await _ICompanyService.UnDeleteCompany(companyId))
                    return BadRequest("Could not undelete data!");

                return Ok("Data undeleted successfully!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
