using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Helpers.ABTesting
{
	public class Alternative
	{
		public int AlternativeID { get; set; }
		public int ExperimentID { get; set; }
		public string Content { get; set; }
		public string Lookup { get; set; }
		public int Participants { get; set; }
		public int Conversions { get; set; }

		public Alternative()
		{
		}

		public Alternative(string content)
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
