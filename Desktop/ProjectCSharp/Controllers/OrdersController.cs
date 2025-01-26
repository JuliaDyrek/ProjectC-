using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektRestauracja.Data;
using ProjektRestauracja.Models;
using static NuGet.Packaging.PackagingConstants;

namespace ProjektRestauracja.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders.Include(o => o.Dish).Include(o => o.Reservation);

            var orders = await _context.Orders
                .Include(o => o.Dish)
                .Include(o => o.Reservation)
            .ToListAsync();
            foreach (var order in orders)
            {
                if (order.Dish != null)
                {
                    order.TotalPrice = order.Dish.Price * order.Quantity;
                }
            }
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Dish)
                .Include(o => o.Reservation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.Dish != null)
            {
                order.TotalPrice = order.Dish.Price * order.Quantity;
            }

            return View(order);
        }

        public IActionResult Create()
        {
            var model = new Order();

            ViewBag.Reservations = _context.Reservations.ToList();
            ViewBag.Dishes = _context.Dishes.ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationId, DishId, Quantity, TotalPrice")] Order order)
        {
            
            if (!ModelState.IsValid)
            {
                Console.WriteLine("DEBUG: ModelState contains error:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"- {error.ErrorMessage}");
                }

                ViewBag.Dishes = _context.Dishes.ToList();
                ViewBag.Reservations = _context.Reservations.ToList();

                return View(order);
            }

            if (order.ReservationId == 0)
            {
                Console.WriteLine("DEBUG: ReservationId is incorrect (0).");
                ModelState.AddModelError("ReservationId", "Please select a valid reservation.");
            }

            if (order.DishId == 0)
            {
                Console.WriteLine("DEBUG: DishId is incorrect (0).");
                ModelState.AddModelError("DishId", "Please select a valid dish.");
            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"DEBUG: {error.Key} - {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(order);
            }

            try
            {
                var dish = await _context.Dishes.FindAsync(order.DishId);
                if (dish != null)
                {
                    order.TotalPrice = dish.Price * order.Quantity;
                }
                else
                {
                    ModelState.AddModelError("DishId", "The selected dish does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: An error occurred while processing your request: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while processing your request.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Dishes = _context.Dishes.ToList();
                ViewBag.Reservations = _context.Reservations.ToList();

                return View(order);
            }

            try
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                Console.WriteLine("DEBUG: Order has been added to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: An error occurred while saving the order: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving the order.");
                ViewBag.Dishes = _context.Dishes.ToList();
                ViewBag.Reservations = _context.Reservations.ToList();
                return View(order);
            }

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Dish)
                .Include(o => o.Reservation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.TotalPrice = order.Dish.Price * order.Quantity;

            ViewBag.Dishes = _context.Dishes.Select(d => new
            {
                d.Id,
                d.Name,
                d.Price
            }).ToList();

            ViewBag.ReservationId = new SelectList(_context.Reservations, "Id", "Id", order.ReservationId);

            return View(order);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationId,DishId,Quantity")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            ViewData["DishId"] = new SelectList(_context.Dishes, "Id", "Id", order.DishId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", order.ReservationId);
            return View(order);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Dish)
                .Include(o => o.Reservation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.Dish != null)
            {
                order.TotalPrice = order.Dish.Price * order.Quantity;
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
