using Dapper;
using portal.Database;
using portal.Models;
using portal.Services;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;

namespace portal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IDbConnection _db;
        public UserRepository(DbConnect dbConnect)
        {
            _db = dbConnect.Connection;
        }
        public List<dynamic> ListAll()
        {
            using (var db = _db)
            {
                var query = @"select user_usuario 
                                    ,senha_usuario
                              from USR_USUARIO";

                List<dynamic> result = db.Query<dynamic>(query).ToList();

                return result;

            }
        }

        public bool ExisteUsuario(UserModel userModel)
        {
            using (var db = _db)
            {
                var query = @"select user_usuario 
                                    ,senha_usuario
                              from USR_USUARIO";
                query += @" where USR_USUARIO.user_usuario = '" + userModel.UserName + "'" +
                          " and USR_USUARIO.senha_usuario = '"+ userModel.Password +"'";

                List<dynamic> result = db.Query<dynamic>(query).ToList();

                if(result.Count > 0)
                {
                    return true;
                } else
                {
                    return false;
                }
                
            }
        }

        public bool Registrar(UserRegisterModel userRegisterModel)
        {
            using (var db = _db)
            {
                var user = new UserRegisterModel();
    
                user.UserName = userRegisterModel.UserName;
                user.Password = userRegisterModel.Password;
                var insertSql = @"INSERT INTO [USR_USUARIO] 
                                        (  
                                            user_usuario, 
                                            senha_usuario )
                                  VALUES(  
                                            @UserName, 
                                            @Password )";


                var rows = _db.Execute(insertSql, new
                {
                    user.Id,
                    user.UserName,
                    user.Password,
                });
                if(rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }


        public bool Recupear(UserRegisterModel userRegisterModel, string userAtual)
        {
            using (var db = _db)
            {

                    var updateQuery = "UPDATE [USR_USUARIO] SET [user_usuario]=@UserName, [senha_usuario]=@Password WHERE [user_usuario]=@userAtual";
                    var rows = _db.Execute(updateQuery, new
                    {
                        UserName = userRegisterModel.UserName,
                        Password = userRegisterModel.Password,
                        userAtual = userAtual
                    });
                    if (rows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    };
              

            }
        }


        public bool Deletar(string userAtual)
        {
            using (var db = _db)
            {
               
                    var deleteQuery = "DELETE FROM [USR_USUARIO] WHERE [user_usuario]=@userAtual";
                    var rows = _db.Execute(deleteQuery, new
                    {
                        userAtual = userAtual
                    });
                    if (rows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    };

            }
        }
    }
}
