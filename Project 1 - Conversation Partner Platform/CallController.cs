using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Twilio;
using Microsoft.AspNet.Identity;
using CPP2.Services;

namespace CPP2.Controllers
{
	using System.Diagnostics.CodeAnalysis;

	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
		Justification = "Reviewed. Suppression is OK here.")]
	public class CallController : Controller
	{
		public readonly CPPdatabaseEntities db = new CPPdatabaseEntities();

		// GET: Call
		public ActionResult Index()
		{
			try
			{
				var contactOwnerId = CppUserService.GetCppUserId(User.Identity.GetUserId(), db);
				List<Contact> contactList;

				try
				{
					contactList = ContactService.GetContactList(contactOwnerId, db).ToList();
				}
				catch (Exception eIndexContactList)
				{
					return View("Error", new HandleErrorInfo(eIndexContactList, "Call", "Index"));
				}
				var profileList = new List<Profile>();
				profileList.AddRange(
					contactList.Select(contact => db.Profiles.FirstOrDefault(prof => prof.CppUserId == contact.ContactListMemberId)));

				return View(profileList);
			}
			catch (Exception eIndex)
			{
				return View("Error", new HandleErrorInfo(eIndex, "Call", "Index"));
			}
		}

		public ActionResult SendEmailRequest()
		{
			var aspNetUserId = User.Identity.GetUserId();
			var sessionId = Request["sessionId"];
			var recipientemail = Request["recipientEmail"];

			MessageService.SendEmailRequest(User.Identity.GetUserName(), recipientemail, sessionId);

			var senderId = db.CppUsers.Where(u => u.AspNetUser.Id == aspNetUserId).Select(u => u.Id).Single();
			var recipientId = db.CppUsers.Where(u => u.AspNetUser.Email == recipientemail).Select(u => u.Id).Single();

			//SessionService.BeginSessionLog(senderId, recipientId); //Shouldn't begin session until they click link


			var contactOwnerId = CppUserService.GetCppUserId(User.Identity.GetUserId(), db);
			List<Contact> contactList;

			try
			{
				contactList = ContactService.GetContactList(contactOwnerId, db).ToList();
			}
			catch (Exception eIndexContactList)
			{
				return View("Error", new HandleErrorInfo(eIndexContactList, "Call", "Index"));
			}

			var profileList = new List<Profile>();
			profileList.AddRange(
				contactList.Select(contact => db.Profiles.FirstOrDefault(prof => prof.CppUserId == contact.ContactListMemberId)));


            return View("Index", profileList);
		}

		/// <summary>
		/// User that receives a call request via email will land on this controller when clicking the link they receive
		/// </summary>
		/// <returns></returns>
		public ActionResult InitiateCall()
		{
			var aspNetUserId = User.Identity.GetUserId();
			var senderAspNetUserId = Request.QueryString["sender"];
			var recipientAspNetUserId = User.Identity.GetUserId();

			var senderId =
				db.CppUsers.Where(p => p.AspNetUser.Id == senderAspNetUserId).Select(p => p.Id).SingleOrDefault();
			var recipientId =
				db.CppUsers.Where(p => p.AspNetUser.Id == recipientAspNetUserId).Select(p => p.Id).SingleOrDefault();


			var sessionId = Request.QueryString["session"];
            //SessionService.BeginSessionLog(senderId, recipientId, sessionId);


            return View("MakeAppRtcCall", (object) sessionId);
			//return view for receiver of request and use session id and apprtc to launch popup for them
		}

		public ActionResult StartSessionLog()
		{
			//use session id to get sender id and recipient id
			var session = Request["session"];
			var sender = int.Parse(Request["sender"]);
			var recipient = int.Parse(Request["recipient"]);

			//create session record in the db
			SessionService.BeginSessionLog(sender, recipient, session);
			List<Contact> contactList;

			try
			{
				contactList = ContactService.GetContactList(recipient, db).ToList();
			}
			catch (Exception eIndexContactList)
			{
				return View("Error", new HandleErrorInfo(eIndexContactList, "Call", "Index"));
			}

			var profileList = new List<Profile>();
			profileList.AddRange(
				contactList.Select(contact => db.Profiles.FirstOrDefault(prof => prof.CppUserId == contact.ContactListMemberId)));

			return View("Index", profileList);
		}

		public ActionResult MakeTwilioCall()
		{
			const string ACCOUNT_SID_Live = "AC46d0e0f69a193b07971e097acaf83492";
			//const string ACCOUNT_SID_Test = "ACc6d4a6b356623b01abc77875d04dc35d";
			const string ACCOUNT_SID_928 = "PNb64a69077177857068b230f464db26b4";
			const string AUTH_TOKEN_Live = "289c300132612448d804f7a64377b73f";
			//const string AUTH_TOKEN_Test = "d917a813c302359c357b42e84694df19";
			const string Twilio_928 = "+19282373669"; //Twilio number
			const string Caroline = "+15712321174"; //Caroline
													//const string Harrison = "+12533487919"; //Harrison
			try
			{
				TwilioClient.Init(ACCOUNT_SID_Live, AUTH_TOKEN_Live);
				var call = Twilio.Rest.Api.V2010.Account.CallResource.Create(
					new Twilio.Types.PhoneNumber(Caroline),
					from: new Twilio.Types.PhoneNumber(Twilio_928),
					url: new Uri("https://demo.twilio.com/welcome/voice")
				//https://my.twiml.here
				);


                return View();
            }
			catch (Exception eTwilio)
			{
				return View("Error", new HandleErrorInfo(eTwilio, "Call", "MakeTwilioCall"));
			}
		}

        /*
         * predetermined conversation themes
         * returns theme in string
         */
        public string getTheme()
        {
            List<string> themesList = new List<string>();
            string themeToReturn = "";
            
            string th1 = "Baseball";
            string th2 = "Musical instruments";
            string th3 = "Cooking";
            string th4 = "Traveling in Europe";
            string th5 = "Movies";
            string th6 = "Gaming";
            string th7 = "Game of Thrones";
            string th8 = "Martial arts";
            string th9 = "Farm";
            string th10 = "Water sports";
            string th11 = "American football";
            string th12 = "Basketball";
            string th13 = "Traveling in Asia";
            string th14 = "Grocery store";
            string th15 = "Dogs and cats";
            string th16 = "Cars";
            string th17 = "Computer programming";
            string th18 = "School and classroom";
            string th19 = "Hospital";
            string th20 = "News and current events";
            string th21 = "Photography";
            string th22 = "Music";
            string th23 = "Hiking";
            string th24 = "Craft beer";
            string th25 = "American history";

            themesList.Add(th1);
            themesList.Add(th2);
            themesList.Add(th3);
            themesList.Add(th4);
            themesList.Add(th5);
            themesList.Add(th6);
            themesList.Add(th7);
            themesList.Add(th8);
            themesList.Add(th9);
            themesList.Add(th10);
            themesList.Add(th11);
            themesList.Add(th12);
            themesList.Add(th13);
            themesList.Add(th14);
            themesList.Add(th15);
            themesList.Add(th16);
            themesList.Add(th17);
            themesList.Add(th18);
            themesList.Add(th19);
            themesList.Add(th20);
            themesList.Add(th21);
            themesList.Add(th22);
            themesList.Add(th23);
            themesList.Add(th24);
            themesList.Add(th25);


            //randomly choose a theme
            Random rand = new Random();
            int i = rand.Next(0, themesList.Count - 1);

            if(i >= 0 && i < themesList.Count)
            {
                themeToReturn = themesList.ElementAtOrDefault(i);
            }
            else
            {
                themeToReturn = "Old Dominion University";
            }

            if(themeToReturn.Equals("") || themeToReturn.Equals(null))
            {
                themeToReturn = "Fishing in Virginia";
            }

            return themeToReturn;
        }
	}
}