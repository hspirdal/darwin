using System.ComponentModel.DataAnnotations;

namespace Darwin.Api.Identities
{
	public class IdentityModel
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
	}
}