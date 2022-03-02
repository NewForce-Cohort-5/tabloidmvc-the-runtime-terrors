using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : ICommentRepository
    {
  
            private readonly IConfiguration _config;

            public CommentRepository(IConfiguration config)
            {
                _config = config;
            }

            public SqlConnection Connection
            {
                get
                {
                    return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                }
            }

            public Comment GetCommentById(int id)
            {
                using (SqlConnection conn = Connection)
                    {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                SELECT Id, Subject, Content, Author, Date
                                  FROM Comment
                                 WHERE Id = @id";

                        cmd.Parameters.AddWithValue("@id", id);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            Comment comment = new Comment()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                Author = reader.GetString(reader.GetOrdinal("Author")),
                                Date = reader.GetInt32(reader.GetOrdinal("Date"))
                            };

                            reader.Close();
                            return comment;
                        }

                        reader.Close();
                        return null;
                         
                    }
                }
            }

            public void AddComment(Comment comment)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                INSERT INTO Comment (Subject, Content, Author, Date)
                                OUTPUT INSERTED.ID
                                VALUES (@subject, @content, @author, @date";

                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@author", comment.Author);
                    cmd.Parameters.AddWithValue("@date", comment.Date);

                    int id = (int)cmd.ExecuteScalar();

                    comment.Id = id;
                }
                }
            }

            public void UpdateComment(Comment comment)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                   UPDATE Comment
                                     SET
                                           Subject = @subject,
                                           Content = @content,
                                           Author = @author,
                                            Date = @date
                                    WHERE Id = @id";

                        cmd.Parameters.AddWithValue("@subject", comment.Subject);
                        cmd.Parameters.AddWithValue("@content", comment.Content);
                        cmd.Parameters.AddWithValue("@author", comment.Author);
                        cmd.Parameters.AddWithValue("@date", comment.Date);

                        cmd.ExecuteNonQuery();

                    }
                }
            }

            public void DeleteComment(int commentId)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            DELETE FROM Comment
                                   WHERE Id = @id";

                        cmd.Parameters.AddWithValue("@id", commentId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            public List<Comment> GetAllComments()
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                SELECT Id, Subject, Content, Author, Date
                                FROM Comment";

                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Comment> comments = new List<Comment>();

                        while (reader.Read())
                        {
                            Comment comment = new Comment
                            {
                                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                Author = reader.GetString(reader.GetOrdinal("Author")),
                                Date = reader.GetInt32(reader.GetOrdinal("Date"))
                            };

                            comments.Add(comment);
                        }

                        reader.Close();

                        return comments;
                    }
                }
            }



        }

    }

