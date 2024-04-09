namespace INTEX.Models;

public interface IRepo
{
    public IQueryable<AspNetRole> AspNetRoles { get;  }
}