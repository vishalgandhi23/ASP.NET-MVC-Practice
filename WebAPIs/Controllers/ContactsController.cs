using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPIs.Models;

namespace WebAPIs.Controllers
{
    public class ContactsController : ApiController
    {
        //https://www.c-sharpcorner.com/article/asp-net-web-api-crud-logics-using-entity-framework-without-writing-single-code/
        private PracticeEntities db = new PracticeEntities();

        // GET: api/Contacts
        [ResponseType(typeof(IEnumerable<Contact>))]
        [Route("api/GetContacts")]
        public IQueryable<Contact> GetContacts()
        {
            return db.Contacts;
        }

        // GET: api/Contacts/5
        [ResponseType(typeof(Contact))] 
        [Route("api/GetContact")]
        public IHttpActionResult GetContact(long id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // PUT: api/Contacts/5
        [ResponseType(typeof(void))]
        [Route("api/PutContact")]
        public IHttpActionResult PutContact(long id, Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contact.Id)
            {
                return BadRequest();
            }

            db.Entry(contact).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Contacts
        [ResponseType(typeof(Contact))]
        [Route("api/PostContact")]
        public IHttpActionResult PostContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contacts.Add(contact);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = contact.Id }, contact);
        }

        // DELETE: api/Contacts/5
        [ResponseType(typeof(Contact))]
        [Route("api/DeleteContact")]
        public IHttpActionResult DeleteContact(long id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }

            db.Contacts.Remove(contact);
            db.SaveChanges();

            return Ok(contact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Route("api/ContactExists")]
        private bool ContactExists(long id)
        {
            return db.Contacts.Count(e => e.Id == id) > 0;
        }
    }
}