using System;
using System.Web.Mvc;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Mvc.Forms;
using AsbaBank.Presentation.Mvc.ViewModelBuilders;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class ClientController : Controller
    {
        private readonly IRepository<Client> clientRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ClientViewModelBuilder viewModelBuilder;

        public ClientController()
        {
            unitOfWork = new InMemoryUnitOfWork(MvcApplication.DataStore);
            clientRepository = unitOfWork.GetRepository<Client>();
            viewModelBuilder = new ClientViewModelBuilder(unitOfWork.GetRepository<Account>());
        }

        public ActionResult Index()
        {
            return View(clientRepository);
        }

        public ActionResult Details(int id = 0)
        {
            var client = clientRepository.Get(id);

            if (client == null)
            {
                return HttpNotFound();
            }

            return View(viewModelBuilder.Build(client));
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(NewClientForm clientForm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new Client(clientForm.ClientName, clientForm.PhoneNumber);
                    clientRepository.Add(client);
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return View(clientForm);
        }
    }
}