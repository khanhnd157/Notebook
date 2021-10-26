using CodeMaze.Cryptography.Hash;
using CodeMaze.Models;
using CodeMaze.Notebooks.Extensions;
using CodeMaze.Services;

using CodeMazeNET.TextGenerator;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMaze.Notebooks.Controllers
{
    public class NoteController : Controller
    {
        private readonly ILogger<NoteController> _logger;
        private readonly INotebookService notebookService;

        public NoteController(ILogger<NoteController> logger,
            INotebookService notebookService)
        {
            _logger = logger;
            this.notebookService = notebookService;
        }

        [HttpGet]
        [Authorize]
        [Route("/notes/manage")]
        public async Task<IActionResult> GetAllAsync()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            var username = HttpContext.User.Claims
                        .Where(x => x.Type.Equals(ClaimUserTypes.UserName) && !string.IsNullOrEmpty(x.Value))
                        .Select(x => x.Value.Trim().ToLower())
                        .FirstOrDefault();

            var notes = await notebookService.GetAllByUserAsync(username.ToLower());
            return View("Manage", notes);
        }

        [HttpGet]
        [Route("/"), Route("/note/add")]
        public IActionResult Add()
        {
            return View("AddNote");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/"), Route("/note/add")]
        public async Task<IActionResult> AddAsync(NotebookModel notebook)
        {
            var code = TextGenerator.Builder(8);

            var exists = await notebookService.AnyByCodeAsync(code);

            if (!exists)
            {
                notebook.Code = code;

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    notebook.Username = HttpContext.User.Claims
                        .Where(x => x.Type.Equals(ClaimUserTypes.UserName) && !string.IsNullOrEmpty(x.Value))
                        .Select(x => x.Value.Trim().ToLower())
                        .FirstOrDefault();
                }

                await notebookService.AddAsync(notebook);

                return Redirect($"/note/{code}?utm_source=notebook&utm_medium=codemaze&utm_campaign=codemaze");
            }

            return View();
        }


        [HttpGet]
        [Authorize]
        [Route("/note/edit/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id)
        {
            var note = await notebookService.GetByIdAsync(id);


            return View("Update", note);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("/note/edit/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromForm] NotebookModel notebook)
        {
            if (await notebookService.AnyByIdAsync(id))
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    notebook.Username = HttpContext.User.Claims
                        .Where(x => x.Type.Equals(ClaimUserTypes.UserName) && !string.IsNullOrEmpty(x.Value))
                        .Select(x => x.Value.Trim().ToLower())
                        .FirstOrDefault();
                }

                await notebookService.UpdateAsync(id, notebook);

                return Redirect($"/notes/manage?utm_source=notebook&utm_medium=codemaze&utm_campaign=codemaze");
            }

            return View();
        }




        [HttpGet]
        [Route("/{code?}"), Route("/note/{code?}")]
        public async Task<IActionResult> GetAsync(string code)
        {
            var notebook = await notebookService.GetByCodeAsync(code);

            if (notebook.Access == "private" && !HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/auth/signin?callback=/" + code);
            }

            var permission = CheckPermission(HttpContext.Session, notebook, string.Empty);

            if (permission)
            {
                var redirectUrl = string.Format(RedirectWithPermission(permission), code);

                return Redirect(redirectUrl);
            }

            return View("Read", notebook);
        }

        [HttpGet]
        [HttpPost]
        [Route("/note/access/{code?}")]
        public async Task<IActionResult> accessAsync(string code, [FromForm] string password)
        {
            var notebook = await notebookService.GetByCodeAsync(code);

            if (!string.IsNullOrEmpty(password))
            {
                var _checked = HttpContext.Session.Get<NotebookChecked>(code);

                if (_checked is not null)
                    HttpContext.Session.Remove(code);

                var notemem = new NotebookChecked { Password = HashPassword.Hash(password), Checked = HashPassword.VerifyHashed(notebook.Password, password) };
                HttpContext.Session.Set(code, notemem);
            }

            var permission = CheckPermission(HttpContext.Session, notebook, password);

            if (!permission)
            {
                var redirectUrl = string.Format(RedirectWithPermission(permission), code);

                return Redirect(redirectUrl);
            }

            ViewData["NOTE_TITLE"] = notebook.Title;

            return View("Access", code);
        }

        private bool CheckPermission(ISession session, NotebookModel notebook, string password)
        {
            if (notebook is null) return false;

            if (notebook.Access.Equals(Enum.GetName(AccessModify.Protected), StringComparison.CurrentCultureIgnoreCase))
            {
                if (session is null) return true;

                var note = session.Get<NotebookChecked>(notebook.Code);

                if (note is null) return true;

                if (HashPassword.VerifyHashed(note.Password, password) && !note.Checked) return true;
            }

            return false;
        }

        [HttpDelete]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("/api/note/{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var username = HttpContext.User.Claims
                     .Where(x => x.Type.Equals(ClaimUserTypes.UserName) && !string.IsNullOrEmpty(x.Value))
                     .Select(x => x.Value.Trim().ToLower())
                     .FirstOrDefault();

                var note = await this.notebookService.GetByIdAsync(id);
                if (note.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase))
                {
                    var result = await this.notebookService.DeleteByIdAsync(id);
                    return Ok(result);
                }
            }

            return Ok(false);
        }

        private string RedirectWithPermission(bool protect = true)
        {
            if (protect)
                return "/note/access/{0}?utm_source=notebook&utm_medium=codemaze&utm_campaign=codemaze_protected";

            return "/{0}?utm_source=notebook&utm_medium=codemaze&utm_campaign=codemaze";
        }
    }
}
