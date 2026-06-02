namespace EmployeeManagement.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/employees")]
[ApiVersion("1.0")]
public class EmployeeController(IEmployeeService _employeeService, IValidator<EmployeeCreateRequestDTO> createValidator, IValidator<EmployeeUpdateRequestDTO> updateValidator) : Controller
{
    #region API Endpoint Methods
    [HttpGet]
    public async Task<IActionResult> GetAllEmployeesAsync(string? id = null, bool isActive = true, int page = 1, int limit = 10)
    {
        Guid? employeeId = null;

        if (!string.IsNullOrWhiteSpace(id))
        {
            if (!Guid.TryParse(id, out var parsedId))
                return BadRequest($"Invalid Guid format for given id: {id}");

            employeeId = parsedId;
        }
        var result = await _employeeService.GetAllEmployeesAsync(employeeId, isActive, page, limit);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateRequestDTO dto)
    {
        var validationRes = await createValidator.ValidateAsync(dto);
        if (!validationRes.IsValid)
            throw new ValidationException(validationRes.Errors);

        var result = await _employeeService.CreateNewEmployeeRecord(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(string id, [FromBody] EmployeeUpdateRequestDTO dto)
    {
        var validationRes = await updateValidator.ValidateAsync(dto);
        if (!validationRes.IsValid)
            throw new ValidationException(validationRes.Errors);

        if (!Guid.TryParse(id, out var parsedId))
            return BadRequest($"Invalid Guid format for given id: {id}");

        var result = await _employeeService.UpdateEmployeeRecord(parsedId, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            return BadRequest($"Invalid Guid format for given id: {id}");

        var result = await _employeeService.DeleteEmployeeRecord(parsedId);
        return Ok(result);
    }
    #endregion
}
