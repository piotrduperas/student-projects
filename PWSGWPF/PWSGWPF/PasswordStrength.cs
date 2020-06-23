namespace PWSGWPF
{
	public enum PasswordStrength
	{
		Invalid,
		VeryWeak,
		Weak,
		Average,
		Strong,
		VeryStrong
	}

	public static class PasswordStrengthUtils
	{
		public static PasswordStrength CalculatePasswordStrength(string password)
		{
			if (string.IsNullOrEmpty(password))
				return PasswordStrength.Invalid;

			bool hasLowercase = false;
			bool hasUppercase = false;
			bool hasDigit = false;
			bool hasSpecialChar = false;

			foreach (var c in password)
			{
				if (char.IsDigit(c))
					hasDigit = true;
				else if (char.IsLower(c))
					hasLowercase = true;
				else if (char.IsUpper(c))
					hasUppercase = true;
				else
					hasSpecialChar = true;
			}

			int score = password.Length;
			if (hasLowercase) score += 2;
			if (hasUppercase) score += 2;
			if (hasDigit) score += 2;
			if (hasSpecialChar) score += 2;

			if (score < 10)
				return PasswordStrength.VeryWeak;
			if (score < 15)
				return PasswordStrength.Weak;
			if (score < 20)
				return PasswordStrength.Average;
			if (score < 25)
				return PasswordStrength.Strong;

			return PasswordStrength.VeryStrong;
		}
	}
}
