#region ================== Copyright (c) 2015 Boris Iwanski

/*
 * Copyright (c) 2015, 2016 Boris Iwanski
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

using System.Collections.Generic;
using System.Linq;

namespace CodeImp.DoomBuilder.DifficultyAnalyzer
{
	public struct Total
	{
		public string name;
		public List<float> skill;

		public Total(string name, int numskills)
		{
			this.name = name;
			this.skill = Enumerable.Repeat(0.0f, numskills).ToList();
		}

		public string[] ToStringArray()
		{
			List<string> l = new List<string>() { name };

			l.AddRange(skill.ConvertAll(o => o.ToString()));

			return l.ToArray();
		}
	}
}