using portal.Models;

namespace portal.Repositories
{
    public interface IUserRepository
    {
        public bool ExisteUsuario(UserModel userModel);

        public bool Registrar(UserRegisterModel userRegister);

        public bool Recupear(UserRegisterModel userRegisterModel, string userAtual);

        public bool Deletar(string userAtual);

        public List<dynamic> ListAll();
    }
}
