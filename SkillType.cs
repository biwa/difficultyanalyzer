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

namespace CodeImp.DoomBuilder.DifficultyAnalyzer
{
	public struct SkillType
	{
		public string name;
		public string shortname;
		public float ammomulti;
		public string flag;

		public SkillType(string name, string shortname, float ammomulti, string flag)
		{
			this.name = name;
			this.shortname = shortname;
			this.ammomulti = ammomulti;
			this.flag = flag;
		}
	}
}