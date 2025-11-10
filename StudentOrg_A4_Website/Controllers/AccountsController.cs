namespace StudentOrg_A4_Website.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentOrg_A4_Website.Models;

public class AccountsController : Controller
{
    private readonly StudentOrgContext _context;

    public AccountsController(StudentOrgContext context) 
    {
        _context = context;
    }

    public IActionResult Data()
    { 
        var accounts = _context.Accounts.ToList();
        return View(accounts);
    }
}
