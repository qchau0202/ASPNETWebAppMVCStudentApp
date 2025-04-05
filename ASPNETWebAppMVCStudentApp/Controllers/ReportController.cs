using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp;
using Microsoft.Reporting.WebForms;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class ReportController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        // GET: Report
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Courses = new SelectList(db.Courses1, "CourseID", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult Generate(int SelectedCourseID, string action)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var course = db.Courses1.FirstOrDefault(c => c.CourseID == SelectedCourseID);
            if (course == null)
            {
                return HttpNotFound("Course not found");
            }
            string courseName = course.Title;

            var students = db.Students
                .Join(db.Enrollments, s => s.StudentID, e => e.StudentID, (s, e) => new { s, e })
                .Where(se => se.e.CourseID == SelectedCourseID)
                .Select(se => new
                {
                    se.s.StudentID,
                    se.s.FirstName,
                    se.s.LastName,
                    se.e.Grade
                }).ToList();

            System.Diagnostics.Debug.WriteLine("Number of students: " + students.Count);

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/StudentReport.rdlc");
            localReport.DataSources.Add(new ReportDataSource("StudentDataSet", students));

            ReportParameter courseNameParam = new ReportParameter("CourseName", courseName);
            localReport.SetParameters(new ReportParameter[] { courseNameParam });

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo = "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>1in</MarginLeft>" +
                "  <MarginRight>1in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                "PDF",
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            if (action == "export")
            {
                // Force download for export
                string fileName = $"StudentReport_{courseName}_{DateTime.Now:yyyyMMdd}.pdf";
                Response.AppendHeader("Content-Disposition", $"attachment; filename={fileName}");
                return File(renderedBytes, mimeType);
            }
            else // preview
            {
                // Display in browser for preview
                return File(renderedBytes, mimeType);
            }
        }
    }
}