
using System.Xml.Serialization;

namespace ExpatSoftware.Helpers.ABTesting
{
	public class ABAlternative
	{
		//public int AlternativeID { get; set; }
		//public int ExperimentID { get; set; }
		public string Content { get; set; }
		//public string Lookup { get; set; }
		public int Participants { get; set; }
		public int Conversions { get; set; }

		[XmlIgnore]
		public int Index { get; set; }

		public double ConversionRate
		{
			get { return (double)Conversions / Participants; }
		}

		public string PrettyConversionRate
		{
			get { return (ConversionRate*100).ToString("0.##") + "%"; }
		}


		public ABAlternative()
		{
		}

		public ABAlternative(string content)
		{
			Content = content;
		}

		public void ScoreParticipation()
		{
			Participants++;
		}

		public void ScoreConversion()
		{
			Conversions++;
		}
	}
}
