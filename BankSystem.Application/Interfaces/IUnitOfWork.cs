// Este ya está en Domain/Interfaces/IUnitOfWork.cs
// No necesitas duplicarlo, solo usa el del Domain

// Pero si quieres agregar interfaces específicas del Application layer:

namespace BankSystem.Application.Interfaces;

// Ejemplo de interface para servicios adicionales si los necesitas después
public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, string customerName);
    Task SendTransactionNotificationAsync(string email, decimal amount, string type);
}

public interface IReportService
{
    Task<byte[]> GenerateAccountStatementAsync(Guid accountId, DateTime startDate, DateTime endDate);
}