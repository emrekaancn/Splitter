using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Split.Data;
using Split.Services;
using Split.Models;
using System.Collections.Generic;
using System.Linq;

namespace Split.Controllers;

[ApiController]
[Route("api/split")]
public class SplitController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly SplitSettlementService _settlementService;

    public SplitController(
        AppDbContext context,
        SplitSettlementService settlementService)
    {
        _context = context;
        _settlementService = settlementService;
    }

    // GET: api/split/calculate
[HttpGet("calculate")]
public IActionResult Calculate()
{
    var users = _context.Users.ToList();

    var expenses = _context.Expenses
        .Include(e => e.User)
        .Where(e => e.User != null) // 👈 NULL OLANLARI ELE
        .ToList();

    if (users.Count == 0 || expenses.Count == 0)
    {
        return Ok(new { message = "Hesaplama için yeterli veri yok" });
    }

    var result = _settlementService.Calculate(users, expenses);
    return Ok(result);
}   

    // GET: api/split/calculate-transfers
    [HttpGet("calculate-transfers")]
    public IActionResult CalculateTransfers()
    {
        var users = _context.Users.ToList();
        var expenses = _context.Expenses.Include(e => e.User).ToList(); // User'ı dahil ediyoruz

        var transfers = _settlementService.CalculateTransfersWithNames(users, expenses);

        return Ok(transfers);
    }

    // POST: api/split/users
    [HttpPost("users")]
    public IActionResult AddUser([FromBody] User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return CreatedAtAction(nameof(AddUser), new { id = user.Id }, user);
    }

    [HttpPost("expenses")]
public IActionResult AddExpense([FromBody] Expense expense)
{
    var user = _context.Users.Find(expense.UserId);
    if (user == null)
        return BadRequest("User bulunamadı");

    expense.User = user; // 👈 ZORUNLU USER BURADA BAĞLANIYOR

    _context.Expenses.Add(expense);
    _context.SaveChanges();

    return CreatedAtAction(nameof(AddExpense), new { id = expense.Id }, expense);
}
    
    [HttpGet("settlements")]
public IActionResult GetSettlements()
{
    var users = _context.Users.ToList();
    var expenses = _context.Expenses.ToList();
    
    var result = _settlementService.CalculateTransfersWithNames(users, expenses);
    
    return Ok(result);
}
}