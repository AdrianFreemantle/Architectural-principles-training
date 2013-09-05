using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Mvc.Forms;
using AsbaBank.Presentation.Mvc.ViewModelBuilders;
using AsbaBank.Presentation.Mvc.ViewModels;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<Account> accountRepository;
        private readonly AccountViewModelBuilder modelBuilder;
        private readonly IUnitOfWork unitOfWork;

        public AccountController()
        {
            unitOfWork = new InMemoryUnitOfWork(MvcApplication.DataStore);
            accountRepository = unitOfWork.GetRepository<Account>();
            modelBuilder = new AccountViewModelBuilder(unitOfWork.GetRepository<Client>());
        }

        public ActionResult Index()
        {
            List<Account> accounts = accountRepository.ToList();
            IEnumerable<AccountViewModel> viewModels = modelBuilder.Build(accounts);
            return View(viewModels);
        }

        public ActionResult Details(int id = 0)
        {
            Account account = accountRepository.Get(id);
            AccountViewModel viewModel = modelBuilder.Build(account);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Close(int id = 0)
        {
            try
            {
                Account account = accountRepository.Get(id);
                account.Close(id);
                unitOfWork.Commit();
                return RedirectToAction("Details", new { id = account.Id });
            }
            catch 
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public ActionResult Debit(int id = 0)
        {
            Account account = accountRepository.Get(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(new AccountDebitForm
            {
                AccountId = account.Id,
                DebitAmount = 0
            });
        }

        [HttpPost]
        public ActionResult Debit(AccountDebitForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Account account = accountRepository.Get(form.AccountId);
                    account.Debit(form.DebitAmount);
                    unitOfWork.Commit();
                    return RedirectToAction("Details", new { id = account.Id });
                }
                catch 
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return View("Debit", form);
        }

        public ActionResult Credit(int id = 0)
        {
            Account account = accountRepository.Get(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(new AccountCreditForm
            {
                AccountId = account.Id,
                CreditAmount = 0
            });
        }

        [HttpPost]
        public ActionResult Credit(AccountCreditForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Account account = accountRepository.Get(form.AccountId);
                    account.Credit(form.AccountId, form.CreditAmount);
                    unitOfWork.Commit();
                    return RedirectToAction("Details", new { id = account.Id });
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            
            return View("Credit", form);
        }

        [HttpPost]
        public ActionResult OpenAccount(int clientId)
        {
            try
            {
                Account account = Account.OpenAccount(clientId);
                accountRepository.Add(account);
                unitOfWork.Commit();
                return RedirectToAction("Details", "Client", new {id = clientId});
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        [HttpPost]
        public ActionResult IssueBankCard(int id = 0)
        {
            try
            {
                Account account = accountRepository.Get(id);
                account.IssueBankCard(id);
                unitOfWork.Commit();
                return RedirectToAction("Details", new { id = account.Id });
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        [HttpPost]
        public ActionResult StopBankCard(int id = 0)
        {
            try
            {
                Account account = accountRepository.Get(id);
                account.StopBankCard();
                unitOfWork.Commit();
                return RedirectToAction("Details", new { id = account.Id });
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        } 
    }
}