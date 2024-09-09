using System.ComponentModel.DataAnnotations;

namespace InmoviliariaSarchioniAlfonzo.Models{
	public class LoginView
	{
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = "";
		[DataType(DataType.Password)]
		public string Password { get; set; } = "";
	}
}