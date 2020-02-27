using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    public class DataTablesCrudController : Controller
    {
        //https://www.c-sharpcorner.com/article/using-datatables-grid-with-asp-net-mvc/

        // GET: DataTablesCrud
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadData()
        {
            try
            {
                //Creating instance of DatabaseContext class  
                using (PracticeEntities _context = new PracticeEntities())
                {
                    var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    var start = Request.Form.GetValues("start").FirstOrDefault();
                    var length = Request.Form.GetValues("length").FirstOrDefault();
                    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                    var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                    //Paging Size (10,20,50,100)    
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;

                    // Getting all Customer data    
                    var customerData = (from tempcustomer in _context.Contacts
                                        select tempcustomer);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        customerData = customerData.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    //Search    
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        customerData = customerData.Where(m => m.FirstName == searchValue ||
                                                               m.LastName == searchValue ||
                                                               m.Email == searchValue ||
                                                               m.City == searchValue ||
                                                               m.State == searchValue ||
                                                               m.Zip == searchValue);
                    }

                    //total number of rows count     
                    recordsTotal = customerData.Count();
                    //Paging     
                    var data = customerData.Skip(skip).Take(pageSize).ToList();
                    //Returning Json Data    
                    return Json(new { draw = Convert.ToInt32(draw), recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,PhonePrimary,PhoneSecondary,Birthday,StreetAddress1,StreetAddress2,City,State,Zip")] Contact contact)
        {
            using (PracticeEntities _context = new PracticeEntities())
            {
                if (ModelState.IsValid)
                {
                    _context.Contacts.Add(contact);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(contact);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (PracticeEntities _context = new PracticeEntities())
            {
                Contact contact = (from c in _context.Contacts
                                   where c.Id == ID
                                   select c).FirstOrDefault();
                if (contact == null)
                {
                    return HttpNotFound();
                }
                return View(contact);
            }
            //try
            //{
            //    using (PracticeEntities _context = new PracticeEntities())
            //    {
            //        var contact = (from c in _context.Contacts
            //                       where c.Id == ID
            //                       select c).FirstOrDefault();
            //        return View(contact);
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,PhonePrimary,PhoneSecondary,Birthday,StreetAddress1,StreetAddress2,City,State,Zip")] Contact contact)
        {
            using (PracticeEntities _context = new PracticeEntities())
            {
                if (ModelState.IsValid)
                {
                    _context.Entry(contact).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(contact);
            }
        }

        [HttpPost]
        public JsonResult DeleteContact(int? ID)
        {
            using (PracticeEntities _context = new PracticeEntities())
            {
                var contact = _context.Contacts.Find(ID);
                if (ID == null)
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                _context.Contacts.Remove(contact);
                _context.SaveChanges();

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
        }

    }
}