using ANPAdmin.Data;

namespace ANPAdmin.Business
{
    public class Auth : IAuth
    {
        private IUserRepository _userRepository;

        public Auth(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AuthModel Login(string email, string senha)
        {
            var usuario = _userRepository.ObterPorEmailESenha(email, senha);

            //if (usuario == null || !usuario.Ativo) // Código correto
            if (usuario == null) // FIXME: Simulação de falha
                return null;

            return new AuthModel(usuario.Id, usuario.Nome, usuario.Email);
        }
    }
}
