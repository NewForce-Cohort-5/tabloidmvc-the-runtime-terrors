using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabloidMVC.Models;
using TabloidMVC.Repositories;
using System;
using System.Collections.Generic;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class CommentController : Controller
    { 
        private readonly ICommentRepository _commentRepository;
        //Might need another of these to connect to something else, see ERD

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            //
        }
    
        public ActionResult Index()
        {
            List<Comment> comments = _commentRepository.GetAllComments();
            return View(comments);
        }

        // GET: Details/5
        public ActionResult Details(int id)
        {
          Comment comment = _commentRepository.GetCommentById(id);

                if (comment == null)
                {
                    return NotFound();
                }
            return View();
        }

        // GET: Create
        public ActionResult Create()
        {
         
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            try
            {
                _commentRepository.AddComment(comment);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Edit/5
        public ActionResult Edit(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepository.UpdateComment(comment);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);

            return View(comment);
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment)
        {
            try
            {
                _commentRepository.DeleteComment(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
