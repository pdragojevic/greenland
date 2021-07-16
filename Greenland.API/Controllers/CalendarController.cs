using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Greenland.API.Business_Logic.GoogleCalendar;
using Greenland.API.Business_Logic.JWT;
using Greenland.API.DB;
using Greenland.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Greenland.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    //changeaj kad deployas
    [ApiController]
    public class CalendarController : ControllerBase
    {

        private CalendarService calendarService = null;

        private readonly CalendarSettings calendarSettings = null;

        private Exception e;

        private X509Certificate2 cert = null;

        private ServiceAccountCredential cred = null;
        private ActivityFeedDB feedDB;



        public CalendarController(IOptions<CalendarSettings> _calendarSettings)
        {
            try
            {
                calendarSettings = _calendarSettings.Value;
                string serviceAccountEmail = "greenland@greenland-298019.iam.gserviceaccount.com";
                //string serviceAccountEmail = calendarSettings.G_Suite_Mail;

                //var certificate = new X509Certificate2(@"key.p12", "notasecret", X509KeyStorageFlags.Exportable);
                var certificate = new X509Certificate2(@"key.p12", calendarSettings.CalendarSecret, X509KeyStorageFlags.MachineKeySet);
                this.cert = certificate;
                ServiceAccountCredential credential = new ServiceAccountCredential(
                   new ServiceAccountCredential.Initializer(serviceAccountEmail)
                   {
                       User = "greenland@vrkic.co",
                       //User = calendarSettings.UserMail,
                       Scopes = new[] { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarReadonly, CalendarService.Scope.CalendarEvents }

                   }.FromCertificate(certificate));
                this.cred = credential;


                // Create the service.
                calendarService = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "GreenlandAgile app",
                });
            }
            catch (Exception e)
            {
                this.e = e;
            }
            feedDB = new ActivityFeedDB();
        }

        [Route("temp")]
        [HttpGet]
        public ActionResult Temp()
        {
            return Ok(e.StackTrace + "\n\n\n" + e.Message + "\n\n\n" + e.Source + "\n\n\n" + e.InnerException + "\n\n\n" +
                e.TargetSite + "\n\n\n" + e.HResult + "\n\n\n" + cert);
        }

        [Route("noex")]
        [HttpGet]
        public ActionResult Noexc()
        {
            if (cert == null)
            {
                return StatusCode(201, "Ne radi1!");
            }
            if (cred == null)
            {
                return StatusCode(204, "Ne radi2!");
            }
            return Ok(cert.ToString());
        }

        [Route("insert")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> InsertEvent(Event myEvent)
        {

            Employees emp = (Employees)HttpContext.Items["Employee"];
            string calendarId = emp.Email;

            try
            {
                EventsResource.InsertRequest request = calendarService.Events.Insert(myEvent, calendarId);
                request.SendNotifications = true;
                Event createdEvent = request.Execute();
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
                return StatusCode(500, "Inserting event failed!");
            }
            ActivityFeed feed = new ActivityFeed()
            {
                IdEmployee = emp.IdCompanyPosition,
                Message = emp.FirstName + " " + emp.LastName + " created meeting with his colleagues",
                CreatedTime = DateTime.Now
            };
            feedDB.AddActivity(feed);
            return StatusCode(201, "Event created");
        }

        [Route("events")]
        [HttpGet]
        [Authorize]
        public ActionResult GetEvents()
        {
            Employees emp = (Employees)HttpContext.Items["Employee"];
            string calendarId = emp.Email;
            Events events = null;
            try
            {
                events = calendarService.Events.List(calendarId).Execute();
            }
            catch (Exception)
            {
                return StatusCode(500, "Cannot get events!");
            }
            if (events == null)
            {
                return StatusCode(500, "Events are null, something is wrong!");
            }
            return Ok(events);
        }

        [Route("update")]
        [HttpPut]
        [Authorize]
        public ActionResult UpdateEvent(Event myEvent)
        {
            Employees emp = (Employees)HttpContext.Items["Employee"];
            string calendarId = emp.Email;
            try
            {
                calendarService.Events.Update(myEvent, calendarId, myEvent.Id);
            }
            catch (Exception)
            {
                return StatusCode(500, "Updating event failed!");
            }
            return Ok("Event updated");
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteEvent(string eventID)
        {
            Employees emp = (Employees)HttpContext.Items["Employee"];
            string calendarId = emp.Email;
            try
            {
                calendarService.Events.Delete(calendarId, eventID);
            }
            catch (Exception)
            {
                return StatusCode(500, "Updating event failed!");
            }
            ActivityFeed feed = new ActivityFeed()
            {
                IdEmployee = emp.IdCompanyPosition,
                Message = emp.FirstName + " " + emp.LastName + " deleted event with id" + eventID,
                CreatedTime = DateTime.Now
            };
            feedDB.AddActivity(feed);
            return Ok("Event updated");
        }


        [Route("clear")]
        [HttpGet]
        [Authorize]
        public ActionResult ClearCalendar()
        {
            Employees emp = (Employees)HttpContext.Items["Employee"];
            string calendarId = emp.Email;
            try
            {
                calendarService.Calendars.Clear(calendarId).Execute();
            }
            catch (Exception)
            {
                return StatusCode(500, "Request for clearing calendar with calendar id " + calendarId + " failed!");
            }
            return Ok("Calendar cleared with id " + calendarId + " successfully!");
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize]
        public ActionResult DeleteCalendar()
        {
            Employees emp = (Employees)HttpContext.Items["Employee"];
            string calendarId = emp.Email;
            try
            {
                calendarService.Calendars.Delete(calendarId).Execute();
            }
            catch (Exception)
            {
                return StatusCode(500, "Request for deleting calendar with calendar id " + calendarId + " failed!");
            }
            return StatusCode(204);
        }

        [Route("get")]
        [HttpGet]
        [Authorize]
        public ActionResult GetCalendar()
        {
            Employees emp = (Employees)HttpContext.Items["Employee"];
            string calendarId = emp.Email;
            Calendar calendar = null;
            try
            {
                calendar = calendarService.Calendars.Get(calendarId).Execute();
            }
            catch (Exception)
            {
                return StatusCode(500, "Request for getting calendar with calendar id " + calendarId + " failed!");
            }
            if (calendar == null)
            {
                return StatusCode(200, "No such calendar!");
            }
            return Ok(calendar);
        }

        [Route("add")]
        [HttpPost]
        [Authorize]
        public ActionResult AddCalendar(Calendar calendar)
        {
            try
            {
                calendarService.Calendars.Insert(calendar).Execute();
            }
            catch (Exception)
            {
                return StatusCode(500, "Cannot insert calendar!");
            }
            return StatusCode(201);
        }


    }
}
