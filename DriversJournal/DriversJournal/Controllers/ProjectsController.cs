using DriversJournal.Models;
using DriversJournal.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace DriversJournal.Controllers
{
    /// <summary>
    /// Controller for the Projects-views
    /// </summary>
    public class ProjectsController : Controller
    {
        /// <summary> Object used to insert, update, delete and read from database</summary>
        private DriversJournalContext db = new DriversJournalContext();

        /// <summary>
        /// Shows table of projects
        /// </summary>
        /// <returns>Index-view</returns>
        public ActionResult Index()
        {
            if (getSessionState() != true)
            {
                return RedirectToAction("LoggedIn", "Home");
            }
            else
            {
                List<Project> projects = db.Projects.Include(p => p.JournalUser).OrderByDescending(o => o.Active).ToList();
                return View(projects);
            }
        }
        
        /// <summary>
        /// Shows create project form
        /// </summary>
        /// <returns>Create-view</returns>
        public ActionResult Create()
        {
            if (getSessionState() != true)
            {
                return RedirectToAction("LoggedIn", "Home");
            }
            else
            {
                ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
                return View();
            }
        }

        /// <summary>
        /// Saves project to DB
        /// </summary>
        /// <param name="projectVM"> Project to be saved</param>
        /// <returns>Index-view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,ProjectNo,Name,Detail,IsActive,UserId")] ProjectVM projectVM)
        {
            if (getSessionState() != true)
            {
                return RedirectToAction("LoggedIn", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    int isActive = 0;
                    if (projectVM.IsActive)
                    {
                        isActive = 1;
                    }
                    Project project = new Project
                    {
                        ProjectId = projectVM.ProjectId,
                        ProjectNo = projectVM.ProjectNo,
                        Name = projectVM.Name,
                        Detail = projectVM.Detail,
                        Active = isActive,
                        UserId = projectVM.UserId
                    };

                    db.Projects.Add(project);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", projectVM.UserId);
                return View(projectVM);
            }
        }

        
        /// <summary>
        /// Shows project to edit
        /// </summary>
        /// <param name="id">project to edig</param>
        /// <returns>Edit-view</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            bool isActive = false;
            if (project.Active == 1)
            {
                isActive = true;
            }
            ProjectVM projectVM = new ProjectVM
            {
                ProjectId = project.ProjectId,
                ProjectNo = project.ProjectNo,
                Name = project.Name,
                Detail = project.Detail,
                IsActive = isActive,
                UserId = project.UserId
            };
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", project.UserId);
            return View(projectVM);
        }

        /// <summary>
        /// Update edited project to DB
        /// </summary>
        /// <param name="projectVM">Project values to update</param>
        /// <returns>Edit-view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectNo,Name,Detail,IsActive,UserId")] ProjectVM projectVM)
        {
            if (ModelState.IsValid)
            {
                int isActive = 0;
                if (projectVM.IsActive)
                {
                    isActive = 1;
                }
                Project project = new Project
                {
                    ProjectId = projectVM.ProjectId,
                    ProjectNo = projectVM.ProjectNo,
                    Name = projectVM.Name,
                    Detail = projectVM.Detail,
                    Active = isActive,
                    UserId = projectVM.UserId
                };
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", projectVM.UserId);
            return View(projectVM);
        }

        /// <summary>
        /// Show project that you want to delete
        /// </summary>
        /// <param name="id">retrieves project</param>
        /// <returns>Delete-view</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        /// <summary>
        /// Delete project from DB
        /// </summary>
        /// <param name="id">Project to delete</param>
        /// <returns>Index-view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// /Method to check if user is validated on session
        /// </summary>
        /// <returns>false if user is not logged in</returns>
        private bool getSessionState()
        {
            bool isActive = false;
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            //if cookie does not exist, return false
            if (cookie == null)
            {
                isActive = false;
            }
            else //if cookies does exist
            {
                FormsAuthenticationTicket ticket = null;

                //Decrypts cookie and retrieves value
                try
                {
                    ticket = FormsAuthentication.Decrypt(cookie.Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                //if ticket is not authorized, return false
                if (ticket == null)
                {
                    isActive = false;
                }
                else
                {
                    //if ticket exists and has a greater expirational
                    //date than now, return true
                    if (ticket.Expiration > DateTime.Now)
                    {
                        isActive = true;
                    }
                }
            }

            return isActive;
        }
    }
}