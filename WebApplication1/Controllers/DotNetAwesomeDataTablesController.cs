using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Linq.Dynamic;

namespace WebApplication1.Controllers
{
    public class DotNetAwesomeDataTablesController : Controller
    {
        //http://www.dotnetawesome.com/2016/12/crud-operation-using-datatables-aspnet-mvc.html

        // GET: DataTablesClient
        public ActionResult Index()
        {
            return View();
        }

        // GET: DataTables Server side pagination and sorting
        public ActionResult Index2()
        {
            return View();
        }

        // GET: DataTables Server side pagination, sorting and filtering
        public ActionResult Index3()
        {
            return View();
        }
         
        // GET: DataTables Server side pagination, sorting, filtering and crud
        //public ActionResult Index4()
        //{
        //    return View();
        //}

        public ActionResult GetContacts()
        {
            using (PracticeEntities db = new PracticeEntities())
            {
                var contacts = db.Contacts.ToList();
                return Json(new { data = contacts }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetContactsServersidePagingSorting()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            using (PracticeEntities db = new PracticeEntities())
            {
                // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
                var v = (from a in db.Contacts select a);

                //SORT
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn + " " + sortColumnDir);
                }

                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = Convert.ToInt32(draw), recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetContactsServersidePagingSortingFiltering()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //find search columns info
            var contactName = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var state = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            using (PracticeEntities db = new PracticeEntities())
            {
                // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
                var v = (from a in db.Contacts select a);

                //SEARCHING...
                if (!string.IsNullOrEmpty(contactName))
                {
                    v = v.Where(a => a.FirstName.Contains(contactName));
                }
                if (!string.IsNullOrEmpty(state))
                {
                    v = v.Where(a => a.State == state);
                }

                //SORT
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn + " " + sortColumnDir);
                }

                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = Convert.ToInt32(draw), recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public ActionResult LoadData()
        //{
        //    try
        //    {
        //        //Creating instance of DatabaseContext class  
        //        using (PracticeEntities _context = new PracticeEntities())
        //        {
        //            var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //            var start = Request.Form.GetValues("start").FirstOrDefault();
        //            var length = Request.Form.GetValues("length").FirstOrDefault();
        //            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


        //            //Paging Size (10,20,50,100)    
        //            int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //            int skip = start != null ? Convert.ToInt32(start) : 0;
        //            int recordsTotal = 0;

        //            // Getting all Customer data    
        //            var customerData = (from tempcustomer in _context.Contacts
        //                                select tempcustomer);

        //            //Sorting    
        //            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
        //            {
        //                customerData = customerData.OrderBy(sortColumn + " " + sortColumnDir);
        //            }
        //            //Search    
        //            if (!string.IsNullOrEmpty(searchValue))
        //            {
        //                customerData = customerData.Where(m => m.FirstName == searchValue ||
        //                                                       m.LastName == searchValue ||
        //                                                       m.Email == searchValue ||
        //                                                       m.City == searchValue ||
        //                                                       m.State == searchValue ||
        //                                                       m.Zip == searchValue);
        //            }

        //            //total number of rows count     
        //            recordsTotal = customerData.Count();
        //            //Paging     
        //            var data = customerData.Skip(skip).Take(pageSize).ToList();
        //            //Returning Json Data    
        //            return Json(new { draw = Convert.ToInt32(draw), recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        //[HttpGet]
        //public ActionResult Save(int id)
        //{
        //    using (PracticeEntities dc = new PracticeEntities())
        //    {
        //        var v = dc.Contacts.Where(a => a.Id == id).FirstOrDefault();
        //        return View(v);
        //    }
        //}

        //[HttpPost]
        //public ActionResult Save(Contact contact)
        //{
        //    bool status = false;
        //    if (ModelState.IsValid)
        //    {
        //        using (PracticeEntities dc = new PracticeEntities())
        //        {
        //            if (contact.Id > 0)
        //            {
        //                //Edit 
        //                var v = dc.Contacts.Where(a => a.Id == contact.Id).FirstOrDefault();
        //                if (v != null)
        //                {
        //                    v.FirstName = contact.FirstName;
        //                    v.LastName = contact.LastName;
        //                    v.Email = contact.Email;
        //                    v.PhonePrimary = contact.PhonePrimary;
        //                    v.PhoneSecondary = contact.PhoneSecondary;
        //                    v.Birthday = contact.Birthday;
        //                    v.StreetAddress1 = contact.StreetAddress1;
        //                    v.StreetAddress2 = contact.StreetAddress2;
        //                    v.City = contact.City;
        //                    v.State = contact.State;
        //                    v.Zip = contact.Zip;
        //                }
        //            }
        //            else
        //            {
        //                //Save
        //                dc.Contacts.Add(contact);
        //            }
        //            dc.SaveChanges();
        //            status = true;
        //        }
        //    }
        //    return new JsonResult { Data = new { status = status } };
        //}

        //[HttpGet]
        //public ActionResult Delete(int id)
        //{
        //    using (PracticeEntities dc = new PracticeEntities())
        //    {
        //        var v = dc.Contacts.Where(a => a.Id == id).FirstOrDefault();
        //        if (v != null)
        //        {
        //            return View(v);
        //        }
        //        else
        //        {
        //            return HttpNotFound();
        //        }
        //    }
        //}

        //[HttpPost]
        //[ActionName("Delete")]
        //public ActionResult DeleteContact(int id)
        //{
        //    bool status = false;
        //    using (PracticeEntities dc = new PracticeEntities())
        //    {
        //        var v = dc.Contacts.Where(a => a.Id == id).FirstOrDefault();
        //        if (v != null)
        //        {
        //            dc.Contacts.Remove(v);
        //            dc.SaveChanges();
        //            status = true;
        //        }
        //    }
        //    return new JsonResult { Data = new { status = status } };
        //}
    }
}