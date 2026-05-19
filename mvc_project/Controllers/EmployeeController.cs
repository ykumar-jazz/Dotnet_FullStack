using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using mvc_project.EMS.Application.Services;
using mvc_project.EMS.Domain.Entities;
using mvc_project.EMS.Web.Hubs;
using X.PagedList.Extensions;

namespace mvc_project.Controllers;
//[Authorize(Roles ="Admin")]
public class EmployeeController:Controller
{
    public readonly EmployeeService _service;
    private readonly IHubContext<NotificationHub> _hubContext;
    public EmployeeController(EmployeeService service,IHubContext<NotificationHub> hubContext)
    {
        _service=service;
        _hubContext=hubContext;
    }
    public async Task<IActionResult> Index(
        string search,
        int page = 1)
    {
        var employees = await _service.GetAllAsync();

        if(!string.IsNullOrEmpty(search))
        {
            employees = employees
                .Where(x =>
                    x.Name.Contains(search,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        int pageSize = 2;

        return View(
            employees.ToPagedList(page,
                                  pageSize));
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        Employee employee,
        IFormFile file)
    {
        if(file != null)
        {
            string folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/uploads");

            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(
                folder,
                file.FileName);

            using FileStream stream =
                new(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            employee.ImagePath = file.FileName;
        }

        await _service.AddAsync(employee);
        await _hubContext.Clients.All.SendAsync("ReceiveNotification","New Employee Added");

        return RedirectToAction(nameof(Index));
    }
}
