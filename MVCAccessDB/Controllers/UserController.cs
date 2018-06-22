using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCAccessDB.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            IList<UserModel> users = new List<UserModel>();

            using (var db = new MyDbContext("Name=MDTDbConn"))
            {
                var userList = db.Users.ToList();
                foreach (var user in userList)
                {
                    users.Add(new UserModel
                    {
                        FirstName = user.FirstName,
                        Lastname = user.Lastname,
                        UserId = user.UserId,
                        UserName = user.UserName

                    });
                }

                return View(users);
            }
        }

        public ActionResult Create()
        {
            UserModel model = new UserModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserModel model)//FormCollection collection)
        {
            try
            {

                using (var db = new MyDbContext("Name=MDTDbConn"))
                {
                    var mdt = db.Set<User>();

                    mdt.Add(new User
                    {
                        DateCreated = DateTime.Now,
                        FirstName = model.FirstName,
                        Lastname = model.Lastname,
                        IsAdmin = model.IsAdmin,
                        UserName = "admin",
                        Password = "admin",
                        RowGuid = Guid.NewGuid()

                    });

                    db.SaveChanges();

                };
                return View("Index", model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}