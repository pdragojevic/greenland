using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greenland.API.Business_Logic;
using Greenland.API.Business_Logic.Mail_Logic;
using Greenland.API.DB;
using Greenland.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Mozilla;

namespace Greenland.API.Controllers
{
    // [Route("api")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IMailService mailService;

        private EmployeesDB employeesDB;

        private ForgotPasswordDB forgotPasswordDB;

        public ForgotPasswordController(IMailService mailService)
        {
            this.mailService = mailService;
            employeesDB = new EmployeesDB();
            forgotPasswordDB = new ForgotPasswordDB();
        }

        [HttpPost]
        [Route("forgot")]
        public async Task<IActionResult> ForgotPassword(ForgotRequest request)
        {
            Employees emp = employeesDB.GetEmployeeByMailSync(request.mail);
            if (emp == null)
            {
                return Unauthorized();
            }
            string guid = Guid.NewGuid().ToString();
            string guidHash = Util.hashSHA256(guid);
            DateTime localDate = DateTime.Now;
            ForgotPassword newRow = new ForgotPassword();
            newRow.Guid = guidHash;
            newRow.TimeCreated = localDate;
            newRow.IdEmployee = emp.IdEmployee;

            try
            {
                await forgotPasswordDB.Insert(newRow);
                Object obj = await SendMail(guid, request.mail);
                return Ok(obj);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
            //dobi trenutacno vrijeme
            //upisi mail
            //ovo troje upisi u bazu
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(NewPassword model)   //NoviModel ima - newPassword i ima guid(ali nije hashiran) 
        {
            var guidHash = Util.hashSHA256(model.guid);
            ForgotPassword row = await forgotPasswordDB.GetForgotPasswordRowByGuid(guidHash);
            if (row == null ^ DateTime.Now > row.TimeCreated.AddDays(1))
            {
                return Unauthorized();
            }
            Employees employee = await employeesDB.GetEmployeeById(row.IdEmployee);
            employee.Password = Util.hashSHA256(model.password);
            try
            {
                await employeesDB.UpdateEmployee(employee);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
            //sad izvadi iz tablice na temelju guida - ti ga hashiras i izvadis
            //ako postoji nesto to znaci da je korisnik zatrazio na pravilan nacin
            //mail employee ces izvuc iz baze
            //nades employee na temelju maila
            //promijeni mu password
            // ako nije proslo vise od 24 sata od zahtjeva
            //na frontu - ako je vracen ok onda ga redirectaj i ispise neku poruku alert();
        }


        private async Task<Object> SendMail(string guid, string mail)
        {
            var sb = new StringBuilder("<body style='margin: 0px;'>");
            //sb.AppendFormat("<div><a href='https://localhost:4200/forgot-password?guid={0}'>Click here</a></div>", guid);
            sb.AppendFormat("<div><a href='http://51.103.112.37/forgot-password?guid={0}'>Click here</a></div>", guid);
            MailRequest request = new MailRequest();
            request.Attachments = null;
            request.Body = sb.ToString();//new pasword and confirm new password?id=guid
            request.Subject = "Greenland";
            request.ToEmail = mail;
            try
            {
                await mailService.SendEmailAsync(request);
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(e.Message);
            }
            return JsonConvert.SerializeObject("OK");
        }
    }
}
