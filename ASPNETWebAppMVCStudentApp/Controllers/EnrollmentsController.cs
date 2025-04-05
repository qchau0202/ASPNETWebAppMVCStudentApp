    using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class EnrollmentsController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        // GET: Enrollments
        public ActionResult Index()
        {
            var enrollments = db.Enrollments.Include(e => e.Cours).Include(e => e.Student);
            return View(enrollments.ToList());
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses1, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students.Select(s => new { s.StudentID, FullName = s.FirstName + " " + s.LastName }), "StudentID", "FullName");
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                // Ensure the Grade value is within the valid range
                if (enrollment.Grade < 1.00m || enrollment.Grade > 4.00m)
                {
                    ModelState.AddModelError("Grade", "Grade must be between 1.00 and 4.00.");
                }
                else
                {
                    db.Enrollments.Add(enrollment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CourseID = new SelectList(db.Courses1, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FullName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses1, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students.Select(s => new { s.StudentID, FullName = s.FirstName + " " + s.LastName }), "StudentID", "FullName", enrollment.StudentID);
            return View(enrollment);
        }


        // POST: Enrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                if (enrollment.Grade < 1.00m || enrollment.Grade > 4.00m)
                {
                    ModelState.AddModelError("Grade", "Grade must be between 1.00 and 4.00.");
                }
                else
                {
                    db.Entry(enrollment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CourseID = new SelectList(db.Courses1, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "FullName", enrollment.StudentID);
            return View(enrollment);
        }


        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
    }
}
