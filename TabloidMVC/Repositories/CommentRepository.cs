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
                                SELECT Id, Subject, Content, UserProfileId, PostId, CreateDateTime
                                  FROM Comment
                                 WHERE Id = @id";

                        cmd.Parameters.AddWithValue("@id", id);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            Comment comment = new Comment()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                                CreateDateTime = reader.GetInt32(reader.GetOrdinal("CreateDateTime"))
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
                                INSERT INTO Comment (Subject, Content, CreateDateTime)
                                OUTPUT INSERTED.ID
                                VALUES (@subject, @content, @createDateTime";

                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", comment.CreateDateTime);

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
                                           
                                           PostId = @postId,
                                           Subject = @subject,
                                           Content = @content,
                                           UserProfileId = @userProfileId,
                                            CreateDateTime = @createDateTime
                                    WHERE Id = @id";

                        cmd.Parameters.AddWithValue("@postId", comment.PostId);
                        cmd.Parameters.AddWithValue("@subject", comment.Subject);
                        cmd.Parameters.AddWithValue("@content", comment.Content);
                        cmd.Parameters.AddWithValue("@userProfileId", comment.UserProfileId);
                        cmd.Parameters.AddWithValue("@createDateTime", comment.CreateDateTime);

                        cmd.ExecuteNonQuery();

                    }
                }
            }

        public Post GetUserPostById(int id, int userProfileId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT p.Id, p.Title, p.Content, 
                              p.ImageLocation AS HeaderImage,
                              p.CreateDateTime, p.PublishDateTime, p.IsApproved,
                              p.CategoryId, p.UserProfileId,
                              c.[Name] AS CategoryName,
                              u.FirstName, u.LastName, u.DisplayName, 
                              u.Email, u.CreateDateTime, u.ImageLocation AS AvatarImage,
                              u.UserTypeId, 
                              ut.[Name] AS UserTypeName
                         FROM Post p
                              LEFT JOIN Category c ON p.CategoryId = c.id
                              LEFT JOIN UserProfile u ON p.UserProfileId = u.id
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE p.id = @id AND p.UserProfileId = @userProfileId";

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@userProfileId", userProfileId);
                    var reader = cmd.ExecuteReader();

                    Post post = null;

                    if (reader.Read())
                    {
                        post = NewPostFromReader(reader);
                    }

                    reader.Close();

                    return post;
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
                                SELECT Id, PostId, UserProfileId, Subject, Content, CreateDateTime
                                FROM Comment";

                    SqlDataReader reader = cmd.ExecuteReader();

                        List<Comment> comments = new List<Comment>();

                        while (reader.Read())
                        {
                            Comment comment = new Comment
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                CreateDateTime = reader.GetInt32(reader.GetOrdinal("CreateDateTime"))
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

