namespace INTEX.Models;

public class EfRepo : IRepo
{
    
    private readonly ApplicationDbContext _context;


    public EfRepo(ApplicationDbContext context)
    {
        _context = context;
    }


    public IQueryable<AspNetRole> AspNetRoles => _context.AspNetRoles;
}