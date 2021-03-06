﻿namespace HealthAndCareHospital.Web.Areas.Admin.Controllers
{
    using HealthAndCareHospital.Services;
    using HealthAndCareHospital.Services.Models.Admin;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class DepartmentController : BaseAdminController
    {
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        public IActionResult All()
        {
            var departments = this.departmentService.All();

            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateServiceModel model)
        {
            if (ModelState.IsValid)
            {
                await this.departmentService
                    .CreateAsync(model.Name, model.Description, model.ImageURL);

                return RedirectToAction(nameof(All));
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var department = await this.departmentService
                .DepartmentExists(id);

            if (!department)
            {
                return NotFound();
            }

            var departmentDetails = await this.departmentService
                .Details(id);

            return View(departmentDetails);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var department = await this.departmentService
                .DepartmentExists(id);

            if (!department)
            {
                return NotFound();
            }

            var departmentDelete = await this.departmentService
                .Details(id);

            return View(departmentDelete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await this.departmentService
                .DepartmentExists(id);

            if (!department)
            {
                return NotFound();
            }

            var success = await this.departmentService
                .Delete(id);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await this.departmentService
                .DepartmentExists(id);

            if (!department)
            {
                return NotFound();
            }

            var departmentEdit = await this.departmentService
                .Details(id);

            return View(departmentEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentCreateServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var department = await this.departmentService
                .DepartmentExists(model.Id);

            if (!department)
            {
                return NotFound();
            }

            var success = await this.departmentService
                .Edit(model.Id,
                model.Name, model.Description, model.ImageURL);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }
    }
}