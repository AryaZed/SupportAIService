namespace SupportAI.Shared.DTOs;

public record UserRegistrationDto(string FullName, string Email, string Password, Guid TenantId);
