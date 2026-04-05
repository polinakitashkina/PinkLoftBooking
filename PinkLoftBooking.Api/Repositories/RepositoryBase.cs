using PinkLoftBooking.Api.Data;

namespace PinkLoftBooking.Api.Repositories;

/// <summary>Общая база репозиториев (доступ к контексту БД).</summary>
public abstract class RepositoryBase
{
    protected readonly AppDbContext Context;

    protected RepositoryBase(AppDbContext context) => Context = context;
}
