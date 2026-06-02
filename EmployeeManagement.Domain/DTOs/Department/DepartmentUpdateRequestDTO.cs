namespace EmployeeManagement.Domain.DTOs.Department;

public record DepartmentUpdateRequestDTO(string DepartmentName, bool? IsActive);