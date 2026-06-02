namespace EmployeeManagement.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/departments")]
[ApiVersion("1.0")]
public class DepartmentController(IDepartmentService _departmentService, IValidator<DepartmentCreateRequestDTO> createValidator, IValidator<DepartmentUpdateRequestDTO> updateValidator) : Controller
{
    #region API Endpoint Methods
    [HttpGet]
    public async Task<IActionResult> GetAllDepartmentsAsync(string? id = null, bool isActive = true, int page = 1, int limit = 10)
    {
        Guid? departmentId = null;

        if (!string.IsNullOrWhiteSpace(id))
        {
            if (!Guid.TryParse(id, out var parsedId))
                return BadRequest($"Invalid Guid format for given id: {id}");

            departmentId = parsedId;
        }

        var result = await _departmentService.GetAllDepartmentsAsync(departmentId, isActive, page, limit);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreateRequestDTO dto)
    {
        var validationRes = await createValidator.ValidateAsync(dto);
        if (!validationRes.IsValid)
            throw new ValidationException(validationRes.Errors);

        var result = await _departmentService.CreateNewDepartmentRecord(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(string id, [FromBody] DepartmentUpdateRequestDTO dto)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return BadRequest($"Invalid Guid format for given id: {id}");

        var validationRes = await updateValidator.ValidateAsync(dto);
        if (!validationRes.IsValid)
            throw new ValidationException(validationRes.Errors);

        var result = await _departmentService.UpdateDepartmentRecord(parsedId, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return BadRequest($"Invalid Guid format for given id: {id}");

        var result = await _departmentService.DeleteDepartmentRecord(parsedId);
        return Ok(result);
    }
    #endregion
}
