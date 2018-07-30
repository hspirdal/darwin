namespace GameLib.Properties.Stats
{
	public static class HealthinessReader
	{
		public static string Measure(HitPoints hitPoints)
		{
			var percentHealthy = (double)hitPoints.Current / hitPoints.Max;

			if (percentHealthy >= 1.0)
			{
				return "Healthy";
			}
			else if (percentHealthy >= 0.80)
			{
				return "Slightly Injured";
			}
			else if (percentHealthy >= 0.40)
			{
				return "Injured";
			}
			else if (percentHealthy >= 0.15)
			{
				return "Badly Injured";
			}
			else if (percentHealthy > 0 && percentHealthy < 0.15)
			{
				return "Near Death";
			}
			else
			{
				return "Dead";
			}
		}
	}
}