using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> GetAllComments();

        Comment GetCommentById(int id);
        void AddComment(Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(int id);

    }
}
